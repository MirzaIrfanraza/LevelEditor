namespace LevelEditor {
    using System.Collections.Generic;
    using System.Collections;
    using System;
    using UnityEditor;
    using UnityEngine;
    
    [InitializeOnLoad]
    public static class EditorUIUtility {
        public static void DrawLabel (string labelString, GUIStyle style, params GUILayoutOption[] gUILayoutOption) {
            EditorGUILayout.LabelField (labelString, gUILayoutOption);
        }
        public static UnityEngine.Object DrawObjectFieldWithLabel (string lable, UnityEngine.Object obj, Type type, params GUILayoutOption[] gUILayouts) {
            return EditorGUILayout.ObjectField (lable, obj, type, true, gUILayouts);
        }
        public static UnityEngine.Object DrawObjectField (UnityEngine.Object obj, Type type, params GUILayoutOption[] gUILayouts) {
            return EditorGUILayout.ObjectField (obj, type, true, gUILayouts);
        }

        public static void DrawButton (string name, System.Action action, GUIStyle style, params GUILayoutOption[] gUILayoutOption) {
            if (GUILayout.Button (name, style, gUILayoutOption)) {
                action ();
            }
        }
        public static void PrintSomething () {
            Debug.Log ("Print Something :");
        }
        public static void DrawButton (string name, System.Action action, params GUILayoutOption[] gUILayoutOption) {
            if (GUILayout.Button (name, gUILayoutOption)) {
                action ();
            }
        }
        public static void DrawButton (Texture texture, System.Action action, GUIStyle style, params GUILayoutOption[] gUILayoutOption) {
            if (GUILayout.Button (texture, style, gUILayoutOption)) {
                action ();
            }
        }
        public static void DrawButton (Texture texture, System.Action action, params GUILayoutOption[] gUILayoutOption) {
            if (GUILayout.Button (texture, gUILayoutOption)) {
                action ();
            }
        }
    }
}