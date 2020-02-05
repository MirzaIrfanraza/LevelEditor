namespace LevelEditor
{
    using System.Collections.Generic;
    using System.Collections;
    using UnityEditor;
    using UnityEngine;
    [System.Serializable]
    public class CellView
    {
        public Cell cell;
        public Texture texture;
        public CellView(Cell cell)
        {
            this.cell = cell;
            this.texture = null;
        }
        public CellView(Cell cell,Texture texture)
        {
            this.cell = cell;
            this.texture = texture;
        }
        public void DrawCell(LevelEditorTool editorTool, int toolIndex, EditorWindow window)
        {
            EditorUIUtility.DrawButton(texture, () => OnMouseEventDrawSelectedToolTexture(editorTool.gridSprite, toolIndex), GUILayout.Width(25), GUILayout.Height(25));
            HandleHoverDraw(editorTool, toolIndex, window);
        }
        public void OnMouseEventDrawSelectedToolTexture(Texture texture, int toolIndex)
        {
           
            //if (editorTool == null)
            //    return;
            cell.toolId = toolIndex;
            this.texture = texture;
        }
        public void HandleHoverDraw(LevelEditorTool editorTool, int toolIndex, EditorWindow window)
        {
            var rect = GUILayoutUtility.GetLastRect();
            var pos = Event.current.mousePosition;
            if(Event.current.modifiers == EventModifiers.Control && rect.Contains(pos))
            {
                OnMouseEventDrawSelectedToolTexture(null, -1);
                window.Repaint();
            }
            else if (Event.current.modifiers == EventModifiers.Shift && rect.Contains(pos))
            {
                OnMouseEventDrawSelectedToolTexture(editorTool.gridSprite, toolIndex);
                window.Repaint();
            }
        }
        public Vector2 GetPosition(int index, Vector2 gridSize)
        {
            return new Vector2(Mathf.FloorToInt(index / gridSize.x), (index % gridSize.x));
        }
    }
}