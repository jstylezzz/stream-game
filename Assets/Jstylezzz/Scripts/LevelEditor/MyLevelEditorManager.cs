/*
* Copyright (c) Jari Senhorst. All rights reserved.  
* Website: www.jarisenhorst.com
* Licensed under the MIT License. See LICENSE file in the project root for full license information.  
* 
*/

using Jstylezzz.AssetCollections;
using Jstylezzz.Grid;
using Jstylezzz.Manager;
using UnityEngine;
using UnityEngine.UI;

namespace Jstylezzz.LevelEditor
{
	/// <summary>
	///
	/// </summary>
	public class MyLevelEditorManager : MonoBehaviour
	{
		[SerializeField]
		private MyWorldTileAssetCollection _activeWorldTileCollection;

		[SerializeField]
		private Transform _tileSelectionContent;

		[SerializeField]
		private GameObject _previewPrefab; //UI element

		private MyGridTileView _activeTileViewPrefab; //Actual sprite to place in world

		private void Awake()
		{
			MyGameState.Instance.RegisterLevelManager(this);
			foreach(MyGridTileView g in _activeWorldTileCollection.GetAllTiles())
			{
				GameObject gO = Instantiate(_previewPrefab);
				gO.GetComponent<MyLevelEditorSelectableTile>().Initialize(g, _tileSelectionContent);
			}
		}

		private void OnDestroy()
		{
			MyGameState.Instance.UnregisterLevelEditorManager();
		}

		public void SetActivePrefab(MyGridTileView active)
		{
			_activeTileViewPrefab = active;
		}

		public void Update()
		{
			if(_activeTileViewPrefab && Input.GetMouseButtonDown(0))
			{
				MyGridTile tile = MyGameState.Instance.ActiveGrid.GridTileFromMousePosition(Input.mousePosition);
				if(tile != null)
				{
					if(tile.HasView)
					{
						tile.UnassignView();
					}

					GameObject g = new GameObject($"Tile[{tile.GridPosition.x},{tile.GridPosition.y}]");
					g.AddComponent<SpriteRenderer>().sprite = _activeTileViewPrefab.TileSprite;
					MyGridTileView view = g.AddComponent<MyGridTileView>();
					tile.AssignView(view);
				}
			}
		}
	}
}