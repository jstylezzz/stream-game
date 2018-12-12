/*
* Copyright (c) Jari Senhorst. All rights reserved.  
* Website: www.jarisenhorst.com
* Licensed under the MIT License. See LICENSE file in the project root for full license information.  
* 
*/

using Jstylezzz.Cam;
using Jstylezzz.Grid;
using Jstylezzz.LevelEditor;
using StyloCore;
using UnityEngine;

namespace Jstylezzz.Manager
{
	/// <summary>
	///
	/// </summary>
	public class MyGameState : MySingleton<MyGameState>
	{
		#region Manager Instances

		public MyCameraOperator CameraOperator { get; private set; }
		public MyLevelEditorManager LevelEditorManager { get; private set; }

		#endregion

		#region World Instances

		public MyGrid ActiveGrid { get; private set; }

		#endregion

		#region Register Methods

		public void RegisterCameraOperator(MyCameraOperator oprt)
		{
			if(CameraOperator != null)
			{
				Debug.LogWarning("[MyGameState]: Camera operator already registered! Be sure to unregister before registering one again.");
				return;
			}
			CameraOperator = oprt;
		}

		public void RegisterActiveGrid(MyGrid g)
		{
			if(ActiveGrid != null)
			{
				Debug.LogWarning("[MyGameState]: Active grid already registered! Be sure to unregister before registering one again.");
				return;
			}
			ActiveGrid = g;
		}

		public void RegisterLevelManager(MyLevelEditorManager levelEditorManager)
		{
			if(LevelEditorManager != null)
			{
				Debug.LogWarning("[MyGameState]: Level editor manager already registered! Be sure to unregister before registering one again.");
				return;
			}
			LevelEditorManager = levelEditorManager;
		}

		#endregion

		#region Unregister Methods

		public void UnregisterCameraOperator()
		{
			CameraOperator = null;
		}

		public void UnregisterActiveGrid()
		{
			ActiveGrid = null;
		}

		public void UnregisterLevelEditorManager()
		{
			LevelEditorManager = null;
		}

		#endregion
	}
}