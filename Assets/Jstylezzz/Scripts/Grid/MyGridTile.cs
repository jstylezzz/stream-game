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
	}
}