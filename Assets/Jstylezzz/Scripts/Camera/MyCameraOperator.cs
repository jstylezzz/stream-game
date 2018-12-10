/*
* Copyright (c) Jari Senhorst. All rights reserved.  
* Website: www.jarisenhorst.com
* Licensed under the MIT License. See LICENSE file in the project root for full license information.  
* 
*/

using Jstylezzz.Manager;
using UnityEngine;

namespace Jstylezzz.Cam
{
	/// <summary>
	///
	/// </summary>
	public class MyCameraOperator : MonoBehaviour
	{
		public Camera MainCamera
		{
			get
			{
				return _mainCamera;
			}
		}

		[SerializeField]
		private Camera _mainCamera;

		private void Awake()
		{
			MyGameState.Instance.RegisterCameraOperator(this);
		}
	}
}