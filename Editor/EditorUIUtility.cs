namespace LevelEditor
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEditor;
    using System;
    public static class EditorUIUtility
    {
        public static void DrawLabel(string labelString, GUIStyle style, params GUILayoutOption[] gUILayoutOption)
        {
            EditorGUILayout.LabelField(labelString, gUILayoutOption);
        }

        public static UnityEngine.Object DrawObjectFieldWithLabel(string lable,UnityEngine.Object obj, Type type, params GUILayoutOption[] gUILayouts)
        {
            return EditorGUILayout.ObjectField(lable, obj, type, true, gUILayouts);
        }
        public static void DrawObjectField(UnityEngine.Object obj, Type type, params GUILayoutOption[] gUILayouts)
        {
            obj = EditorGUILayout.ObjectField(obj, type, true, gUILayouts);
        }

        public static void DrawButton(string name, System.Action action, GUIStyle style, params GUILayoutOption[] gUILayoutOption)
        {
            if (GUILayout.Button(name, style, gUILayoutOption))
            {
                action();
            }
        }
        public static void DrawButton(Texture texture,System.Action action,GUIStyle style,params GUILayoutOption[] gUILayoutOption)
        {
            if(GUILayout.Button(texture))
            {
                action();
            }
        }
    }
}
