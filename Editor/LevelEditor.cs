namespace LevelEditor
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEditor;
    using System;
    public class LevelEditor : EditorWindow
    {
        #region PRIVATE_VARS
        GUISkin skin;
        Rect mainRect;
        int numberOfToolBoxItems = 10;
        int totalLevels = 10;
        Vector2 toolBoxScrollPosition;
        Vector2 levelBoxScrollPosition;
        Vector2 gridContainerScrollPosition;
        Vector2Int gridSize = new Vector2Int(25, 25);
        LevelEditorConfiguration configuration;

        #endregion

        #region Editor_Callbacks
        [MenuItem("LevelEditor/Editor")]
        static void Initialize()
        {
            LevelEditor window = (LevelEditor)EditorWindow.GetWindow(typeof(LevelEditor));
            window.title = "LevelEditor";
            window.minSize = new Vector2(600, 300);
            window.Show();
        }
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
        public void OnGUI()
        {
            DrawEditor();
            HandleEvents();
        }
        #endregion

        #region DrawEditor
        public void DrawEditor()
        {
            mainRect = EditorGUILayout.BeginVertical(skin.GetStyle("mainpanel"), GUILayout.ExpandHeight(true), GUILayout.ExpandWidth(true));
            DrawToolBar();
            if (configuration)
            {
                DrawMainArea();
            }
            EditorGUILayout.EndVertical();
        }

        #endregion

        #region ToolBarDrawer
        public void DrawToolBar()
        {
            EditorGUILayout.BeginHorizontal(skin.GetStyle("subpanel"), GUILayout.ExpandWidth(true), GUILayout.Height(Screen.height * 0.1f));
            configuration = (LevelEditorConfiguration)EditorUIUtility.DrawObjectFieldWithLabel("Level Editor Configuration : ", configuration, typeof(LevelEditorConfiguration));
            EditorGUILayout.EndHorizontal();
        }
        #endregion

        #region MainAreaDrawer
        public void DrawMainArea()
        {
            EditorGUILayout.BeginHorizontal(skin.GetStyle("subpanel"), GUILayout.ExpandWidth(true));
            DrawToolBarContainer();
            DrawGridContainer();
            EditorGUILayout.EndHorizontal();
        }
        public void DrawToolBarContainer()
        {
            Rect toolbarRect = EditorGUILayout.BeginVertical(skin.GetStyle("mainpanel"), GUILayout.ExpandHeight(true), GUILayout.Width(EditorGUIUtility.currentViewWidth * 0.3f));
            DrawEditorToolBox(toolbarRect.height / 2.0f);
            DrawLevelEditorToolBox(toolbarRect.height / 2.0f);
            EditorGUILayout.EndVertical();
        }

        public void DrawGridContainer()
        {
            EditorGUILayout.BeginVertical(skin.GetStyle("mainpanel"), GUILayout.ExpandHeight(true), GUILayout.ExpandWidth(true));
            DrawGrid();
            EditorGUILayout.EndVertical();
        }
        #endregion

        #region DrawToolBarArea
        #region ToolBoxDrawer
        public void DrawEditorToolBox(float height)
        {
            EditorGUILayout.BeginVertical(skin.GetStyle("toolbox"), GUILayout.ExpandHeight(true), GUILayout.ExpandWidth(true));
            DrawToolBoxItems();
            EditorGUILayout.EndVertical();
        }
        public void DrawToolBoxItems()
        {
            toolBoxScrollPosition = EditorGUILayout.BeginScrollView(toolBoxScrollPosition, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
            //int rows = Mathf.CeilToInt((float)configuration.toolConfiguration.tools.Count / 2);
            for (int index = 0; index < configuration.toolConfiguration.tools.Count;)
            {
                DrawEditorToolBoxRows(ref index, Mathf.Min((index + 1), (configuration.toolConfiguration.tools.Count - 1)));
            }
            EditorGUILayout.EndScrollView();
        }
        public void DrawEditorToolBoxRows(ref int index, int count)
        {
            EditorGUILayout.BeginHorizontal(skin.GetStyle("mainpanel"), GUILayout.ExpandWidth(true), GUILayout.Height(50));

            for (; index <= count; index++)
            {
                DrawEditorToolBoxColumns(index);
            }
            EditorGUILayout.EndHorizontal();
        }
        public void DrawEditorToolBoxColumns(int index)
        {
            EditorGUILayout.BeginVertical(skin.GetStyle("toolboxItems"), GUILayout.ExpandHeight(true), GUILayout.ExpandWidth(true));
            EditorUIUtility.DrawButton(configuration.toolConfiguration.tools[index].gridSprite, () => Debug.Log("Clicked"), skin.GetStyle("button"));
            EditorGUILayout.EndVertical();
        }

        #endregion

        #region LevelButtonDrawer
        public void DrawLevelEditorToolBox(float height)
        {
            EditorGUILayout.BeginVertical(skin.GetStyle("toolbox"), GUILayout.ExpandHeight(true), GUILayout.ExpandWidth(true));
            levelBoxScrollPosition = EditorGUILayout.BeginScrollView(levelBoxScrollPosition, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
            DrawLevelButtons();
            EditorGUILayout.EndScrollView();
            EditorGUILayout.EndVertical();
        }
        public void DrawLevelButtons()
        {
            for (int index = 0; index < configuration.levelData.levels.Count; index++)
            {
                GUILayout.Button("Level " + index, GUILayout.ExpandHeight(true), GUILayout.ExpandWidth(true), GUILayout.Height(30));
            }
        }
        #endregion

        #endregion

        #region GridArea

        public void DrawGrid()
        {
            gridContainerScrollPosition = EditorGUILayout.BeginScrollView(gridContainerScrollPosition, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
            for (int row = 0; row < gridSize.x; row++)
            {
                EditorGUILayout.BeginHorizontal();
                for (int colomn = 0; colomn < gridSize.y; colomn++)
                {
                    EditorUIUtility.DrawButton(System.String.Empty, () => Debug.Log("row : " + row + " :Colomn : " + colomn), skin.GetStyle("toolbox"), GUILayout.Width(25), GUILayout.Height(25));
                }
                EditorGUILayout.EndHorizontal();
            }
            EditorGUILayout.EndScrollView();

        }
        #endregion

        #region HandleEditorEvent
        public void HandleEvents()
        {
            HandlePanMoveOfGrid();
        }
        public void HandlePanMoveOfGrid()
        {
            if (Event.current.type == EventType.MouseDrag &&
           (Event.current.button == 0 && Event.current.modifiers == EventModifiers.Alt) ||
           Event.current.button == 2)
            {
                Vector2 delta = -Event.current.delta;
                gridContainerScrollPosition += delta;

                Event.current.Use();
            }
        }
        #endregion
    }
}


