/*
* Copyright (c) Jari Senhorst. All rights reserved.  
* Website: www.jarisenhorst.com
* Licensed under the MIT License. See LICENSE file in the project root for full license information.  
* 
*/

using UnityEngine;

namespace Jstylezzz.Grid
{
	/// <summary>
	///
	/// </summary>
	[RequireComponent(typeof(SpriteRenderer))]
	public class MyGridTileView : MonoBehaviour
	{
		public string PrefabName { get { return _prefabName; } }
		public Sprite TileSprite { get { return _spriteRenderer.sprite; } }

		[SerializeField]
		private SpriteRenderer _spriteRenderer;

		[SerializeField]
		private string _prefabName;
	}

	public static class MyGridTileViewExtensions
	{
		public static string[,] GetPrefabNames(this MyGridTileView[,] tiles)
		{
			string[,] prefabNames = new string[tiles.GetLength(0), tiles.GetLength(1)];
			for(int y = 0; y < tiles.GetLength(1); y++)
			{
				for(int x = 0; x < tiles.GetLength(0); x++)
				{
					if(tiles[x,y] == null)
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