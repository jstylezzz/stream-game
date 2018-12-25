/*
* Copyright (c) Jari Senhorst. All rights reserved.  
* Website: www.jarisenhorst.com
* Licensed under the MIT License. See LICENSE file in the project root for full license information.  
* 
*/

using Jstylezzz.Manager;
using UnityEngine;
using UnityEngine.UI;

namespace Jstylezzz.Popups
{
	/// <summary>
	///
	/// </summary>
	public class MyNewLevelPopup : MyPopupBase
	{
		#region Properties

		public override string PopupKey { get { return MyPopupManager.CreateNewLevelPopupKey; } }

		#endregion

		#region Editor Variables

		[SerializeField]
		private InputField _levelNameInputElement;

		[SerializeField]
		private InputField _uniformSizeInputElement;

		#endregion

		#region Public Methods

		public void OnCreateClicked()
		{
			int uSize = 0;
			string levelName = string.Empty;

			if(int.TryParse(_uniformSizeInputElement.text, out uSize))
			{
				if(uSize <= 0)
				{
					Debug.LogWarning("Please enter a value above 0 for 'uniform size'.");
				}
			}
			else
			{
				Debug.LogWarning("Please enter a valid integer for 'uniform size'.");
			}

			if(_levelNameInputElement.text == string.Empty)
			{
				Debug.LogWarning("Please enter a valid string for 'level name'.");
			}
			else
			{
				levelName = _levelNameInputElement.text;
			}

			if(!string.IsNullOrEmpty(levelName) && uSize > 0)
			{
				CreateNewLevel(levelName, uSize);
			}
		}

		public override void Close()
		{
			_levelNameInputElement.text = string.Empty;
			_uniformSizeInputElement.text = string.Empty;
			base.Close();
		}

		#endregion

		#region Private Methods

		private void CreateNewLevel(string levelName, int uniformSize)
		{
			MyGameState.Instance.LevelManager.SwitchToNewLevel(levelName, uniformSize);
			Close();
		}

		#endregion
	}
}