namespace LevelEditor 
{
	using System.Collections.Generic;
	using System.Collections;
	using UnityEngine;
    [CreateAssetMenu(menuName ="LevelEditor/ToolConfiguration")]
	public class ToolConfiguration : ScriptableObject {
		public List<LevelEditorTool> tools;
	}
}