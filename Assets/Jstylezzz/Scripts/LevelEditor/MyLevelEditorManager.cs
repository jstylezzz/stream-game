/*
* Copyright (c) Jari Senhorst. All rights reserved.  
* Website: www.jarisenhorst.com
* Licensed under the MIT License. See LICENSE file in the project root for full license information.  
* 
*/

using Jstylezzz.AssetCollections;
using Jstylezzz.Grid;
using Jstylezzz.Manager;
using Jstylezzz.Storage;
using Jstylezzz.Storage.Modules;
using System.Collections.Generic;
using UnityEngine;

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

		private MyGridTileView[,] _gridTileViews;

		private MyGridTileView _activeTileViewPrefab; //Actual sprite to place in world
		

		private void Start()
		{
			MyGameState.Instance.RegisterLevelManager(this);

			int gridSize = MyGameState.Instance.ActiveGrid.GridSize;
			_gridTileViews = new MyGridTileView[gridSize, gridSize];
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

		public void OnSaveClicked()
		{
			MyStorageManager.Instance.GetModule<MyLevelStorageModule>().DumpGridData(_gridTileViews.GetPrefabNames());
			MyStorageManager.Instance.SaveModule<MyLevelStorageModule>();
		}

		public void OnLoadClicked()
		{
			string[,] prefabs = MyStorageManager.Instance.GetModule<MyLevelStorageModule>().PrefabNames;
			for(int y = 0; y < prefabs.GetLength(1); y++)
			{
				for(int x = 0; x < prefabs.GetLength(0); x++)
				{
					SetTile(new Vector2Int(x, y), prefabs[x, y]);
				}
			}
		}

		private void SetTile(Vector2Int position, string prefabName)
		{
			SetTile(MyGameState.Instance.ActiveGrid.GridTileFromGridPos(position), prefabName);
		}

		private void SetTile(MyGridTile tile, string prefabName)
		{
			if(prefabName == null || !_activeWorldTileCollection.AssetDictionary.ContainsKey(prefabName))
			{
				_gridTileViews[tile.GridPosition.x, tile.GridPosition.y] = null;
				tile.UnassignView();
				return;
			}

			if(tile.HasView)
			{
				_gridTileViews[tile.GridPosition.x, tile.GridPosition.y] = null;
				tile.UnassignView();
			}

			string activePrefabName = prefabName;

			GameObject g = Instantiate(_activeWorldTileCollection.AssetDictionary[activePrefabName].gameObject);
			g.name = $"Tile[{tile.GridPosition.x},{tile.GridPosition.y}]";
			MyGridTileView view = g.GetComponent<MyGridTileView>();
			_gridTileViews[tile.GridPosition.x, tile.GridPosition.y] = view;
			tile.AssignView(view);
		}

		public void Update()
		{
			if(_activeTileViewPrefab && Input.GetMouseButtonDown(0))
			{
				MyGridTile tile = MyGameState.Instance.ActiveGrid.GridTileFromMousePosition(Input.mousePosition);
				if(tile != null)
				{
					SetTile(tile, _activeTileViewPrefab.PrefabName);
				}
			}
		}
	}
}