/*
* Copyright (c) Jari Senhorst. All rights reserved.  
* Website: www.jarisenhorst.com
* Licensed under the MIT License. See LICENSE file in the project root for full license information.  
* 
*/

using DG.Tweening;
using Jstylezzz.Manager;
using UnityEngine;

namespace Jstylezzz.Popups
{
	/// <summary>
	///
	/// </summary>
	public abstract class MyPopupBase : MonoBehaviour
	{
		#region Consts

		private const float PopupTweenTime = 0.4f;
		private const float PopupClosedScale = 0.01f;
		private const float PopupOpenScale = 1f;

		#endregion

		#region Properties

		public abstract string PopupKey { get; }
		public bool IsOpen { get; private set; }
		public bool IsGeneric { get; protected set; } = false;

		#endregion

		#region Event Delegates

		public delegate void PopupOpenedEventHandler();
		public delegate void PopupClosedEventHandler();

		#endregion

		#region Events

		public event PopupOpenedEventHandler Opened;
		public event PopupClosedEventHandler Closed;

		#endregion

		#region Editor Variables

		[SerializeField]
		private GameObject m_popupViewObject;

		#endregion

		#region Variables

		private bool m_firstOpen = true;

		#endregion

		#region Lifecycle

		protected virtual void Awake()
		{
			m_popupViewObject.SetActive(false);
			MyPopupManager.Instance.RegisterPopup(this);
		}

		#endregion

		#region Public Methods

		public virtual void Open()
		{
			if(m_firstOpen == true)
			{
				m_firstOpen = false;
				m_popupViewObject.transform.localScale = Vector3.one * PopupClosedScale;
			}

			IsOpen = true;
			Opened?.Invoke();
			m_popupViewObject.SetActive(true);
			m_popupViewObject.transform.DOScale(PopupOpenScale, PopupTweenTime).SetEase(Ease.OutBack);
		}

		public virtual void Close()
		{
			m_popupViewObject.transform.DOScale(PopupClosedScale, PopupTweenTime).SetEase(Ease.InBack).OnComplete(() =>
			{
				m_popupViewObject.SetActive(false);
				IsOpen = false;
				Closed?.Invoke();
			});
		}

		#endregion
	}
}