/*
* Copyright (c) Jari Senhorst. All rights reserved.  
* Website: www.jarisenhorst.com
* Licensed under the MIT License. See LICENSE file in the project root for full license information.  
* 
*/

using StyloCore.Storage;
using System;
using UnityEngine;
using System.IO;

namespace Jstylezzz.StorageModules
{
	/// <summary>
	///	This storage module stores player stats.
	/// </summary>
	public class MyLevelStorageModule : IMyStorageModule
	{
		#region Consts

		private const string LevelFolder = "levels/";

		#endregion

		#region Variables

		private string _levelName;
		private int _uniformSize;

		#endregion

		#region Properties (interface)

		public string ModuleName { get { return "Grid Tile Data"; } }

		public string RelativePath { get { return LevelFolder + _levelName + "/"; } }

		public string RelativeFilePath { get { return RelativePath + _levelName + "_tileData.stylz"; } }

		public bool IsLoaded { get; set; } = false;

		#endregion

		#region Properties

		public string[,] PrefabNames { get; private set; }
		public int UniformSize { get; private set; }

		#endregion

		public MyLevelStorageModule(string levelName)
		{
			_levelName = levelName;
		}

		#region Public Methods

		public void DumpGridData(int uniformSize, string[,] prefabNames)
		{
			UniformSize = uniformSize;
			PrefabNames = prefabNames;
		}

		#endregion

		#region Public Methods (interface)

		public void OnModuleDataGeneratedFirstTime()
		{

		}

		public string GetJSON()
		{
			
			return JsonUtility.ToJson(new MyStorableVariables(UniformSize, PrefabNames));
		}

		public void InitFromJSON(string json)
		{
			try
			{
				MyStorableVariables v = JsonUtility.FromJson<MyStorableVariables>(json);

				if(v == null)
					v = new MyStorableVariables();

				UniformSize = v.UniformSize;
				PrefabNames = v.GetJaggedPrefabNames();
			}
			catch(Exception e)
			{
				Debug.LogError($"Could not init module {ModuleName} from JSON.\n{e.Message}\n{e.StackTrace}");
			}
			IsLoaded = true;
		}

		#endregion

		#region Public Static Methods

		public static string[] GetPotentialLevels()
		{
			string[] levels = Directory.GetDirectories(MyStorageManager.StorageRoot + LevelFolder);
			for(int i = 0; i < levels.Length; i++)
			{
				int lastIdx = levels[i].LastIndexOf('/') + 1;
				levels[i] = levels[i].Remove(0, lastIdx);
			}
			return levels;
		}

		#endregion

		#region Storable

		private class MyStorableVariables
		{
			public int UniformSize;
			public string[] PrefabNames;

			/// <summary>
			/// JSON constructor
			/// </summary>
			public MyStorableVariables()
			{

			}

			/// <summary>
			/// Save constructor
			/// </summary>
			public MyStorableVariables(int uniformSize, string[,] prefabNames)
			{
				UniformSize = uniformSize;
				PrefabNames = JaggedToSingleDimension(prefabNames);
			}

			public string[,] GetJaggedPrefabNames()
			{
				return SingleToJagged(PrefabNames);
			}

			private string[,] SingleToJagged(string[] singleStrings)
			{
				if(singleStrings == null)
				{
					return new string[0, 0];
				}

				int uniformSize = Mathf.RoundToInt(Mathf.Sqrt(singleStrings.Length));
				string[,] jagged = new string[uniformSize, uniformSize];

				int x = 0;
				int y = 0;
				for(int i = 0; i < singleStrings.Length; i++)
				{
					jagged[x, y] = singleStrings[i];

					if(i > 0 && y == uniformSize - 1)
					{
						x++;
						y = 0;
					}
					else
					{
						y++;
					}

					
				}

				return jagged;
			}

			private string[] JaggedToSingleDimension(string[,] jaggedStrings)
			{
				if(jaggedStrings == null)
				{
					return new string[0];
				}

				int xLen = jaggedStrings.GetLength(0);
				int yLen = jaggedStrings.GetLength(1);

				string[] single = new string[xLen * yLen];

				for(int y = 0; y < yLen; y++)
				{
					for(int x = 0; x < xLen; x++)
					{
						single[x * yLen + y] = jaggedStrings[x, y];
					}
				}

				return single;
			}
		}

		#endregion
	}
}