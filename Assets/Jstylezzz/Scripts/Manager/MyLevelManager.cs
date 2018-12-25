/*
* Copyright (c) Jari Senhorst. All rights reserved.  
* Website: www.jarisenhorst.com
* Licensed under the MIT License. See LICENSE file in the project root for full license information.  
* 
*/

using Jstylezzz.AssetCollections;
using Jstylezzz.Grid;
using Jstylezzz.StorageModules;
using StyloCore.Storage;
using UnityEngine;

namespace Jstylezzz.Manager
{
	/// <summary>
	///
	/// </summary>
	public class MyLevelManager
	{
		#region Properties

		public string ActiveLevelName { get; private set; }
		public MyWorldTileAssetCollection ActiveTileAssetCollection { get; private set; }

		#endregion

		#region Variables

		private readonly MyGameState _gameState;
		private readonly MyStorageManager _storageManager;

		#endregion

		public MyLevelManager(MyGameState gameState, MyStorageManager storageManager)
		{
			_gameState = gameState;
			_storageManager = storageManager;

			_storageManager.OnModuleLoaded += OnStorageModuleLoaded;
		}

		#region Public Methods

		public void SwitchToNewLevel(string newLevelName, int uniformSize, bool saveOldLevel = false)
		{
			if(saveOldLevel)
			{
				SaveLevelData();
			}

			ClearLevel();
			ActiveLevelName = newLevelName;

			_gameState.ActiveGrid.Initialize(uniformSize);
			_storageManager.RegisterStorageModule<MyLevelStorageModule>(new MyLevelStorageModule(ActiveLevelName), false);
			_storageManager.GetModule<MyLevelStorageModule>().DumpGridData(uniformSize, new string[uniformSize, uniformSize]);
		}

		public void SwitchLevel(string newLevelName, bool saveOldLevel = false)
		{
			if(saveOldLevel)
			{
				SaveLevelData();
			}

			ClearLevel();

			ActiveLevelName = newLevelName;
			LoadLevelFromStorage();
		}

		public void ClearLevel()
		{
			if(_gameState != null && _gameState.ActiveGrid != null && _gameState.ActiveGrid.Initialized)
			{
				_gameState.ActiveGrid.Deinitialize();
			}
			ActiveLevelName = string.Empty;
			_storageManager.UnregisterModule<MyLevelStorageModule>(false);
		}

		public void SaveLevelData()
		{
			if(!string.IsNullOrEmpty(ActiveLevelName) && _gameState != null && _gameState.ActiveGrid != null && _gameState.ActiveGrid.Initialized && _storageManager != null)
			{
				_storageManager.GetModule<MyLevelStorageModule>()?.DumpGridData(_gameState.ActiveGrid.GridSize, _gameState.ActiveGrid.Tiles.GetPrefabNames());
				_storageManager.SaveModule<MyLevelStorageModule>();
			}
		}

		public void LoadLevelFromStorage()
		{
			if(!string.IsNullOrEmpty(ActiveLevelName) && _gameState != null && _gameState.ActiveGrid != null && _storageManager != null)
			{
				if(_gameState.ActiveGrid.Initialized)
				{
					_gameState.ActiveGrid.Deinitialize();
				}

				_storageManager.RegisterStorageModule<MyLevelStorageModule>(new MyLevelStorageModule(ActiveLevelName), true);
			}
		}

		public void SetTile(string prefabName, Vector2Int gridPos)
		{
			MyGrid activeGrid = MyGameState.Instance.ActiveGrid;
			MyGridTile tile = activeGrid.Tiles[gridPos.x, gridPos.y];

			if(prefabName == null || ActiveTileAssetCollection == null || !ActiveTileAssetCollection.AssetDictionary.ContainsKey(prefabName))
			{
				return;
			}

			if(tile.HasView)
			{
				tile.UnassignView();
			}

			GameObject g = Object.Instantiate(ActiveTileAssetCollection.AssetDictionary[prefabName].gameObject);
			g.name = $"Tile[{gridPos.x},{gridPos.y}]";
			MyGridTileView view = g.GetComponent<MyGridTileView>();
			tile.AssignView(activeGrid.transform, view);
		}
		
		public void AssignAssetCollection(MyWorldTileAssetCollection worldTileAssetCollection)
		{
			ActiveTileAssetCollection = worldTileAssetCollection;
		}

		#endregion

		#region Private Methods

		private void OnStorageModuleLoaded(IMyStorageModule module)
		{
			if(module is MyLevelStorageModule)
			{
				MyLevelStorageModule levelModule = (MyLevelStorageModule)module;
				if(!string.IsNullOrEmpty(ActiveLevelName) && _gameState != null && _gameState.ActiveGrid != null && _storageManager != null && !_gameState.ActiveGrid.Initialized)
				{
					_gameState.ActiveGrid.Initialize(levelModule.UniformSize);
					for(int y = 0; y < levelModule.PrefabNames.GetLength(1); y++)
					{
						for(int x = 0; x < levelModule.PrefabNames.GetLength(0); x++)
						{
							SetTile(levelModule.PrefabNames[x, y], new Vector2Int(x, y));
						}
					}
				}
			}
		}

		#endregion
	}
}