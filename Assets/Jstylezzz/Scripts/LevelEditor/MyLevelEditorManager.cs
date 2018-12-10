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

		private Sprite _activeSpritePrefab; //Actual sprite to place in world

		private void Awake()
		{
			foreach(GameObject g in _activeWorldTileCollection.GetAllTiles())
			{
				GameObject gO = Instantiate(_previewPrefab);
				gO.GetComponent<Image>().sprite = g.GetComponent<SpriteRenderer>().sprite;
				gO.transform.SetParent(_tileSelectionContent, false);
			}
		}

		public void SetActivePrefab(Sprite active)
		{
			_activeSpritePrefab = active;
		}

		public void Update()
		{
			if(_activeSpritePrefab && Input.GetMouseButtonDown(0))
			{
				MyGridTile tile = MyGameState.Instance.ActiveGrid.GridTileFromMousePosition(Input.mousePosition);
				if(tile != null)
				{
					if(tile.HasView)
					{
						tile.UnassignView();
					}

					GameObject g = new GameObject($"Tile[{tile.GridPosition.x},{tile.GridPosition.y}]");
					g.AddComponent<SpriteRenderer>().sprite = _activeSpritePrefab;
					MyGridTileView view = g.AddComponent<MyGridTileView>();
					tile.AssignView(view);
				}
			}
		}
	}
}