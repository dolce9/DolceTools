// Assets/DolceTools/Editor/ProjectExtensions.cs
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.IO;

namespace DolceTools
{
    [InitializeOnLoad]
    public static class ProjectExtensions
    {
        static ProjectExtensions()
        {
            ApplySetting();
        }

        public static void ApplySetting()
        {
            EditorApplication.projectWindowItemOnGUI -= OnProjectWindowGUI;

            // どちらかが ON なら描画フックを登録
            if (EnabledFlags.Project.ShowFileExtensions ||
                EnabledFlags.Project.ShowFolderFileCount)
            {
                EditorApplication.projectWindowItemOnGUI += OnProjectWindowGUI;
            }

            EditorApplication.RepaintProjectWindow();
        }

        private static void OnProjectWindowGUI(string guid, Rect selectionRect)
        {
            InitializeStyleAndColors();

            string path = AssetDatabase.GUIDToAssetPath(guid);

            // ============================================
            // フォルダ
            // ============================================
            if (AssetDatabase.IsValidFolder(path))
            {
                if (EnabledFlags.Project.ShowFolderFileCount)
                {
                    DrawFolderFileCount(path, selectionRect);
                }
                return;
            }

            // ============================================
            // ファイル
            // ============================================
            if (EnabledFlags.Project.ShowFileExtensions)
            {
                DrawFileExtension(path, selectionRect);
            }
        }

        private static GUIStyle labelStyle;
        private static Dictionary<string, Color> extensionColors;

        private static void InitializeStyleAndColors()
        {
            if (labelStyle != null) return;

            labelStyle = new GUIStyle(EditorStyles.boldLabel)
            {
                alignment = TextAnchor.MiddleCenter,
                fontSize = 11
            };

            extensionColors = new Dictionary<string, Color>()
            {
                { ".cs", Color.cyan },

                { ".png", Color.green },
                { ".jpg", Color.green },
                { ".jpeg", Color.green },
                { ".gif", Color.green },
                { ".tga", Color.green },
                { ".exr", Color.green },

                { ".mat", Color.magenta },

                { ".anim", new Color(0.7f, 0.4f, 0.9f) },
                { ".controller", new Color(1f, 0.65f, 0f) },

                { ".prefab", Color.yellow }
            };
        }


        // ---------------------------------------------
        // フォルダのファイル数描画
        // ---------------------------------------------
        private static void DrawFolderFileCount(string folderPath, Rect selectionRect)
        {
            string absolute = Path.GetFullPath(folderPath);

            // サブファイル + サブフォルダをカウント（.meta 除外）
            int fileCount = 0;

            if (Directory.Exists(absolute))
            {
                // ---------- ファイル数（.meta は除外） ----------
                string[] files = Directory.GetFiles(absolute, "*", SearchOption.TopDirectoryOnly);
                foreach (string file in files)
                {
                    string name = Path.GetFileName(file);

                    // Unity に表示されない項目を除外
                    if (name.StartsWith(".")) continue;   // .DS_Store, ._xxx など
                    if (name.EndsWith(".meta")) continue; // .meta は除外

                    fileCount++;
                }

                // ---------- フォルダ数 ----------
                string[] dirs = Directory.GetDirectories(absolute, "*", SearchOption.TopDirectoryOnly);
                foreach (string dir in dirs)
                {
                    string name = Path.GetFileName(dir);

                    // .git や .vs などの隠しフォルダも除外
                    if (name.StartsWith(".")) continue;

                    fileCount++;
                }
            }



            bool isGridView = selectionRect.height > 20f;

            if (isGridView)
            {
                // ====== Grid モード表示：中央大バッジ ======
                string countText = fileCount + "";

                GUIStyle centerStyle = new GUIStyle(labelStyle)
                {
                    alignment = TextAnchor.MiddleCenter,
                    normal = { textColor = Color.black },
                    hover = { textColor = Color.black }
                };

                GUI.Label(selectionRect, countText, centerStyle);
            }
            else
            {
                // ====== List モード表示：右端小ラベル ======
                string countText = fileCount + "";

                GUIStyle rightStyle = new GUIStyle(labelStyle)
                {
                    normal = { textColor = Color.gray },
                    hover = { textColor = Color.gray }
                };

                float padding = 4f;
                Vector2 size = labelStyle.CalcSize(new GUIContent(countText));
                float width = Mathf.Max(10f, size.x + padding);

                Rect rightRect = new Rect(
                    selectionRect.xMax - width - 2,
                    selectionRect.y,
                    width,
                    selectionRect.height
                );

                GUI.Label(rightRect, countText, rightStyle);
            }
        }

        // ---------------------------------------------
        // ファイルの拡張子ラベル描画
        // ---------------------------------------------
        private static void DrawFileExtension(string path, Rect selectionRect)
        {
            // グリッドビュー時は表示しない
            bool isGridView = selectionRect.height > 20f;
            if (isGridView) return;

            string extension = Path.GetExtension(path).ToLower();
            if (string.IsNullOrEmpty(extension)) return;


            
            float padding = 4f;
            string extText = extension;
            Vector2 size = labelStyle.CalcSize(new GUIContent(extText));
            float width = Mathf.Max(50f, size.x + padding);
            float height = Mathf.Min(EditorGUIUtility.singleLineHeight, selectionRect.height - 2f);

            Rect labelRect = new Rect(
                selectionRect.xMax - width - 2,
                selectionRect.y + (selectionRect.height - height) / 2f,
                width,
                height
            );

            // 背景描画
            EditorGUI.DrawRect(labelRect, new Color(0f, 0f, 0f, 0.3f));

            Color color = extensionColors.ContainsKey(extension) ? extensionColors[extension] : Color.gray;

            GUIStyle style = new GUIStyle(labelStyle)
            {
                clipping = TextClipping.Clip,
                alignment = TextAnchor.MiddleCenter,
                normal = { textColor = color },
                hover = { textColor = color }
            };

            labelRect.x += 1f; // 少し右に

            // 文字描画
            GUI.Label(labelRect, extText, style: style);
        }
    }
}
