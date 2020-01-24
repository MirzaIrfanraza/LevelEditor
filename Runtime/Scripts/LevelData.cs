namespace LevelEditor {
	using System.Collections.Generic;
	using System.Collections;
	using UnityEngine;
    [CreateAssetMenu(menuName ="LevelEditor/LevelData")]
	public class LevelData : ScriptableObject 
	{
		public List<Level> levels;
	}
}