/*
* Copyright (c) Jari Senhorst. All rights reserved.  
* Website: www.jarisenhorst.com
* Licensed under the MIT License. See LICENSE file in the project root for full license information.  
* 
*/

using Jstylezzz.Grid;
using Jstylezzz.Manager;
using UnityEngine;
using UnityEngine.UI;

namespace Jstylezzz.LevelEditor
{
	/// <summary>
	///
	/// </summary>
	public class MyLevelEditorSelectableTile : MonoBehaviour
	{
		private MyGridTileView _tileView;

		public void Initialize(MyGridTileView tile, Transform uiParent)
		{
			_tileView = tile;
			transform.SetParent(uiParent, false);
			GetComponent<Image>().sprite = tile.TileSprite;
		}

		public void OnClick()
		{
			MyGameState.Instance.LevelEditorManager.SetActivePrefab(_tileView);
		}
	}
}