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

        public LevelEditorConfiguration configuration;
        LevelEditorTool currentSelectedTool;
        int currentSelectedToolIndex = -1;
        // Level level;
        int currentSelectedLevel = -1;
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
            configuration.levelData.currentLevel = new Level(new Vector2Int(5, 5), new List<CellView>(),"Untitled");
            for (int i = 0; i < configuration.levelData.currentLevel.gridSize.x * configuration.levelData.currentLevel.gridSize.y; i++)
            {
                configuration.levelData.currentLevel.cellViews.Add(new CellView(new Cell(-1)));
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
            Repaint();
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
            EditorGUILayout.BeginHorizontal(skin.GetStyle("subpanel"), GUILayout.ExpandWidth(true), GUILayout.Height(Screen.height * 0.035f));
            EditorGUILayout.BeginVertical(GUILayout.ExpandWidth(false));
            GUILayout.Space(5);
            EditorUIUtility.DrawLabel("Editor Configuration : ", skin.GetStyle("label"), GUILayout.ExpandWidth(false) );
            EditorGUILayout.EndVertical();
            GUILayout.Space(5);
            configuration = (LevelEditorConfiguration)EditorUIUtility.DrawObjectField(configuration, typeof(LevelEditorConfiguration),GUILayout.ExpandHeight(true));
            EditorUIUtility.DrawButton("New ", () => OnCreateNewLevelButtonClick(),GUILayout.ExpandHeight(true));
            EditorUIUtility.DrawButton("Save", () => OnSaveButtonClick(), GUILayout.ExpandHeight(true));
            EditorUIUtility.DrawButton("Save As", () => OnNewLevelButtonClick(), GUILayout.ExpandHeight(true));

            EditorGUILayout.EndHorizontal();
        }
        public void OnNewLevelButtonClick()
        {
            LevelCreatorEditor.Initialize();
            LevelCreatorEditor creatorEditor=(LevelCreatorEditor)EditorWindow.GetWindow(typeof(LevelCreatorEditor));
            creatorEditor.InitializeLevelEditorConfiguration(configuration);
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
            DrawEditorToolBox(500 / 2.0f);
            DrawLevelEditorToolBox(500 / 2.0f);
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
            Vector2Int tempGridSize = configuration.levelData.currentLevel.gridSize;
            List<CellView> tempCellView = new List<CellView>();

            foreach (CellView cellView in configuration.levelData.currentLevel.cellViews)
            {
                tempCellView.Add(new CellView(cellView.cell,cellView.texture));
            }
            tempLevel = new Level(tempGridSize, tempCellView,configuration.levelData.currentLevel.name);
            configuration.levelData.Levels.Add(tempLevel);

            Debug.Log(tempCellView.Equals(configuration.levelData.currentLevel.cellViews));
            Debug.Log(tempLevel.Equals(configuration.levelData.currentLevel));
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
            EditorGUILayout.BeginVertical(skin.GetStyle("toolbox"), GUILayout.Height(height), GUILayout.ExpandWidth(true));
            DrawToolBoxItems();
            EditorGUILayout.EndVertical();
        }
        public void DrawToolBoxItems()
        {
            EditorGUILayout.BeginVertical(skin.GetStyle("mainpanel"));
            toolBoxScrollPosition = EditorGUILayout.BeginScrollView(toolBoxScrollPosition, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
            for (int index = 0; index < configuration.toolConfiguration.tools.Count;)
            {
                DrawEditorToolBoxRows(ref index, Mathf.Min((index + 3), (configuration.toolConfiguration.tools.Count - 1)));
            }
            EditorGUILayout.EndScrollView();
            EditorGUILayout.EndVertical();

        }
        public void DrawEditorToolBoxRows(ref int index, int count)
        {
            EditorGUILayout.BeginHorizontal(skin.GetStyle("toolbox"), GUILayout.ExpandWidth(true), GUILayout.Height(50));
            GUILayout.FlexibleSpace();
            for (; index <= count; index++)
            {
                GUILayout.FlexibleSpace();
                EditorGUILayout.BeginVertical();
                GUILayout.Space(50 / 2 - 35 / 2);
                int tempIndex = index;
                EditorUIUtility.DrawButton(configuration.toolConfiguration.tools[index].gridSprite, () => SelectTool(tempIndex), GUILayout.Height(35), GUILayout.Width(35));
                EditorGUILayout.EndVertical();
                GUILayout.FlexibleSpace();
            }
            GUILayout.FlexibleSpace();

            EditorGUILayout.EndHorizontal();
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
            EditorGUILayout.BeginVertical(skin.GetStyle("toolbox"), GUILayout.Height(height), GUILayout.ExpandWidth(true));
            levelBoxScrollPosition = EditorGUILayout.BeginScrollView(levelBoxScrollPosition, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
            DrawLevelButtons();
            EditorGUILayout.EndScrollView();
            EditorGUILayout.EndVertical();
        }
        public void DrawLevelButtons()
        {
            // Debug.Log(configuration.levelData.Levels.Count);
            for (int index = 0; index < configuration.levelData.Levels.Count; index++)
            {
                EditorGUILayout.BeginHorizontal(skin.GetStyle("toolbox"));
                EditorUIUtility.DrawButton(configuration.levelData.Levels[index].name, () => OnLevelButtonClicked(index), GUILayout.Height(45), GUILayout.ExpandWidth(true));
                EditorUIUtility.DrawButton("X", () => OnLevelDeleteButtonClicked(index), GUILayout.Height(45), GUILayout.ExpandWidth(true));
                EditorUIUtility.DrawButton(skin.GetStyle("editbutton").normal.background, () => OnLevelEditButtonClicked(index), GUILayout.Height(45), GUILayout.Width(45));
                EditorGUILayout.EndHorizontal();
            }
        }
        public void OnLevelDeleteButtonClicked(int index)
        {
            configuration.levelData.Levels.RemoveAt(index);
            Repaint();
        }
        public void OnLevelEditButtonClicked(int index)
        {
            currentSelectedLevel = index;
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
            tempLevel = new Level(tempGridSize, tempCellView,configuration.levelData.Levels[index].name);
            configuration.levelData.currentLevel = tempLevel;
            Repaint();
        }
        #endregion

        #endregion

        #region GridArea

        public void DrawGrid()
        {
            gridContainerScrollPosition = EditorGUILayout.BeginScrollView(gridContainerScrollPosition, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
            int cellIndex = 0;
            if (configuration.levelData.currentLevel.cellViews.Count > 0)
            {
                for (int row = 0; row < configuration.levelData.currentLevel.gridSize.x; row++)
                {
                    EditorGUILayout.BeginHorizontal();
                    for (int colomn = 0; colomn < configuration.levelData.currentLevel.gridSize.y; colomn++)
                    {
                        configuration.levelData.currentLevel.cellViews[cellIndex].DrawCell(currentSelectedTool, currentSelectedToolIndex, this);
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
            if ( (Event.current.button == 0 && Event.current.modifiers == EventModifiers.Alt) || Event.current.button == 2)
            {
                Vector2 delta = -Event.current.delta;
                gridContainerScrollPosition += delta;
            }
        }
        public void HandleCellClicked()
        {

        }
        #endregion
    }
}