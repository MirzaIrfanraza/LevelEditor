namespace LevelEditor
{
    using System.Collections.Generic;
    using System.Collections;
    using UnityEngine;
    using UnityEditor;
    using System;
    using System.Linq;

    public class LevelToolPrefabMapperEditor : EditorWindow
    {
        #region PRIVATE_VARS
        GUISkin skin;
        LevelEditorConfiguration configuration;
        string name;
        Vector2 toolContainerPosition;
        #endregion

        #region PUBLIC_VARS
        #endregion

        #region LEVEL_CREATOR_EDITOR
        public static void Initialize()
        {
            LevelToolPrefabMapperEditor window = (LevelToolPrefabMapperEditor)EditorWindow.GetWindow(typeof(LevelToolPrefabMapperEditor));
            window.title = "LevelToolPrefabMapperEditor";
            window.minSize = new Vector2(300, 400);
            window.maxSize = new Vector2(300, 400);
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
            int toolSize = configuration.toolConfiguration.tools.Count;
            int prefabSize = configuration.toolPrefabMapper.prefabs.Count;
            if (toolSize < prefabSize)
            {
                configuration.toolPrefabMapper.prefabs.RemoveRange(toolSize - 1, prefabSize - toolSize);
            }
            else if (toolSize > prefabSize)
            {
                List<GameObject> tempList = new List<GameObject>(toolSize - prefabSize);
                for(int indexOfGameObject=0;indexOfGameObject<toolSize-prefabSize;indexOfGameObject++)
                {
                    tempList.Add(null);
                }
                configuration.toolPrefabMapper.prefabs.AddRange(tempList);
                Debug.Log("Inside Else");
            }
            Debug.Log("Called : "+ configuration.toolPrefabMapper.prefabs.Count);
        }
     
        public void OnGUI()
        {
            if (!configuration)
                return;

            EditorGUILayout.BeginVertical();
            toolContainerPosition = EditorGUILayout.BeginScrollView(toolContainerPosition, GUILayout.ExpandWidth(LevelEditorConstants.BoolTrue), GUILayout.ExpandHeight(LevelEditorConstants.BoolTrue));
            DrawMapper();
            EditorGUILayout.EndScrollView();
            EditorUIUtility.DrawButton(LevelEditorConstants.Close, () => Save());
            EditorGUILayout.EndVertical();
        }

        #endregion
        #region PUBLIC_METHODS
        public void DrawMapper()
        {
            for (int indexOfTool = 0; indexOfTool < configuration.toolConfiguration.tools.Count; indexOfTool++)
            {
                EditorGUILayout.BeginHorizontal();
                GUILayout.Button(configuration.toolConfiguration.tools[indexOfTool].gridSprite, GUILayout.Height(LevelEditorConstants.i35), GUILayout.Width(LevelEditorConstants.i35));
                configuration.toolPrefabMapper.prefabs[indexOfTool]=(GameObject)EditorUIUtility.DrawObjectField(configuration.toolPrefabMapper.prefabs[indexOfTool], typeof(GameObject),GUILayout.Height(LevelEditorConstants.i35));
                EditorGUILayout.EndHorizontal();
            }
        }
        public void Save()
        {
            this.Close();
        }
        #endregion

        #region PRIVATE_METHODS

        #endregion
    }
}