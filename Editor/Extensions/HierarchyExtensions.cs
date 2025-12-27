// Assets/DolceTools/Editor/HierarchyExtensions.cs
using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Generic;

namespace DolceTools.Editor
{
    [InitializeOnLoad]
    public static class HierarchyExtensions
    {
        static HierarchyExtensions()
        {
            ApplySetting();
        }

        public static void ApplySetting()
        {
            EditorApplication.hierarchyWindowItemOnGUI -= OnHierarchyGUI;

            // どちらか一方でも有効な場合のみ登録
            bool iconsEnabled = EnabledFlags.Hierarchy.IconsEnabled;
            bool editorOnlyButtonEnabled = EnabledFlags.Hierarchy.EditorOnlyButtonEnabled;

            if (iconsEnabled || editorOnlyButtonEnabled)
            {
                EditorApplication.hierarchyWindowItemOnGUI += OnHierarchyGUI;
            }

            EditorApplication.RepaintHierarchyWindow();
        }

        // Transform を除外
        private static readonly HashSet<Type> ignoreTypes = new()
        {
            typeof(Transform)
        };

        private static void OnHierarchyGUI(int instanceID, Rect rect)
        {
            GameObject obj = EditorUtility.InstanceIDToObject(instanceID) as GameObject;
            if (obj == null) return;

            // EditorOnly背景色
            if (EnabledFlags.Hierarchy.EditorOnlyBackgroundEnabled)
            {
                DrawEditorOnlyBackground(obj, rect);
            }

            // Active + EditorOnly ボタン
            if (EnabledFlags.Hierarchy.EditorOnlyButtonEnabled)
            {
                DrawEditorOnlyAndActiveUI(obj, rect);
            }

            // コンポーネントアイコン
            if (EnabledFlags.Hierarchy.IconsEnabled)
            {
                DrawComponentIcons(obj, rect);
            }

        }

        // ================================
        // 背景色（EditorOnlyのとき）
        // ================================
        private static void DrawEditorOnlyBackground(GameObject obj, Rect rect)
        {
            if (obj.tag == "EditorOnly")
            {
                Rect bgRect = new Rect(
                    rect.x + 16f, // 左側のアイコンを避ける
                    rect.y,
                    rect.width - 16f,
                    rect.height
                );

                EditorGUI.DrawRect(bgRect, new Color(1f, 0.4f, 0.4f, 0.3f));
            }
        }

        // ===========================================
        // Active トグル + EditorOnly ボタン
        // ===========================================
        private static void DrawEditorOnlyAndActiveUI(GameObject obj, Rect rect)
        {
            const float buttonSize = 16f;
            const float buttonSpacing = 2f;
            const float numberOfButtonsFromRight = 2f;

            Rect activeRect = new Rect(
                rect.xMax - buttonSize,        // 一番右
                rect.y + (rect.height - buttonSize) * 0.5f,
                buttonSize,
                buttonSize
            );

            Rect tagRect = new Rect(
                rect.xMax - buttonSize * numberOfButtonsFromRight - buttonSpacing, // その左
                rect.y + (rect.height - buttonSize) * 0.5f,
                buttonSize,
                buttonSize
            );

            // Active チェック
            bool newActive = GUI.Toggle(activeRect, obj.activeSelf, string.Empty);
            if (newActive != obj.activeSelf)
            {
                Undo.RecordObject(obj, "Toggle Active");
                obj.SetActive(newActive);
            }

            // EditorOnly ボタン
            Color prevColor = GUI.color;
            GUI.color = (obj.tag == "EditorOnly") ? Color.red : Color.white;

            if (GUI.Button(tagRect, "E"))
            {
                ToggleEditorOnly(obj);
            }

            GUI.color = prevColor;
        }

        private static void ToggleEditorOnly(GameObject obj)
        {
            Undo.RecordObject(obj, "Toggle EditorOnly Tag");

            if (obj.tag == "EditorOnly")
                obj.tag = "Untagged";
            else
                obj.tag = "EditorOnly";
        }

        // =========================
        // コンポーネントアイコン
        // =========================
        private static void DrawComponentIcons(GameObject obj, Rect rect)
        {
            const float iconSize = 16f;     // ← アイコンサイズはここで定義
            const float isonSpacing = 2f;

            float reservedWidth = GetRightPaddingForIcons();
            float x = rect.xMax - reservedWidth;

            Component[] components = obj.GetComponents<Component>();

            for (int i = components.Length - 1; i >= 0; i--)
            {
                Component comp = components[i];
                if (comp == null) continue;

                Type type = comp.GetType();

                if (ignoreTypes.Contains(type))
                    continue;

                if (type.Name == "DLTComment" && !EnabledFlags.Hierarchy.DLTCommentIconEnabled)
                    continue;

                Texture icon = EditorGUIUtility.ObjectContent(comp, type).image;
                if (icon == null) continue;

                x -= iconSize + isonSpacing;

                Rect iconRect = new Rect(
                    x,
                    rect.y + (rect.height - iconSize) * 0.5f,
                    iconSize,
                    iconSize
                );

                GUI.DrawTexture(iconRect, icon, ScaleMode.ScaleToFit);
            }
        }

        private static float GetRightPaddingForIcons()
        {
            const float defaultPadding = 2f;
            const float buttonSpacing = 2f;
            const float buttonWidth = 16f;

            if (EnabledFlags.Hierarchy.EditorOnlyButtonEnabled)
                return buttonWidth * 2f + buttonSpacing + defaultPadding;
            else
                return defaultPadding;
        }
    }
}
