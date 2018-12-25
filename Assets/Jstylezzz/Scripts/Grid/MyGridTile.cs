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
		#region Properties

		public Vector2Int GridPosition { get; }
		public bool HasView { get { return View != null; } }
		public MyGridTileView View { get; private set; }

		#endregion
		#region Variables

		#endregion

		public MyGridTile(Vector2Int positionInGrid)
		{
			GridPosition = positionInGrid;
		}

		#region Public Functions

		public void AssignView(Transform gridParent, MyGridTileView view)
		{
			View = view;
			View.transform.SetParent(gridParent, false);
			View.transform.localPosition = new Vector3(GridPosition.x * MyGrid.GridTileSize, GridPosition.y * MyGrid.GridTileSize, 0);
		}

		public void UnassignView()
		{
			if(View == null)
				return;

			Object.Destroy(View.gameObject);
			View = null;
		}

		#endregion
	}
}