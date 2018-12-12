/*
* Copyright (c) Jari Senhorst. All rights reserved.  
* Website: www.jarisenhorst.com
* Licensed under the MIT License. See LICENSE file in the project root for full license information.  
* 
*/

using Jstylezzz.Grid;
using System.Collections.Generic;
using UnityEngine;

namespace Jstylezzz.AssetCollections
{
	/// <summary>
	///
	/// </summary>
	[CreateAssetMenu(fileName = "DefaultTileCollection", menuName = "Jstylezzz/World/Tile Collection")]
	public class MyWorldTileAssetCollection : ScriptableObject
	{
		[SerializeField]
		private MyGridTileView[] _tiles;

		public MyGridTileView[] GetAllTiles()
		{
			return _tiles;
		}
	}
}