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
		#region Properties

		public Dictionary<string, MyGridTileView> AssetDictionary => GetPrefabDictionary();

		#endregion

		#region Editor Variables

		[SerializeField]
		private string _collectionKey;

		[SerializeField]
		private MyGridTileView[] _tiles;

		#endregion

		#region Variables

		private Dictionary<string, MyGridTileView> _prefabDictionary;

		#endregion

		#region Public Methods

		public MyGridTileView[] GetAllTiles()
		{
			return _tiles;
		}

		#endregion

		#region Private Methods

		private Dictionary<string, MyGridTileView> GetPrefabDictionary()
		{
			if(_prefabDictionary == null)
			{
				_prefabDictionary = new Dictionary<string, MyGridTileView>();
				for(int i = 0; i < _tiles.Length; i++)
				{
					_prefabDictionary.Add(_tiles[i].PrefabName, _tiles[i]);
				}
			}

			return new Dictionary<string, MyGridTileView>(_prefabDictionary);
		}

		#endregion
	}
}