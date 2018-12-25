/*
* Copyright (c) Jari Senhorst. All rights reserved.  
* Website: www.jarisenhorst.com
* Licensed under the MIT License. See LICENSE file in the project root for full license information.  
* 
*/

using Jstylezzz.Manager;
using System;
using System.Collections;
using UnityEngine;

namespace Jstylezzz.Grid
{
	/// <summary>
	///
	/// </summary>
	public class MyGrid : MonoBehaviour
	{
		#region Consts

		public const float GridTileSize = 0.32f;

		private const int LoadFractions = 2;

		#endregion

		#region Properties

		public int GridSize { get; private set; }
		public MyGridTile[,] Tiles { get; private set; }
		public bool Initialized { get; private set; }

		#endregion

		#region Editor Variables

		[SerializeField]
		private SpriteRenderer _overlayRenderer;

		#endregion

		#region Variables

		private Action _onloadCallback;
		private int _loadFractionsComplete = 0;

		#endregion

		#region Lifecycle

		private void Awake()
		{
			MyGameState.Instance.RegisterActiveGrid(this);
			_overlayRenderer.enabled = false;
		}

		#endregion

		public void Initialize(int uniformSize, Action onloadCallback = null)
		{
			if(Initialized)
			{
				Deinitialize();
			}

			GridSize = uniformSize;
			_onloadCallback = onloadCallback;
			_loadFractionsComplete = 0;
			StartCoroutine(GenerateGrid());
			StartCoroutine(GenerateOverlay());
		}

		#region Public Methods

		public void Deinitialize()
		{
			for(int y = 0; y < GridSize; y++)
			{
				for(int x = 0; x < GridSize; x++)
				{
					Tiles[x, y].UnassignView();
				}
			}

			Tiles = null;
			_overlayRenderer.enabled = false;
			Initialized = false;
		}

		public Vector2 GridTilePositionToLocalWorldPosition(MyGridTile tile)
		{
			return new Vector3(GridTileSize * tile.GridPosition.x, GridTileSize * tile.GridPosition.y, transform.position.z);
		}

		public MyGridTile GridTileFromMousePosition(Vector3 mousePos)
		{
			Camera mainCam = MyGameState.Instance.CameraOperator.MainCamera;
			Vector3 worldPosition = mainCam.ScreenToWorldPoint(mousePos) - transform.position;
			Vector2Int gridIndex = new Vector2Int(Mathf.RoundToInt(worldPosition.x / GridTileSize), Mathf.RoundToInt(worldPosition.y / GridTileSize));

			if((gridIndex.x >= 0 && gridIndex.x < GridSize) && (gridIndex.y >= 0 && gridIndex.y < GridSize))
			{
				return Tiles[gridIndex.x, gridIndex.y];
			}
			return null;
		}

		public MyGridTile GridTileFromGridPos(Vector2Int pos)
		{
			return Tiles[pos.x, pos.y];
		}

		#endregion

		#region Private Methods

		private IEnumerator GenerateGrid()
		{
			Tiles = new MyGridTile[GridSize, GridSize];

			for(int y = 0; y < GridSize; y++)
			{
				for(int x = 0; x < GridSize; x++)
				{
					Tiles[x, y] = new MyGridTile(new Vector2Int(x, y));
				}
			}
			OnLoadFractionComplete();
			yield return null;
		}

		private IEnumerator GenerateOverlay()
		{
			_overlayRenderer.enabled = true;
			_overlayRenderer.size = Vector2.one * (GridTileSize * GridSize);
			Vector2 bottomLeft = GridTilePositionToLocalWorldPosition(Tiles[0, 0]);
			Vector2 topRight = GridTilePositionToLocalWorldPosition(Tiles[GridSize - 1, GridSize - 1]);
			Vector2 center = (bottomLeft + topRight) / 2f;
			_overlayRenderer.transform.localPosition = center;
			OnLoadFractionComplete();
			yield return null;
		}

		private void OnLoadFractionComplete()
		{
			_loadFractionsComplete++;

			if(_loadFractionsComplete == LoadFractions)
			{
				_onloadCallback?.Invoke();
				Initialized = true;
			}
		}

		#endregion
	}
}