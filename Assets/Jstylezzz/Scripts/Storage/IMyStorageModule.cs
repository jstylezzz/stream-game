/*
* Copyright (c) Jari Senhorst. All rights reserved.  
* Website: www.jarisenhorst.com
* Licensed under the MIT License. See LICENSE file in the project root for full license information.  
* 
*/

namespace Jstylezzz.Storage
{
	/// <summary>
	/// Interface describing a StorageModule's setup.
	/// </summary>
	public interface IMyStorageModule
	{
		/// <summary>
		/// The module's localized name.
		/// </summary>
		string ModuleName { get; }

		/// <summary>
		/// True if the module is loaded.
		/// </summary>
		bool IsLoaded { get; set; }

		/// <summary>
		/// The relative path (from StorageRoot) to this module's folder.
		/// </summary>
		string RelativePath { get; }

		/// <summary>
		/// The relative path (from StorageRoot) to this module's file.
		/// </summary>
		string RelativeFilePath { get; }

		/// <summary>
		/// Get the JSON for stored data of this module.
		/// </summary>
		/// <returns>JSON string for stored data of this module.</returns>
		string GetJSON();

		/// <summary>
		/// Set up storable variables from JSON.
		/// </summary>
		/// <param name="json">The JSON to set the storable variables up with.</param>
		void InitFromJSON(string json);

		/// <summary>
		/// Call this function when module data is written to the disk for the first time.
		/// This should only occur when starting a new game or when booting up the game for the first time.
		/// </summary>
		void OnModuleDataGeneratedFirstTime();
	}
}