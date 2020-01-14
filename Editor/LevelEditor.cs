namespace LevelEditor {
	using System.Collections.Generic;
	using System.Collections;
	using UnityEditor;
	using UnityEngine;
	using System;
	public class LevelEditor : EditorWindow {

		public LevelEditorConfiguration editorConfiguration;
		public GUISkin skin;
		[MenuItem ("LevelEditor/Editor")]
		static void Initialize () {
			// Get existing open window or if none, make a new one:
			LevelEditor window = (LevelEditor) EditorWindow.GetWindow (typeof (LevelEditor));
			window.titleContent = new GUIContent ("Level Editor");
			window.minSize = new Vector2 (600, 300);
			window.Show ();
		}
		void OnEnable () {
			skin = Resources.Load<GUISkin> ("LevelEditor");
			if (skin == null) {
				Debug.Log ("NotFound");
			} else {
				Debug.Log ("Found");
			}
		}
		public void OnGUI () {
			DrawEditor ();
		}
		public void DrawEditor () {
			EditorGUILayout.BeginVertical (skin.GetStyle("MainPanel"));
			// GUIStyle mainPanelStyle=skin.GetStyle("MainPanel");
			DrawToolBox ();
			EditorGUILayout.EndVertical ();
		}
		public void DrawToolBox () {
			
			EditorGUILayout.BeginHorizontal ();
			DrawEditorConfigurationField ();
			EditorGUILayout.EndHorizontal ();
		}
		public void DrawEditorConfigurationField () {
			DrawLabel("Editor Configuration",skin.GetStyle("label"));
			// editorConfiguration = (LevelEditorConfiguration) EditorGUILayout.ObjectField (editorConfiguration, typeof (LevelEditorConfiguration), true);
			// DrawObjectField(typeof(LevelEditorConfiguration),(object)editorConfiguration);
			DrawEditorOperationButtons();
		}
		public void DrawEditorOperationButtons()
		{
			DrawButton("Load",null,skin.GetStyle("button"));
			DrawButton("Save",null,skin.GetStyle("button"));
		}
		public void DrawLabel(string labelText,GUIStyle labelStyle)
		{
			EditorGUILayout.LabelField (labelText,labelStyle);
		}
		public void DrawObjectField(Type type,UnityEngine.Object obj)
		{
			obj=EditorGUILayout.ObjectField (obj, type, true);
		}
		public void DrawButton(string name,System.Action action,GUIStyle style)
		{
			if(GUILayout.Button(name,style))
			{
				action();
			}
		}
	}
}