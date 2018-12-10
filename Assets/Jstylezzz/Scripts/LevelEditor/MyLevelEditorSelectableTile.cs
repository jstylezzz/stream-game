/*
* Copyright (c) Jari Senhorst. All rights reserved.  
* Website: www.jarisenhorst.com
* Licensed under the MIT License. See LICENSE file in the project root for full license information.  
* 
*/

using UnityEngine;
using UnityEngine.UI;

namespace Jstylezzz.LevelEditor
{
	/// <summary>
	///
	/// </summary>
	public class MyLevelEditorSelectableTile : MonoBehaviour
	{
		public void OnClick()
		{
			FindObjectOfType<MyLevelEditorManager>().SetActivePrefab(GetComponent<Image>().sprite);
		}
	}
}