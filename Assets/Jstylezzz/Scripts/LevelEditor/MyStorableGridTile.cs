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
	[System.Serializable]
	public class MyStorableGridTile
	{
		public string JSONData { get { return JsonUtility.ToJson(this); } }

		public string PrefabName { get; }
		public Vector2Int GridTileLocation { get; }

		public MyStorableGridTile(Vector2Int location, string prefabname)
		{
			PrefabName = prefabname;
			GridTileLocation = location;
		}
	}
}