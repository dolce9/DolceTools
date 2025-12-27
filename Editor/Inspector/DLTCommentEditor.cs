// Assets/DolceTools/Editor/DLTCommentEditor.cs
#if UNITY_EDITOR

using UnityEngine;
using UnityEditor;

namespace DolceTools.Editor
{
    [CustomEditor(typeof(DLTComment))]
    public class DLTCommentEditor : UnityEditor.Editor
    {
        private SerializedProperty commentTextProp;

        private void OnEnable()
        {
            commentTextProp = serializedObject.FindProperty("commentText");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.BeginVertical("HelpBox");
            EditorGUILayout.PropertyField(commentTextProp,
                new GUIContent("コメント（NDMF によりビルド時に削除されます）")
            );
            EditorGUILayout.EndVertical();

            serializedObject.ApplyModifiedProperties();
        }
    }
}

#endif
