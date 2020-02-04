namespace LevelEditor
{
    using System.Collections.Generic;
    using System.Collections;
    using UnityEngine;
    using UnityEditor;

    [CreateAssetMenu(menuName = "LevelEditor/LevelData")]
    
    public class LevelData : ScriptableObject
    {
        public List<Level> Levels;
        public Level currentLevel;
        public int currentLevelIndex=-1;
    }
}