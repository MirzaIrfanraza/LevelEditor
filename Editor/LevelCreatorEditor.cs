namespace LevelEditor {
	using System.Collections.Generic;
	using System.Collections;
	using UnityEngine;
	using UnityEditor;

	public class LevelCreatorEditor : EditorWindow {
		#region PRIVATE_VARS
        GUISkin skin;
		LevelEditorConfiguration configuration;
		string name;
		Vector2Int gridSize;
		#endregion

		#region PUBLIC_VARS
		#endregion

		#region LEVEL_CREATOR_EDITOR
		public static void Initialize() 
		{
			LevelCreatorEditor window=(LevelCreatorEditor)EditorWindow.GetWindow(typeof(LevelCreatorEditor));
			window.title="LevelCreatorEditor";
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
			gridSize=EditorUIUtility.DrawVector2FieldWithName("Grid Size : ",gridSize);
			name=EditorUIUtility.DrawTextFieldWithName("Level Name : ",name);
			EditorUIUtility.DrawButton("Create",()=>InitializeGrid());
		}
		#endregion
		#region PUBLIC_METHODS
		
		#endregion

		#region PRIVATE_METHODS
		void InitializeGrid()
        {
			configuration.levelData.currentLevel = new Level(gridSize, new List<CellView>(),name);
            for (int i = 0; i < configuration.levelData.currentLevel.gridSize.x * configuration.levelData.currentLevel.gridSize.y; i++)
            {
                configuration.levelData.currentLevel.cellViews.Add(new CellView(new Cell(-1)));
            }
			this.Close();
        }
		#endregion
	}
}