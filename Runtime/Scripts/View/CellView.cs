namespace LevelEditor {
	using System.Collections.Generic;
	using System.Collections;
	using LevelEditor;
	using UnityEditor;
	using UnityEngine;
	[System.Serializable]
	public class CellView {
		public Cell cell;
		public Texture texture;
		public CellView (Cell cell, Texture texture) {
			this.cell = cell;
			this.texture = texture;
		}
		public void DrawCell (LevelEditorTool editorTool, int toolIndex,EditorWindow window) {
			EditorUIUtility.DrawButton (texture, () => OnMouseEventDrawSelectedToolTexture (editorTool, toolIndex), GUILayout.Width (25), GUILayout.Height (25));
			HandleHoverDraw (editorTool, toolIndex,window);
		}
		public void OnMouseEventDrawSelectedToolTexture (LevelEditorTool editorTool, int toolIndex) {
			Debug.Log ("OnCellClicked");
			if (editorTool == null)
				return;
			cell.toolId = toolIndex;
			texture = editorTool.gridSprite;
		}
		public void HandleHoverDraw (LevelEditorTool editorTool, int toolIndex,EditorWindow window) {
			var rect = GUILayoutUtility.GetLastRect ();
			var pos = Event.current.mousePosition;
			if ( Event.current.type == EventType.MouseDrag  && (Event.current.button == 0) && rect.Contains (pos)) {
				OnMouseEventDrawSelectedToolTexture (editorTool, toolIndex);
				Event.current.Use();
				window.Repaint();
			}
		}

	}
}