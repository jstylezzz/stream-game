/*
* Copyright (c) Jari Senhorst. All rights reserved.  
* Website: www.jarisenhorst.com
* Licensed under the MIT License. See LICENSE file in the project root for full license information.  
* 
*/

using UnityEngine;

namespace Jstylezzz.Grid
{
	/// <summary>
	///
	/// </summary>
	public class MyGridTile
	{
		public Vector2Int GridPosition { get { return _positionInGrid; } }
		public bool HasView { get { return _tileView != null; } }

		private Vector2Int _positionInGrid;
		private MyGridTileView _tileView;

		public MyGridTile(Vector2Int positionInGrid)
		{
			_positionInGrid = positionInGrid;
		}

		public void AssignView(MyGridTileView view)
		{
			_tileView = view;
			_tileView.transform.localPosition = new Vector3(_positionInGrid.x * MyGrid.GridTileSize, _positionInGrid.y * MyGrid.GridTileSize, 0);
		}

		public void UnassignView()
		{
			if(_tileView == null)
				return;

			Object.Destroy(_tileView.gameObject);
			_tileView = null;
		}
	}
}