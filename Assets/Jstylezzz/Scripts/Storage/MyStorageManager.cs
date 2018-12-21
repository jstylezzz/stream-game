/*
* Copyright (c) Jari Senhorst. All rights reserved.  
* Website: www.jarisenhorst.com
* Licensed under the MIT License. See LICENSE file in the project root for full license information.  
* 
*/

using StyloCore;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Jstylezzz.Storage
{
	/// <summary>
	/// This class manages all storage related tasks.
	/// </summary>
	public class MyStorageManager : MySingleton<MyStorageManager>
	{
		#region Delegates

		public delegate void OnModuleLoadedEventHandler(IMyStorageModule module);

		#endregion

		#region Events

		public event OnModuleLoadedEventHandler OnModuleLoaded;

		#endregion

		#region Properties

		/// <summary>
		/// The storage manager's root.
		/// </summary>
		public string StorageRoot
		{
			get
			{
				return Application.persistentDataPath + "/DataVault/";
			}
		}

		#endregion

		#region Variables

		/// <summary>
		/// Registered storage modules.
		/// </summary>
		private List<IMyStorageModule> _storageModules = new List<IMyStorageModule>();
		private Dictionary<Type, IMyStorageModule> _storageModuleDictionary = new Dictionary<Type, IMyStorageModule>();

		#endregion

		#region Public Methods

		/// <summary>
		/// Save a registered module of certain type.
		/// </summary>
		/// <param name="moduleType">The moduletype to save.</param>
		public void SaveModule<T>()
		{
			Type moduleType = typeof(T);
			if(!_storageModuleDictionary.ContainsKey(moduleType))
			{
				Debug.LogWarning($"Module with type {moduleType} not registered on module save.");
				return;
			}

			IMyStorageModule module = _storageModuleDictionary[moduleType];
			if(module == null)
			{
				Debug.LogError($"Could not find module {moduleType}. Perhaps it was not registered?");
			}
			WriteModuleData(module);
		}

		/// <summary>
		/// Save a certain module.
		/// </summary>
		/// <param name="module">The module to save.</param>
		public void SaveModule(IMyStorageModule module)
		{
			WriteModuleData(module);
		}

		/// <summary>
		/// Load all registered modules from the file storage.
		/// </summary>
		public void LoadAllModules()
		{
			foreach(IMyStorageModule module in _storageModules)
			{
				LoadModuleFromFile(module);
			}
		}

		/// <summary>
		/// Register a storage module.
		/// </summary>
		/// <param name="module">The StorageModule to register.</param>
		public void RegisterStorageModule(IMyStorageModule module, Type t)
		{
			if(_storageModules.Contains(module))
			{
				Debug.LogWarning($"Attempted to register duplicate storage module {module.ModuleName}");
				return;
			}

			_storageModuleDictionary.Add(t, module);
			_storageModules.Add(module);
		}

		/// <summary>
		/// Unregister the module from the active manager.
		/// </summary>
		/// <param name="module">Module instance to unregister.</param>
		/// <param name="moduleType">The Type of the module.</param>
		/// <param name="saveOnUnload">Whether to save the module before unloading.</param>
		public void UnregisterModule<T>(IMyStorageModule module, bool saveOnUnload)
		{
			Type moduleType = typeof(T);
			if(!_storageModuleDictionary.ContainsKey(moduleType))
			{
				Debug.LogWarning($"Attempted to unregister non-existant storage module type {moduleType}");
				return;
			}

			if(saveOnUnload)
			{
				SaveModule<T>();
			}

			_storageModuleDictionary.Remove(moduleType);
			_storageModules.Remove(module);
		}

		/// <summary>
		/// Get the moduletype as object T.
		/// </summary>
		/// <typeparam name="T">The type of object we want back.</typeparam>
		/// <param name="t">The moduletype to get.</param>
		/// <returns>StorageModule as T if found.</returns>
		public T GetModule<T>()
		{
			Type t = typeof(T);
			if(!_storageModuleDictionary.ContainsKey(t))
			{
				Debug.LogWarning($"No StorageModule with type {t} registered.");
				throw new Exception("Non Existant");
			}

			try
			{
				T module = (T)_storageModuleDictionary[t];
				if(module == null)
				{
					Debug.LogWarning($"Could not get module with type {t.Name}.");
				}

				return module;
			}
			catch(Exception e)
			{
				Debug.LogWarning($"Could not get module with type {t.Name}.\n{e.Message}\n{e.StackTrace}");
			}

			throw new Exception("Not Found");
		}

		/// <summary>
		/// Get the moduletype as object T.
		/// </summary>
		/// <typeparam name="T">The type of object we want back.</typeparam>
		/// <param name="module">The module to get as T.</param>
		/// <returns>StorageModule as T if found.</returns>
		public T GetModule<T>(IMyStorageModule module)
		{
			try
			{
				return (T)module;
			}
			catch(Exception e)
			{
				Debug.LogWarning($"Could not get module with type {module.ModuleName}.\n{e.Message}\n{e.StackTrace}");
			}

			throw new Exception("Not Found");
		}

		/// <summary>
		/// Clear ALL the game data.
		/// </summary>
		public void ClearAll()
		{
			foreach(IMyStorageModule module in _storageModules)
			{
				DeleteModuleData(module);
			}
		}

		/// <summary>
		/// Unload all registered modules.
		/// </summary>
		public void DeinitModules()
		{
			_storageModuleDictionary.Clear();
			_storageModules.Clear();
			_storageModules.TrimExcess();
		}

		#endregion

		#region Private Methods

		/// <summary>
		/// Write the StorageModule's data to disk.
		/// </summary>
		/// <param name="module">StorageModule to save.</param>
		private void WriteModuleData(IMyStorageModule module)
		{
			try
			{
				if(!File.Exists(FullModuleFilePath(module)))
				{
					if(!Directory.Exists(FullModulePath(module)))
						Directory.CreateDirectory(FullModulePath(module));
					if(!File.Exists(FullModuleFilePath(module)))
						File.Create(FullModuleFilePath(module)).Close();
				}
				File.WriteAllText(FullModuleFilePath(module), module.GetJSON());
			}
			catch(Exception e)
			{
				Debug.LogError($"Something went wrong while saving module {module.ModuleName}.\n{e.Message}\n{e.StackTrace}");
			}
		}

		/// <summary>
		/// Load a StorageModule's data from disk.
		/// </summary>
		/// <param name="module">The StorageModule to load.</param>
		private void LoadModuleFromFile(IMyStorageModule module)
		{
			try
			{
				if(File.Exists(FullModuleFilePath(module)))
				{
					string data = File.ReadAllText(FullModuleFilePath(module));
					module.InitFromJSON(data);
				}
				else
				{
					if(!Directory.Exists(FullModulePath(module)))
						Directory.CreateDirectory(FullModulePath(module));
					if(!File.Exists(FullModuleFilePath(module)))
						File.Create(FullModuleFilePath(module)).Close();

					module.OnModuleDataGeneratedFirstTime();

					SaveModule(module);
					module.IsLoaded = true;
				}
			}
			catch(Exception e)
			{
				Debug.LogError($"Something went wrong while loading module {module.ModuleName}.\n{e.Message}\n{e.StackTrace}");
			}
			OnModuleLoaded?.Invoke(module);
		}

		/// <summary>
		/// Delete a module's file.
		/// </summary>
		private void DeleteModuleData(IMyStorageModule module)
		{
			try
			{
				string moduleFile = FullModuleFilePath(module);
				if(File.Exists(moduleFile))
					File.Delete(moduleFile);
			}
			catch(Exception e)
			{
				Debug.LogError($"Could not find module data file {module.ModuleName}.\n{e.Message}\n{e.StackTrace}");
			}
		}

		/// <summary>
		/// Get the full path in which the StorageModule's file resides.
		/// </summary>
		/// <param name="module">The StorageModule we want the path for.</param>
		/// <returns>Full path to StorageModule's file folder.</returns>
		private string FullModulePath(IMyStorageModule module)
		{
			return StorageRoot + module.RelativePath;
		}

		/// <summary>
		/// Get the full path to the StorageModule's file.
		/// </summary>
		/// <param name="module">The StorageModule we want the filepath for.</param>
		/// <returns>Full path to StorageModule's file.</returns>
		private string FullModuleFilePath(IMyStorageModule module)
		{
			return StorageRoot + module.RelativeFilePath;
		}

		#endregion
	}
}