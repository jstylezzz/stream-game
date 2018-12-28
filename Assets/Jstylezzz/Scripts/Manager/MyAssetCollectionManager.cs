/*
* Copyright (c) Jari Senhorst. All rights reserved.  
* Website: www.jarisenhorst.com
* Licensed under the MIT License. See LICENSE file in the project root for full license information.  
* 
*/

using Jstylezzz.AssetCollections;
using Jstylezzz.Grid;
using UnityEngine;

namespace Jstylezzz.Manager
{
	/// <summary>
	///
	/// </summary>
	public class MyAssetCollectionManager : MonoBehaviour
	{
		#region Properties

		public MyWorldTileAssetCollection DefaultSet { get { return _defaultSet; } }

		#endregion

		#region Editor Variables

		[SerializeField]
		private MyWorldTileAssetCollection _defaultSet;

		#endregion

		#region Lifecycle

		private void Awake()
		{
			MyGameState.Instance.RegisterAssetManager(this);
		}

		#endregion
	}
}