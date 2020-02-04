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
        int currentSelectedToolIndex = LevelEditorConstants.iM1;
        #endregion

        #region Editor_Callbacks
        [MenuItem("LevelEditor/Editor")]
        static void Initialize()
        {
            LevelEditor window = (LevelEditor)EditorWindow.GetWindow(typeof(LevelEditor));
            window.title = LevelEditorConstants.LevelEditor;
            window.minSize = new Vector2(LevelEditorConstants.i600, LevelEditorConstants.i300);
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
            Repaint();
        }
        #endregion

        #region DrawEditor
        public void DrawEditor()
        {
            mainRect = EditorGUILayout.BeginVertical(skin.GetStyle(LevelEditorConstants.MainPanel), GUILayout.ExpandHeight(LevelEditorConstants.BoolTrue), GUILayout.ExpandWidth(LevelEditorConstants.BoolTrue));
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
            EditorGUILayout.BeginVertical(skin.GetStyle(LevelEditorConstants.SubPanel), GUILayout.Height(Screen.height * LevelEditorConstants.fZP6), GUILayout.ExpandWidth(LevelEditorConstants.BoolTrue));
            DrawConfigurationTool();
            DrawFileNameAndMapper();
            EditorGUILayout.EndVertical();
        }

        public void DrawFileNameAndMapper()
        {
            if (configuration)
            {
                EditorGUILayout.BeginVertical(GUILayout.ExpandWidth(true));
                GUILayout.FlexibleSpace();
                EditorGUILayout.BeginHorizontal();
                EditorUIUtility.DrawButton(LevelEditorConstants.Map, () => OnMapButtonClick());
                GUILayout.Space(10);
                DrawLevelName();
                EditorGUILayout.EndHorizontal();
                GUILayout.FlexibleSpace();
                EditorGUILayout.EndVertical();
            }
        }

        public void OnMapButtonClick()
        {
            LevelToolPrefabMapperEditor.Initialize();
            LevelToolPrefabMapperEditor creatorEditor = (LevelToolPrefabMapperEditor)EditorWindow.GetWindow(typeof(LevelToolPrefabMapperEditor));
            creatorEditor.InitializeLevelEditorConfiguration(configuration);
        }
        public void DrawConfigurationTool()
        {
            EditorGUILayout.BeginHorizontal();
            DrawLevelEditorConfiguration();
            DrawFileManagementButtons();
            EditorGUILayout.EndHorizontal();
        }

        public void DrawLevelEditorConfiguration()
        {
            GUIContent content = new GUIContent(LevelEditorConstants.EditorConfigurationLable);
            Vector2 size = skin.GetStyle(LevelEditorConstants.Label).CalcSize(content);
            EditorUIUtility.DrawLabel(LevelEditorConstants.EditorConfigurationLable, GUILayout.Width(size.x));
            GUILayout.Space(LevelEditorConstants.i10);
            configuration = (LevelEditorConfiguration)EditorUIUtility.DrawObjectField(configuration, typeof(LevelEditorConfiguration));
        }

        public void DrawFileManagementButtons()
        {
            EditorUIUtility.DrawButton(LevelEditorConstants.New, () => OnNewLevelButtonClick());
            EditorUIUtility.DrawButton(LevelEditorConstants.Save, () => OnSaveButtonClick());
            EditorUIUtility.DrawButton(LevelEditorConstants.SaveAs, () => OnSaveAsButtonClick());
        }
        public void DrawLevelName()
        {
            if (configuration)
            {
                EditorGUILayout.BeginHorizontal();
                GUIStyle lableStyle = new GUIStyle();
                lableStyle.normal.textColor = Color.green;
                lableStyle.fontSize = LevelEditorConstants.i18;
                Vector2 size = skin.GetStyle(LevelEditorConstants.Label).CalcSize(new GUIContent(LevelEditorConstants.LevelName));

                EditorUIUtility.DrawLabel(LevelEditorConstants.LevelName, lableStyle, GUILayout.Width(size.x));
                GUILayout.Space(LevelEditorConstants.i35);
                EditorUIUtility.DrawLabel(configuration.levelData.currentLevel.name, lableStyle);
                EditorGUILayout.EndHorizontal();
            }
        }

        public void OnNewLevelButtonClick()
        {
            LevelCreatorEditor.Initialize();
            LevelCreatorEditor creatorEditor = (LevelCreatorEditor)EditorWindow.GetWindow(typeof(LevelCreatorEditor));
            creatorEditor.InitializeLevelEditorConfiguration(configuration);
        }
        public void OnSaveAsButtonClick()
        {
            LevelSaveEditor.Initialize();
            LevelSaveEditor creatorEditor = (LevelSaveEditor)EditorWindow.GetWindow(typeof(LevelSaveEditor));
            creatorEditor.InitializeLevelEditorConfiguration(configuration);
        }
        #endregion

        #region MainAreaDrawer
        public void DrawMainArea()
        {
            EditorGUILayout.BeginHorizontal(skin.GetStyle(LevelEditorConstants.SubPanel), GUILayout.ExpandWidth(LevelEditorConstants.BoolTrue));
            DrawToolBarContainer();
            DrawGridContainer();
            EditorGUILayout.EndHorizontal();
        }

        public void DrawToolBarContainer()
        {
            Rect toolbarRect = EditorGUILayout.BeginVertical(skin.GetStyle(LevelEditorConstants.MainPanel), GUILayout.ExpandHeight(LevelEditorConstants.BoolTrue), GUILayout.Width(EditorGUIUtility.currentViewWidth * 0.3f));
            DrawEditorToolBox(LevelEditorConstants.i500 / LevelEditorConstants.f2);
            DrawLevelEditorToolBox(LevelEditorConstants.i500 / LevelEditorConstants.f2);
            EditorGUILayout.EndVertical();
        }
        public void DrawGridContainer()
        {
            EditorGUILayout.BeginVertical(skin.GetStyle(LevelEditorConstants.MainPanel), GUILayout.ExpandHeight(LevelEditorConstants.BoolTrue), GUILayout.ExpandWidth(LevelEditorConstants.BoolTrue));
            DrawGrid();
            EditorGUILayout.EndVertical();
        }
        public void OnSaveButtonClick()
        {
            if (configuration.levelData.currentLevelIndex != LevelEditorConstants.iM1)
            {
                Level tempLevel;
                Vector2Int tempGridSize = configuration.levelData.currentLevel.gridSize;
                List<CellView> tempCellView = new List<CellView>();

                foreach (CellView cellView in configuration.levelData.currentLevel.cellViews)
                {
                    tempCellView.Add(new CellView(cellView.cell, cellView.texture));
                }
                tempLevel = new Level(tempGridSize, tempCellView, configuration.levelData.currentLevel.name);
                configuration.levelData.Levels[configuration.levelData.currentLevelIndex] = tempLevel;
            }
        }

        #endregion

        #region DrawToolBarArea
        #region ToolBoxDrawer
        public void DrawEditorToolBox(float height)
        {
            EditorGUILayout.BeginVertical(skin.GetStyle(LevelEditorConstants.ToolBox), GUILayout.Height(height), GUILayout.ExpandWidth(LevelEditorConstants.BoolTrue));
            DrawToolBoxItems();
            EditorGUILayout.EndVertical();
        }
        public void DrawToolBoxItems()
        {
            EditorGUILayout.BeginVertical(skin.GetStyle(LevelEditorConstants.MainPanel));
            toolBoxScrollPosition = EditorGUILayout.BeginScrollView(toolBoxScrollPosition, GUILayout.ExpandWidth(LevelEditorConstants.BoolTrue), GUILayout.ExpandHeight(LevelEditorConstants.BoolTrue));
            for (int index = LevelEditorConstants.i0; index < configuration.toolConfiguration.tools.Count;)
            {
                DrawEditorToolBoxRows(ref index, Mathf.Min((index + LevelEditorConstants.i3), (configuration.toolConfiguration.tools.Count - LevelEditorConstants.i1)));
            }
            EditorGUILayout.EndScrollView();
            EditorGUILayout.EndVertical();

        }
        public void DrawEditorToolBoxRows(ref int index, int count)
        {
            EditorGUILayout.BeginHorizontal(skin.GetStyle(LevelEditorConstants.ToolBox), GUILayout.ExpandWidth(LevelEditorConstants.BoolTrue), GUILayout.Height(LevelEditorConstants.i50));
            GUILayout.FlexibleSpace();
            for (; index <= count; index++)
            {
                GUILayout.FlexibleSpace();
                EditorGUILayout.BeginVertical();
                GUILayout.Space(LevelEditorConstants.i50 / LevelEditorConstants.f2 - LevelEditorConstants.i35 / LevelEditorConstants.f2);
                int tempIndex = index;
                EditorUIUtility.DrawButton(configuration.toolConfiguration.tools[index].gridSprite, () => SelectTool(tempIndex), GUILayout.Height(LevelEditorConstants.i35), GUILayout.Width(LevelEditorConstants.i35));
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
            EditorGUILayout.BeginVertical(skin.GetStyle(LevelEditorConstants.ToolBox), GUILayout.Height(height), GUILayout.ExpandWidth(LevelEditorConstants.BoolTrue));
            levelBoxScrollPosition = EditorGUILayout.BeginScrollView(levelBoxScrollPosition, GUILayout.ExpandWidth(LevelEditorConstants.BoolTrue), GUILayout.ExpandHeight(LevelEditorConstants.BoolTrue));
            DrawLevelButtons();
            EditorGUILayout.EndScrollView();
            EditorGUILayout.EndVertical();
        }
        public void DrawLevelButtons()
        {
            for (int index = LevelEditorConstants.i0; index < configuration.levelData.Levels.Count; index++)
            {
                EditorGUILayout.BeginHorizontal(skin.GetStyle(LevelEditorConstants.ToolBox));
                EditorUIUtility.DrawButton(configuration.levelData.Levels[index].name, () => OnLevelButtonClicked(index), GUILayout.Height(LevelEditorConstants.i45), GUILayout.ExpandWidth(LevelEditorConstants.BoolTrue));
                EditorUIUtility.DrawButton(LevelEditorConstants.X, () => OnLevelDeleteButtonClicked(index), GUILayout.Height(LevelEditorConstants.i45), GUILayout.ExpandWidth(LevelEditorConstants.BoolTrue));
                EditorUIUtility.DrawButton(LevelEditorConstants.Load, ()=> OnLoadButtonClick(index), GUILayout.Height(LevelEditorConstants.i45), GUILayout.ExpandWidth(LevelEditorConstants.BoolTrue));
                EditorGUILayout.EndHorizontal();
            }
        }
        public void OnLevelDeleteButtonClicked(int index)
        {
            if (configuration.levelData.currentLevelIndex == index)
            {
                configuration.levelData.currentLevelIndex = LevelEditorConstants.iM1;
            }
            configuration.levelData.Levels.RemoveAt(index);
            Repaint();
        }
        public void OnLoadButtonClick(int levelIndex)
        {
            Level tempLevel = configuration.levelData.Levels[levelIndex];
            List<CellView> cellViews = tempLevel.cellViews.FindAll(x=>x.cell.toolId!=-1);
            GameObject parentGameObject = new GameObject("Level Parent");
            for(int indexOfCell=0;indexOfCell<tempLevel.cellViews.Count;indexOfCell++)
            {
                if(tempLevel.cellViews[indexOfCell].cell.toolId!=-1)
                {
                    Debug.Log(tempLevel.cellViews[indexOfCell].cell.toolId+" : "+ configuration.toolPrefabMapper.prefabs.Count);
                    
                    Vector2 pos = tempLevel.cellViews[indexOfCell].GetPosition(indexOfCell, tempLevel.gridSize);
                    Instantiate(configuration.toolPrefabMapper.prefabs[tempLevel.cellViews[indexOfCell].cell.toolId], new Vector3(pos.x, 0, pos.y),Quaternion.identity,parentGameObject.transform);
                }
            }
        }
        public void OnLevelButtonClicked(int index)
        {
            Level tempLevel;
            Vector2Int tempGridSize = configuration.levelData.Levels[index].gridSize;
            List<CellView> tempCellView = new List<CellView>();
            foreach (CellView cellView in configuration.levelData.Levels[index].cellViews)
            {
                tempCellView.Add(new CellView(cellView.cell, cellView.texture));
            }
            tempLevel = new Level(tempGridSize, tempCellView, configuration.levelData.Levels[index].name);
            configuration.levelData.currentLevel = tempLevel;
            configuration.levelData.currentLevelIndex = index;
            Repaint();
        }
        #endregion

        #endregion

        #region GridArea
        public void DrawGrid()
        {
            gridContainerScrollPosition = EditorGUILayout.BeginScrollView(gridContainerScrollPosition, GUILayout.ExpandWidth(LevelEditorConstants.BoolTrue), GUILayout.ExpandHeight(LevelEditorConstants.BoolTrue));
            int cellIndex = LevelEditorConstants.i0;
            if (configuration.levelData.currentLevel.cellViews.Count > LevelEditorConstants.i0)
            {
                for (int row = LevelEditorConstants.i0; row < configuration.levelData.currentLevel.gridSize.x; row++)
                {
                    EditorGUILayout.BeginHorizontal();
                    for (int colomn = LevelEditorConstants.i0; colomn < configuration.levelData.currentLevel.gridSize.y; colomn++)
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
        }
        public void HandlePanMoveOfGrid()
        {
            if ((Event.current.button == LevelEditorConstants.i0 && Event.current.modifiers == EventModifiers.Alt) || Event.current.button == LevelEditorConstants.i2)
            {
                Vector2 delta = -Event.current.delta;
                gridContainerScrollPosition += delta;
            }
        }
        #endregion
    }
}