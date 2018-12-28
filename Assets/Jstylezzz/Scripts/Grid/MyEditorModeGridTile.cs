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
	public class MyEditorModeGridTile : MonoBehaviour
	{
		[SerializeField]
		private MonoBehaviour[] _disableInEditor;

		public void InitEditor()
		{
			for(int i = 0; i < _disableInEditor.Length; i++)
			{
				_disableInEditor[i].enabled = false;
			}
		}

		public void InitPlay()
		{
			for(int i = 0; i < _disableInEditor.Length; i++)
			{
				_disableInEditor[i].enabled = true;
			}
		}
	}
}