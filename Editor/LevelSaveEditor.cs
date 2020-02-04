namespace LevelEditor {
	using System.Collections.Generic;
	using System.Collections;
	using UnityEngine;
	using UnityEditor;

	public class LevelSaveEditor : EditorWindow {
		#region PRIVATE_VARS
        GUISkin skin;
		LevelEditorConfiguration configuration;
		string name;
		#endregion

		#region PUBLIC_VARS
		#endregion

		#region LEVEL_CREATOR_EDITOR
		public static void Initialize() 
		{
			LevelSaveEditor window =(LevelSaveEditor)EditorWindow.GetWindow(typeof(LevelSaveEditor));
			window.title="LevelSaveEditor";
			window.minSize=new Vector2(400,100);
			window.maxSize=new Vector2(400,100);
			window.Show();
		}
		#endregion
		#region UNITY_CALLBACKS
		private void OnEnable() 
		{
			skin = Resources.Load<GUISkin>("Skins/Theme1");
            if (skin == null)
            {
                Debug.Log("Skin not found");
            }
            else
            {
                Debug.Log("Skin found");
            }
		}
		public void InitializeLevelEditorConfiguration(LevelEditorConfiguration configuration)
		{
			this.configuration = configuration; 
		}
		public void OnGUI()
        {
			name=EditorUIUtility.DrawTextFieldWithName("Level Name : ",name);
			EditorUIUtility.DrawButton("Save",()=>OnSaveButtonClick());
		}
		#endregion
		#region PUBLIC_METHODS
		
		#endregion

		#region PRIVATE_METHODS
		void OnSaveButtonClick()
        {
			Level tempLevel;
			configuration.levelData.currentLevel.name = name;
			Vector2Int tempGridSize = configuration.levelData.currentLevel.gridSize;
			List<CellView> tempCellView = new List<CellView>();
			foreach (CellView cellView in configuration.levelData.currentLevel.cellViews)
			{
				tempCellView.Add(new CellView(cellView.cell, cellView.texture));
			}
			tempLevel = new Level(tempGridSize, tempCellView, configuration.levelData.currentLevel.name);
            configuration.levelData.Levels.Add(tempLevel);
            this.Close();
        }
		#endregion
	}
}