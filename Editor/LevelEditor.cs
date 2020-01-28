namespace LevelEditor
{
    using System.Collections.Generic;
    using System.Collections;
    using System;
    using UnityEditor;
    using UnityEngine;
    public class LevelEditor : EditorWindow
    {
        #region PRIVATE_VARS
        GUISkin skin;
        Rect mainRect;
        Vector2 toolBoxScrollPosition;
        Vector2 levelBoxScrollPosition;
        Vector2 gridContainerScrollPosition;

        LevelEditorConfiguration configuration;
        LevelEditorTool currentSelectedTool;
        int currentSelectedToolIndex = -1;
        Level level;

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
        void InitializeGrid()
        {
            level = new Level(new Vector2Int(10,10),new List<CellView>());
            for (int i = 0; i < level.gridSize.x * level.gridSize.y; i++)
            {
                level.cellViews.Add(new CellView(new Cell(-1), skin.GetStyle("button").normal.background));
            }
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
            InitializeGrid();
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
            EditorGUILayout.BeginVertical(skin.GetStyle("subpanel"), GUILayout.ExpandWidth(true), GUILayout.Height(Screen.height * 0.1f));
            // EditorGUILayout.BeginHorizontal(skin.GetStyle("subpanel"), GUILayout.ExpandWidth(true), GUILayout.Height(Screen.height * 0.1f));
            configuration = (LevelEditorConfiguration)EditorUIUtility.DrawObjectFieldWithLabel("Level Editor Configuration : ", configuration, typeof(LevelEditorConfiguration));
            EditorGUILayout.BeginHorizontal();
            EditorUIUtility.DrawButton("New ", () => OnCreateNewLevelButtonClick());
            EditorUIUtility.DrawButton("Save", () => OnSaveButtonClick());
            EditorGUILayout.EndHorizontal();
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
        public void OnSaveButtonClick()
        {
            Level tempLevel;

            Vector2Int tempGridSize = level.gridSize;
            List<CellView> tempCellView = new List<CellView>();
            foreach(CellView cellView in level.cellViews)
            {
                tempCellView.Add(new CellView(cellView.cell,cellView.texture));
            }

            tempLevel = new Level(tempGridSize,tempCellView);
            configuration.levelData.Levels.Add(tempLevel);

            Debug.Log(tempCellView.Equals(level.cellViews));
            Debug.Log(tempLevel.Equals(level));
            //Repaint();
            //InitializeGrid();
        }
        public void OnCreateNewLevelButtonClick()
        {
            InitializeGrid();
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
            EditorUIUtility.DrawButton(configuration.toolConfiguration.tools[index].gridSprite, () => SelectTool(index));
            EditorGUILayout.EndVertical();
        }
        public void SelectTool(int index)
        {
            currentSelectedTool = configuration.toolConfiguration.tools[index];
            currentSelectedToolIndex = index;
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
            for (int index = 0; index < configuration.levelData.Levels.Count; index++)
            {
                EditorGUILayout.BeginHorizontal(skin.GetStyle("toolbox"));
                EditorUIUtility.DrawButton("Level : " + index, () => OnLevelButtonClicked(index), GUILayout.Height(30), GUILayout.ExpandWidth(true));
                EditorUIUtility.DrawButton("X", () => OnLevelDeleteButtonClicked(index), GUILayout.Height(30), GUILayout.ExpandWidth(true));
                EditorGUILayout.EndHorizontal();
            }
        }
        public void OnLevelDeleteButtonClicked(int index)
        {
            configuration.levelData.Levels.RemoveAt(index);
            Repaint();
        }

        public void OnLevelButtonClicked(int index)
        {
            Level tempLevel;
            Vector2Int tempGridSize = configuration.levelData.Levels[index].gridSize;
            List<CellView> tempCellView = new List<CellView>();
            foreach (CellView cellView in configuration.levelData.Levels[index].cellViews)
            {
                tempCellView.Add(new CellView(cellView.cell,cellView.texture));
            }
            tempLevel = new Level(tempGridSize, tempCellView);
            level = tempLevel;
            Repaint();
        }
        #endregion

        #endregion

        #region GridArea

        public void DrawGrid()
        {
            gridContainerScrollPosition = EditorGUILayout.BeginScrollView(gridContainerScrollPosition, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
            int cellIndex = 0;
            if (level.cellViews.Count > 0)
            {
                for (int row = 0; row < level.gridSize.x; row++)
                {
                    EditorGUILayout.BeginHorizontal();
                    for (int colomn = 0; colomn < level.gridSize.y; colomn++)
                    {
                        level.cellViews[cellIndex].DrawCell(currentSelectedTool, currentSelectedToolIndex, this);
                        cellIndex++;
                    }
                    EditorGUILayout.EndHorizontal();
                }
            }
            EditorGUILayout.EndScrollView();
        }
        #endregion

        #region HandleEditorEvent
        public void HandleEvents()
        {
            HandlePanMoveOfGrid();
            HandleCellClicked();
        }
        public void HandlePanMoveOfGrid()
        {
            if (Event.current.type == EventType.MouseDrag && (Event.current.button == 0 && Event.current.modifiers == EventModifiers.Alt) || Event.current.button == 2)
            {
                Vector2 delta = -Event.current.delta;
                gridContainerScrollPosition += delta;
                Event.current.Use();
            }
        }
        public void HandleCellClicked()
        {

        }
        #endregion
    }
}