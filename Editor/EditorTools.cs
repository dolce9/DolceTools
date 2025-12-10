// Assets/DolceTools/Editor/EditorTools.cs
using UnityEditor;
using UnityEngine;

namespace DolceTools
{
    [InitializeOnLoad]
    public static class EditorTools
    {
        // EditorOnly と Active を条件付きで切り替え（Shift + E）
        public static void ToggleEditorOnlyAndActive()
        {
            foreach (var obj in Selection.gameObjects)
            {
                Undo.RecordObject(obj, "Toggle EditorOnly + Active");

                if (obj.tag == "EditorOnly" && !obj.activeSelf)
                {
                    obj.tag = "Untagged";
                    obj.SetActive(true);
                }
                else
                {
                    obj.tag = "EditorOnly";
                    obj.SetActive(false);
                }
            }
        }

        // 選択中の GameObject に DLT Comment コンポーネントを追加
        public static void AddDLTCommentToSelected()
        {
            GameObject selected = Selection.activeGameObject;
            if (selected == null)
            {
                EditorUtility.DisplayDialog("DolceTools", "GameObject が選択されていません。", "OK");
                return;
            }

            if (selected.GetComponent<DolceTools.DLTComment>() != null)
            {
                EditorUtility.DisplayDialog("DolceTools", "既に DLT Comment が追加されています。", "OK");
                return;
            }

            // コンポーネント追加（Undo 対応）
            var newComp = Undo.AddComponent<DolceTools.DLTComment>(selected);

            // Transform の直下で最前列に移動
            int moveTimes = selected.GetComponents<Component>().Length - 1; // Transform を除く
            for (int i = 0; i < moveTimes; i++)
            {
                UnityEditorInternal.ComponentUtility.MoveComponentUp(newComp);
            }
        }
    }
}