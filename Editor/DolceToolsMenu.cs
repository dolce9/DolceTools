// Assets/DolceTools/Editor/DolceToolsMenu.cs
using UnityEditor;

namespace DolceTools.Editor
{
    public static class DolceToolsMenu
    {
        // EditorOnly + Active トグル
        [MenuItem("Tools/DolceTools/Toggle EditorOnly and Active #e", priority = 0)] // Shift + E
        private static void ToggleEditorOnlyAndActiveMenu()
        {
            if (!EnabledFlags.Shortcut.ToggleEditorOnlyActive) return;
            EditorTools.ToggleEditorOnlyAndActive();
        }

        // DLTComment 追加
        [MenuItem("Tools/DolceTools/Add Comment to Selected GameObject %g", priority = 0)] // Ctrl + G
        private static void AddDLTCommentMenu()
        {
            if (!EnabledFlags.Shortcut.AddDLTCommentToSelected) return;
            EditorTools.AddDLTCommentToSelected();
        }

        // 設定パネル
        [MenuItem("Tools/DolceTools/Settings", priority = 9000)]
        [MenuItem("Window/DolceTools")]
        public static void OpenControlPanel()
        {
            ControlPanel.OpenWindow();
        }
    }
}