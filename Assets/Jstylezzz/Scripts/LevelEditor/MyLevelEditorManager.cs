/*
* Copyright (c) Jari Senhorst. All rights reserved.  
* Website: www.jarisenhorst.com
* Licensed under the MIT License. See LICENSE file in the project root for full license information.  
* 
*/

using Jstylezzz.Grid;
using Jstylezzz.Manager;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Jstylezzz.LevelEditor
{
	/// <summary>
	///
	/// </summary>
	public class MyLevelEditorManager : MonoBehaviour
	{
		private const float CameraMovementSpeed = 3.5f;
		private const float CameraZoomSpeed = 0.9f;

		[SerializeField]
		private Transform _tileSelectionContent;

		[SerializeField]
		private GameObject _previewPrefab; //UI element

		private MyGridTileView _activeTileViewPrefab; //Actual sprite to place in world
		

		private void Start()
		{
			MyGameState.Instance.RegisterLevelManager(this);

			foreach(MyGridTileView g in MyGameState.Instance.AssetManager.DefaultSet.GetAllTiles())
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

		private void UnsetTile(MyGridTile tile)
		{
			MyGameState.Instance.LevelManager.UnsetTile(tile.GridPosition);
		}

		private void SetTile(MyGridTile tile, string prefabName)
		{
			MyGameState.Instance.LevelManager.SetTile(prefabName, tile.GridPosition);
		}

		private void PerformCameraManipulation()
		{
			Vector2 movement = Vector2.zero;
			if(Input.GetKey(KeyCode.A))
			{
				movement.x -= CameraMovementSpeed;
			}
			else if(Input.GetKey(KeyCode.D))
			{
				movement.x += CameraMovementSpeed;
			}

			if(Input.GetKey(KeyCode.W))
			{
				movement.y += CameraMovementSpeed;
			}
			else if(Input.GetKey(KeyCode.S))
			{
				movement.y -= CameraMovementSpeed;
			}

			MyGameState.Instance.CameraOperator.RelativeMoveActiveCamera(movement * Time.deltaTime);
			MyGameState.Instance.CameraOperator.RelativeZoomActiveCamera(Input.mouseScrollDelta.y);
		}

		private void PerformTilePlacementCheck()
		{
			if(!EventSystem.current.IsPointerOverGameObject())
			{
				if(Input.GetMouseButtonDown(1))
				{
					MyGridTile tile = MyGameState.Instance.ActiveGrid.GridTileFromMousePosition(Input.mousePosition);
					if(tile != null && tile.HasView)
					{
						UnsetTile(tile);
					}
				}
				else if(_activeTileViewPrefab && Input.GetMouseButtonDown(0))
				{
					MyGridTile tile = MyGameState.Instance.ActiveGrid.GridTileFromMousePosition(Input.mousePosition);
					if(tile != null && !tile.HasView)
					{
						SetTile(tile, _activeTileViewPrefab.PrefabName);
					}
				}
			}
		}

		public void Update()
		{
			PerformTilePlacementCheck();
			PerformCameraManipulation();
		}
	}
}