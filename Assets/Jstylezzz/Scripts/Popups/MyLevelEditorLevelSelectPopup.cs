/*
* Copyright (c) Jari Senhorst. All rights reserved.  
* Website: www.jarisenhorst.com
* Licensed under the MIT License. See LICENSE file in the project root for full license information.  
* 
*/

using Jstylezzz.Manager;
using Jstylezzz.StorageModules;
using StyloCore.Util;
using UnityEngine;

namespace Jstylezzz.Popups
{
	/// <summary>
	///
	/// </summary>
	public class MyLevelEditorLevelSelectPopup : MyPopupBase
	{
		#region Properties

		public override string PopupKey
		{
			get
			{
				return MyPopupManager.LevelEditorLevelSelectPopupKey;
			}
		}

		#endregion

		#region Editor Variables

		[SerializeField]
		private GameObject _levelElementPrefab;

		[SerializeField]
		private GameObject _noLevelsElement;

		[SerializeField]
		private Transform _contentTransform;

		#endregion

		#region Variables

		private GameObject[] _selectElements;

		#endregion

		#region Lifecycle

		public override void Open()
		{
			string[] levels = MyLevelStorageModule.GetPotentialLevels();
			if(levels.Length > 0)
			{
				_noLevelsElement.SetActive(false);
				_selectElements = new GameObject[levels.Length];
				for(int i = 0; i < levels.Length; i++)
				{
					GameObject element = Instantiate(_levelElementPrefab);
					element.GetComponent<MyLevelSelectionElement>().AssignData(levels[i], MyTimeUtil.CurrentTimeStamp());
					_selectElements[i] = element;
					element.transform.SetParent(_contentTransform, false);
				}
			}
			else
			{
				_noLevelsElement.SetActive(true);
			}

			base.Open();
		}

		public override void Close()
		{
			if(_selectElements != null)
			{
				for(int i = 0; i < _selectElements.Length; i++)
				{
					Destroy(_selectElements[i]);
				}

				_selectElements = null;
			}

			base.Close();
		}

		#endregion
	}
}