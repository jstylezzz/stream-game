/*
* Copyright (c) Jari Senhorst. All rights reserved.  
* Website: www.jarisenhorst.com
* Licensed under the MIT License. See LICENSE file in the project root for full license information.  
* 
*/

using Jstylezzz.Manager;
using System.Collections;
using UnityEngine;

namespace Jstylezzz.Grid
{
	/// <summary>
	///
	/// </summary>
	public class MyGrid : MonoBehaviour
	{
		public const float GridTileSize = 0.32f;

		public int GridSize
		{
			get
			{
				return _gridSize;
			}
		}

		[SerializeField]
		private int _gridSize;

		[SerializeField]
		private GameObject _prefabView;

		[SerializeField]
		private Texture2D _overlaySample;

		[SerializeField]
		private SpriteRenderer _overlayRenderer;

		private MyGridTile[,] _gridTiles;

		private void Awake()
		{
			MyGameState.Instance.RegisterActiveGrid(this);
			StartCoroutine(GenerateGrid());
			StartCoroutine(GenerateOverlay());
		}

		private IEnumerator GenerateGrid()
		{
			_gridTiles = new MyGridTile[_gridSize, _gridSize];

			for(int y = 0; y < _gridSize; y++)
			{
				for(int x = 0; x < _gridSize; x++)
				{
					_gridTiles[x, y] = new MyGridTile(this, new Vector2Int(x, y));
				}
			}
			yield return null;
		}

		private IEnumerator GenerateOverlay()
		{
			_overlayRenderer.size = Vector2.one * (GridTileSize * _gridSize);
			Vector2 bottomLeft = GridTilePositionToLocalWorldPosition(_gridTiles[0, 0]);
			Vector2 topRight = GridTilePositionToLocalWorldPosition(_gridTiles[_gridSize - 1, _gridSize - 1]);
			Vector2 center = (bottomLeft + topRight) / 2f;
			_overlayRenderer.transform.localPosition = center;
			yield return null;
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

			if((gridIndex.x >= 0 && gridIndex.x < _gridSize) && (gridIndex.y >= 0 && gridIndex.y < _gridSize))
			{
				return _gridTiles[gridIndex.x, gridIndex.y];
			}
			return null;
		}

		public MyGridTile GridTileFromGridPos(Vector2Int pos)
		{
			return _gridTiles[pos.x, pos.y];
		}
	}
}