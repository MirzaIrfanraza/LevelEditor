namespace LevelEditor {
	using System.Collections.Generic;
	using System.Collections;
	using UnityEngine;
    [CreateAssetMenu(menuName ="LevelEditor/LevelEditorConfiguration")]
	public class LevelEditorConfiguration : ScriptableObject 
	{
		public ToolConfiguration toolConfiguration;
		public LevelData levelData;
	}
}