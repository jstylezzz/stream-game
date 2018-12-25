/*
* Copyright (c) Jari Senhorst. All rights reserved.  
* Website: www.jarisenhorst.com
* Licensed under the MIT License. See LICENSE file in the project root for full license information.  
* 
*/

namespace Jstylezzz.Grid
{
	/// <summary>
	///
	/// </summary>
	public static class MyGridTileViewExtensions
	{
		public static string [,] GetPrefabNames(this MyGridTile[,] gridTiles)
		{
			string[,] prefabNames = new string[gridTiles.GetLength(0), gridTiles.GetLength(1)];
			for(int y = 0; y < gridTiles.GetLength(1); y++)
			{
				for(int x = 0; x < gridTiles.GetLength(0); x++)
				{
					if(!gridTiles[x, y].HasView)
					{
						continue;
					}

					prefabNames[x, y] = gridTiles[x, y].View.PrefabName;
				}
			}

			return prefabNames;
		}

		public static string[,] GetPrefabNames(this MyGridTileView[,] tiles)
		{
			string[,] prefabNames = new string[tiles.GetLength(0), tiles.GetLength(1)];
			for(int y = 0; y < tiles.GetLength(1); y++)
			{
				for(int x = 0; x < tiles.GetLength(0); x++)
				{
					if(tiles[x, y] == null)
					{
						continue;
					}

					prefabNames[x, y] = tiles[x, y].PrefabName;
				}
			}

			return prefabNames;
		}
	}
}