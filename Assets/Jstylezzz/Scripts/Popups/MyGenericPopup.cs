/*
* Copyright (c) Jari Senhorst. All rights reserved.  
* Website: www.jarisenhorst.com
* Licensed under the MIT License. See LICENSE file in the project root for full license information.  
* 
*/
using Jstylezzz.Manager;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Jstylezzz.Popups
{
	/// <summary>
	///
	/// </summary>
	public class MyGenericPopup : MyPopupBase
	{
		public override string PopupKey { get { return MyPopupManager.GenericPopupKey; } }

		[SerializeField]
		private Text _title;

		[SerializeField]
		private Button _buttonLeft;

		[SerializeField]
		private Button _buttonRight;

		[SerializeField]
		private Text _buttonLeftText;

		[SerializeField]
		private Text _buttonRightText;

		[SerializeField]
		private Text _textField;

		private Action _buttonLeftAction;
		private Action _buttonRightAction;

		protected override void Awake()
		{
			IsGeneric = true;
			base.Awake();
		}

		public void Prepare(MyGenericPopupData data)
		{
			_title.text = data.Title;
			_buttonLeftText.text = data.LeftButtonText;
			_buttonLeftAction = data.LeftButtonAction;

			if(data.RightButtonAction == null)
			{
				_buttonRightAction = null;
				_buttonRight.gameObject.SetActive(false);
			}
			else
			{
				_buttonRightAction = data.RightButtonAction;
				_buttonRightText.text = data.RightButtonText;
				_buttonRight.gameObject.SetActive(true);
			}

			_textField.text = data.Content;
		}

		public override void Close()
		{
			base.Close();
			_buttonLeftAction = null;
			_buttonRightAction = null;
		}

		public void ButtonLeftClicked()
		{
			_buttonLeftAction?.Invoke();
		}

		public void ButtonRightClicked()
		{
			if(_buttonRightAction != null)
				_buttonRightAction?.Invoke();
		}

		public void CloseClicked()
		{
			Close();
		}
	}

	public class MyGenericPopupData
	{
		public string Title { get; private set; }
		public string Content{ get; private set; }
		public string LeftButtonText { get; private set; }
		public string RightButtonText { get; private set; }
		public Action LeftButtonAction { get; private set; }
		public Action RightButtonAction { get; private set; }

		public MyGenericPopupData(string title, string content, string leftbutton, string rightbutton, Action leftbuttonAction, Action rightbuttonAction)
		{
			Title = title;
			Content = content;
			LeftButtonText = leftbutton;
			RightButtonText = rightbutton;
			LeftButtonAction = leftbuttonAction;
			RightButtonAction = rightbuttonAction;
		}
	}
}