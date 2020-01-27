namespace LevelEditor
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEditor;
    public class NewLevelEditor : PopupWindowContent
    {

        public override void OnGUI(Rect rect)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Level : ");
            
            EditorGUILayout.EndHorizontal();
        }
        public override void OnClose()
        {
            base.OnClose();
            Debug.Log("OnClose");
        }
        public override void OnOpen()
        {
            base.OnOpen();
        }
    }
}
