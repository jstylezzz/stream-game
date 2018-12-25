/*
* Copyright (c) Jari Senhorst. All rights reserved.  
* Website: www.jarisenhorst.com
* Licensed under the MIT License. See LICENSE file in the project root for full license information.  
* 
*/

using Jstylezzz.Manager;
using StyloCore.Util;
using UnityEngine;
using UnityEngine.UI;

namespace Jstylezzz.Popups
{
	/// <summary>
	///
	/// </summary>
	public class MyLevelSelectionElement : MonoBehaviour
	{
		#region Editor Variables

		[SerializeField]
		private Text _levelNameElement;

		[SerializeField]
		private Text _levelSavedElement;

		#endregion

		#region Variables

		private string _levelName;

		#endregion

		#region Public Methods

		public void AssignData(string levelName, long saveTimeStamp)
		{
			_levelName = levelName;
			_levelNameElement.text = levelName;
			_levelSavedElement.text = MyTimeUtil.UnixTimeStampToDateTime(saveTimeStamp).ToString("dd-MM-yy");
		}

		public void OnSelected()
		{
			MyPopupManager.Instance.RequestGenericPopup("Load Level?", $"Are you sure you want to edit level '{_levelName}'?", 
			"Yes", () =>
			{
				MyPopupManager.Instance.CloseActiveGeneric();
				MyPopupManager.Instance.CloseActive();
				MyGameState.Instance.LevelManager.SwitchLevel(_levelName);
			}, 
			"No", () =>
			{
				MyPopupManager.Instance.CloseActiveGeneric();
			});
		}

		#endregion
	}
}