/*
* Copyright (c) Jari Senhorst. All rights reserved.  
* Website: www.jarisenhorst.com
* Licensed under the MIT License. See LICENSE file in the project root for full license information.  
* 
*/

using Jstylezzz.Grid;
using Jstylezzz.Manager;
using UnityEngine;

namespace Jstylezzz.LevelEditor
{
	/// <summary>
	///
	/// </summary>
	public class MyLevelEditorManager : MonoBehaviour
	{
		[SerializeField]
		private Transform _tileSelectionContent;

		[SerializeField]
		private GameObject _previewPrefab; //UI element

		private MyGridTileView _activeTileViewPrefab; //Actual sprite to place in world
		

		private void Start()
		{
			MyGameState.Instance.RegisterLevelManager(this);

			//foreach(MyGridTileView g in MyGameState.Instance.LevelManager.ActiveTileAssetCollection.GetAllTiles())
			//{
			//	GameObject gO = Instantiate(_previewPrefab);
			//	gO.GetComponent<MyLevelEditorSelectableTile>().Initialize(g, _tileSelectionContent);
			//}
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
			if(!string.IsNullOrEmpty(MyGameState.Instance.LevelManager.ActiveLevelName))
			{
				MyGameState.Instance.LevelManager.SaveLevelData();
			}
			else
			{
				MyPopupManager.Instance.RequestGenericPopup("No Active level", "There is no level active that can be saved.", "Okay", MyPopupManager.Instance.CloseActiveGeneric);
			}
		}

		public void OnLoadClicked()
		{
			MyPopupManager.Instance.RequestPopup(MyPopupManager.LevelEditorLevelSelectPopupKey);
		}

		public void OnNewLevelClicked()
		{
			MyPopupManager.Instance.RequestPopup(MyPopupManager.CreateNewLevelPopupKey);
		}

		private void SetTile(Vector2Int position, string prefabName)
		{
			SetTile(MyGameState.Instance.ActiveGrid.GridTileFromGridPos(position), prefabName);
		}

		private void SetTile(MyGridTile tile, string prefabName)
		{
			MyGameState.Instance.LevelManager.SetTile(prefabName, tile.GridPosition);
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