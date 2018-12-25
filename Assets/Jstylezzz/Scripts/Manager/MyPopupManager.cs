/*
* Copyright (c) Jari Senhorst. All rights reserved.  
* Website: www.jarisenhorst.com
* Licensed under the MIT License. See LICENSE file in the project root for full license information.  
* 
*/

using Jstylezzz.Popups;
using StyloCore;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Jstylezzz.Manager
{
	/// <summary>
	///
	/// </summary>
	public class MyPopupManager : MySingleton<MyPopupManager>
	{
		#region Consts

		public const string GenericPopupKey = "GenericPopup";
		public const string CreateNewLevelPopupKey = "CreateNewLevel";
		public const string LevelEditorLevelSelectPopupKey = "LevelEditorLevelSelect";

		#endregion

		#region Variables

		private Dictionary<string, MyPopupBase> _popupDictionary = new Dictionary<string, MyPopupBase>();
		private Queue<MyPopupBase> _popupQueue = new Queue<MyPopupBase>();
		private Queue<MyGenericPopupData> _genericPopupDataQueue = new Queue<MyGenericPopupData>();
		private MyPopupBase _activePopup = null;
		private MyGenericPopup _genericPopupReference;

		#endregion

		#region Public Methods

		public void RegisterPopup(MyPopupBase popup)
		{
			if(_popupDictionary.ContainsKey(popup.PopupKey))
				Debug.LogWarning($"Attempted to register duplicate popup to manager ({popup.PopupKey}).");
			else
			{
				_popupDictionary.Add(popup.PopupKey, popup);
				if(popup.IsGeneric)
					_genericPopupReference = (MyGenericPopup)popup;
			}
		}

		public void RequestPopup(string key)
		{
			if(_popupDictionary.ContainsKey(key) == false)
				Debug.LogWarning($"No such popup registered ({key})!");
			else
			{
				_popupQueue.Enqueue(_popupDictionary[key]);
				PopupQueueUpdated();
			}
		}

		public void RequestGenericPopup(string title, string message, string leftButton, Action leftAction, string rightButton = null, Action rightAction = null)
		{
			_genericPopupDataQueue.Enqueue(new MyGenericPopupData(title, message, leftButton, rightButton, leftAction, rightAction));
			_popupQueue.Enqueue(_popupDictionary[GenericPopupKey]);
			PopupQueueUpdated();
		}

		public void CloseActiveGeneric()
		{
			_genericPopupReference.Close();
		}

		public void CloseActive()
		{
			_activePopup.Close();
		}

		#endregion

		#region Private Methods

		private void OnActivePopupClosed()
		{
			PopupQueueUpdated();
		}

		private void PerformOpenRequest(MyPopupBase popup)
		{
			if(_activePopup != null && _activePopup.IsOpen == true)
				return;
			else
			{
				if(_activePopup != null)
				{
					_activePopup.Closed -= OnActivePopupClosed;
				}

				_activePopup = popup;
				_activePopup.Closed += OnActivePopupClosed;
				popup.Open();
			}
		}

		private void PopupQueueUpdated()
		{
			if(_activePopup != null)
			{
				_activePopup.Closed -= OnActivePopupClosed;
				_activePopup = null;
			}

			if(_activePopup == null && _popupQueue.Count > 0)
			{
				MyPopupBase popup = _popupQueue.Dequeue();
				if(popup.IsGeneric)
				{
					if(_genericPopupDataQueue.Count == 0)
					{
						Debug.LogWarning("Popup queue encountered GenericPopup without GenericPopupData. Skipping..");
						PopupQueueUpdated();
						return;
					}

					_genericPopupReference.Prepare(_genericPopupDataQueue.Dequeue());
				}

				PerformOpenRequest(popup);
			}
		}

		#endregion
	}
}