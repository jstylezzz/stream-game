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

		[SerializeField]
		private int _gridSize;

		[SerializeField]
		private GameObject _prefabView;

		private MyGridTile[,] _gridTiles;

		private void Awake()
		{
			StartCoroutine(GenerateGrid());
		}

		private IEnumerator GenerateGrid()
		{
			_gridTiles = new MyGridTile[_gridSize, _gridSize];

			for(int y = 0; y < _gridSize; y++)
			{
				for(int x = 0; x < _gridSize; x++)
				{
					GameObject g = Instantiate(_prefabView);
					_gridTiles[x, y] = new MyGridTile(new Vector2Int(x, y));
					_gridTiles[x, y].AssignView(g.GetComponent<MyGridTileView>());
					g.name = $"Tile[{x}, {y}]";
				}
			}
			yield return null;
		}

		public MyGridTile GridTileFromMousePosition(Vector3 mousePos)
		{
			Camera mainCam = MyGameState.Instance.CameraOperator.MainCamera;
			Vector3 worldPosition = mainCam.ScreenToWorldPoint(mousePos);
			Vector2Int gridIndex = new Vector2Int(Mathf.RoundToInt(worldPosition.x / GridTileSize), Mathf.RoundToInt(worldPosition.y / GridTileSize));
			Debug.Log(gridIndex);
			if((gridIndex.x > 0 && gridIndex.x < _gridSize) && (gridIndex.y > 0 && gridIndex.y < _gridSize))
			{
				return _gridTiles[gridIndex.x, gridIndex.y];
			}
			return null;
		}
	}
}