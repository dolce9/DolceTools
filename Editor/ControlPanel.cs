// Assets/DolceTools/Editor/ControlPanel.cs
using UnityEditor;
using UnityEngine;

namespace DolceTools
    {
    public class ControlPanel : EditorWindow
    {
        private bool projectShowFileExtensionsEnabled;
        private bool projectShowFolderFileCountEnabled;
        private bool hierarchyEditorOnlyBackgroundEnabled;
        private bool hierarchyEditorOnlyButtonEnabled;
        private bool hierarchyIconsEnabled;
        private bool hierarchyDltCommentIconEnabled;
        private bool shortcutToggleEditorOnlyActiveEnabled;
        private bool shortcutAddDLTCommentToSelectedEnabled;

        private Texture2D windowIcon;

        private void OnEnable()
        {
            windowIcon = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/DolceTools/DolceTools-icon.png");

            // 設定の初期値を読み込み
            projectShowFileExtensionsEnabled        = EnabledFlags.Project.ShowFileExtensions;
            projectShowFolderFileCountEnabled       = EnabledFlags.Project.ShowFolderFileCount;
            hierarchyEditorOnlyBackgroundEnabled         = EnabledFlags.Hierarchy.EditorOnlyBackgroundEnabled;
            hierarchyEditorOnlyButtonEnabled        = EnabledFlags.Hierarchy.EditorOnlyButtonEnabled;
            hierarchyIconsEnabled                   = EnabledFlags.Hierarchy.IconsEnabled;
            hierarchyDltCommentIconEnabled          = EnabledFlags.Hierarchy.DLTCommentIconEnabled;
            shortcutToggleEditorOnlyActiveEnabled   = EnabledFlags.Shortcut.ToggleEditorOnlyActive;
            shortcutAddDLTCommentToSelectedEnabled  = EnabledFlags.Shortcut.AddDLTCommentToSelected;
        }

        public static void OpenWindow()
        {
            GetWindow<ControlPanel>("DolceTools");
        }

        private void OnGUI()
        {
            GUIStyle titleStyle = new GUIStyle(EditorStyles.boldLabel)
            {
                fontSize = 22,
                fontStyle = FontStyle.Bold
            };
            GUILayout.BeginHorizontal();
            if (windowIcon != null)
            {
                float iconSize = titleStyle.lineHeight;
                GUILayout.Label(windowIcon, GUILayout.Width(iconSize), GUILayout.Height(iconSize));
                GUILayout.Space(6);
            }
            GUILayout.Label("DolceTools for Unity", titleStyle);
            GUILayout.EndHorizontal();
            
            GUILayout.Space(8);

            // ------------------------------
            // Hierarchy 拡張機能
            // ------------------------------
            GUILayout.Label("Hierarchy 拡張機能", EditorStyles.boldLabel);

            hierarchyEditorOnlyBackgroundEnabled = EditorGUILayout.ToggleLeft(
                "EditorOnly背景色を表示",
                hierarchyEditorOnlyBackgroundEnabled
            );

            hierarchyEditorOnlyButtonEnabled = EditorGUILayout.ToggleLeft(
                "EditorOnlyボタンを表示",
                hierarchyEditorOnlyButtonEnabled
            );

            hierarchyIconsEnabled = EditorGUILayout.ToggleLeft(
                "コンポーネントアイコンを表示",
                hierarchyIconsEnabled
            );

            EditorGUI.indentLevel++;
            EditorGUI.BeginDisabledGroup(!hierarchyIconsEnabled);
            hierarchyDltCommentIconEnabled = EditorGUILayout.ToggleLeft(
                "DLT Comment を表示",
                hierarchyDltCommentIconEnabled
            );
            EditorGUI.EndDisabledGroup();
            EditorGUI.indentLevel--;

            EditorGUILayout.Space(12);


            // ==============================
            // Project 拡張機能
            // ==============================
            GUILayout.Label("Project 拡張機能", EditorStyles.boldLabel);

            projectShowFileExtensionsEnabled = EditorGUILayout.ToggleLeft(
                "拡張子を表示",
                projectShowFileExtensionsEnabled
            );

            projectShowFolderFileCountEnabled = EditorGUILayout.ToggleLeft(
                "ファイル数を表示",
                projectShowFolderFileCountEnabled
            );

            GUILayout.Space(12);


            // ==============================
            // Project 拡張子表示
            // ==============================
            GUILayout.Label("ショートカット", EditorStyles.boldLabel);

            // OS に応じてショートカット表記を切り替え
            string keyToggleEditorOnly;
            string keyAddComment;

            #if UNITY_EDITOR_OSX
                keyToggleEditorOnly = "⇧E";
                keyAddComment       = "⌘G";
            #else
                keyToggleEditorOnly = "Shift + E";
                keyAddComment       = "Ctrl + G";
            #endif

            shortcutToggleEditorOnlyActiveEnabled = EditorGUILayout.ToggleLeft(
                $"EditorOnly と Active を同時トグル ({keyToggleEditorOnly})",
                shortcutToggleEditorOnlyActiveEnabled
            );

            shortcutAddDLTCommentToSelectedEnabled = EditorGUILayout.ToggleLeft(
                $"選択中の GameObject に Comment を追加 ({keyAddComment})",
                shortcutAddDLTCommentToSelectedEnabled
            );

            GUILayout.Space(12);


            // ==============================
            // 適用ボタン
            // ==============================
            EditorGUI.BeginDisabledGroup(!IsDirty());
            if (GUILayout.Button("適用"))
            {
                EnabledFlags.Project.ShowFileExtensions        = projectShowFileExtensionsEnabled;
                EnabledFlags.Project.ShowFolderFileCount       = projectShowFolderFileCountEnabled;
                EnabledFlags.Hierarchy.EditorOnlyBackgroundEnabled       = hierarchyEditorOnlyBackgroundEnabled;
                EnabledFlags.Hierarchy.EditorOnlyButtonEnabled       = hierarchyEditorOnlyButtonEnabled;
                EnabledFlags.Hierarchy.IconsEnabled            = hierarchyIconsEnabled;
                EnabledFlags.Hierarchy.DLTCommentIconEnabled   = hierarchyDltCommentIconEnabled;
                EnabledFlags.Shortcut.ToggleEditorOnlyActive = shortcutToggleEditorOnlyActiveEnabled;
                EnabledFlags.Shortcut.AddDLTCommentToSelected  = shortcutAddDLTCommentToSelectedEnabled;

                ProjectExtensions.ApplySetting();
                HierarchyExtensions.ApplySetting();
            }
            EditorGUI.EndDisabledGroup();

            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);


            // ==============================
            // ツール操作
            // ==============================
            GUILayout.Label("その他操作", EditorStyles.boldLabel);
            GUILayout.Space(5);

            if (GUILayout.Button("選択中の GameObject に Comment を追加"))
            {
                EditorTools.AddDLTCommentToSelected();
            }
        }

        private bool IsDirty()
        {
            if (projectShowFileExtensionsEnabled        != EnabledFlags.Project.ShowFileExtensions)        return true;
            if (projectShowFolderFileCountEnabled       != EnabledFlags.Project.ShowFolderFileCount)       return true;
            if (hierarchyEditorOnlyBackgroundEnabled     != EnabledFlags.Hierarchy.EditorOnlyBackgroundEnabled)       return true;
            if (hierarchyEditorOnlyButtonEnabled         != EnabledFlags.Hierarchy.EditorOnlyButtonEnabled)       return true;
            if (hierarchyIconsEnabled                   != EnabledFlags.Hierarchy.IconsEnabled)            return true;
            if (hierarchyDltCommentIconEnabled          != EnabledFlags.Hierarchy.DLTCommentIconEnabled)   return true;
            if (shortcutToggleEditorOnlyActiveEnabled   != EnabledFlags.Shortcut.ToggleEditorOnlyActive)   return true;
            if (shortcutAddDLTCommentToSelectedEnabled  != EnabledFlags.Shortcut.AddDLTCommentToSelected)  return true;
            return false;
        }
    }
}
