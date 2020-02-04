namespace LevelEditor
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    [CreateAssetMenu(menuName = "LevelEditor/ToolPrefabMapper")]
    public class ToolPrefabMapper : ScriptableObject
    {
        public List<GameObject> prefabs;
    }
}

