/*
* Copyright (c) Jari Senhorst. All rights reserved.  
* Website: www.jarisenhorst.com
* Licensed under the MIT License. See LICENSE file in the project root for full license information.  
* 
*/

using Jstylezzz.Cam;
using StyloCore;
using UnityEngine;

namespace Jstylezzz.Manager
{
	/// <summary>
	///
	/// </summary>
	public class MyGameState : MySingleton<MyGameState>
	{
		public MyCameraOperator CameraOperator { get; private set; }

		public void RegisterCameraOperator(MyCameraOperator oprt)
		{
			if(CameraOperator != null)
			{
				Debug.LogWarning("[MyGameState]: Camera operator already registered! Be sure to unregister before registering one again.");
				return;
			}
			CameraOperator = oprt;
		}

		public void UnregisterCameraOperator()
		{
			CameraOperator = null;
		}
	}
}