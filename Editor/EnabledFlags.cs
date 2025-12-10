// Assets/DolceTools/Editor/EnabledFlags.cs
using UnityEditor;

namespace DolceTools
{
    public static class EnabledFlags
    {
        public static class Project
        {
            private const string PrefKey_ShowFileExtensions = "DolceTools_ShowFileExtensionsEnabled";
            public static bool ShowFileExtensions
            {
                get => EditorPrefs.GetBool(PrefKey_ShowFileExtensions, true);
                set => EditorPrefs.SetBool(PrefKey_ShowFileExtensions, value);
            }

            private const string PrefKey_ShowFolderFileCount = "DolceTools_ShowFolderFileCountEnabled";
            public static bool ShowFolderFileCount
            {
                get => EditorPrefs.GetBool(PrefKey_ShowFolderFileCount, true);
                set => EditorPrefs.SetBool(PrefKey_ShowFolderFileCount, value);
            }
        }

        public static class Hierarchy
        {
            private const string PrefKey_IconsEnabled = "DolceTools_HierarchyIconsEnabled";
            public static bool IconsEnabled
            {
                get => EditorPrefs.GetBool(PrefKey_IconsEnabled, true);
                set => EditorPrefs.SetBool(PrefKey_IconsEnabled, value);
            }

            private const string PrefKey_EditorOnlyBackgroundEnabled = "DolceTools_HierarchyEditorOnlyBackgroundEnabled";
            public static bool EditorOnlyBackgroundEnabled
            {
                get => EditorPrefs.GetBool(PrefKey_EditorOnlyBackgroundEnabled, true);
                set => EditorPrefs.SetBool(PrefKey_EditorOnlyBackgroundEnabled, value);
            }

            private const string PrefKey_EditorOnlyButtonEnabled = "DolceTools_HierarchyEditorOnlyButtonEnabled";
            public static bool EditorOnlyButtonEnabled
            {
                get => EditorPrefs.GetBool(PrefKey_EditorOnlyButtonEnabled, true);
                set => EditorPrefs.SetBool(PrefKey_EditorOnlyButtonEnabled, value);
            }

            private const string PrefKey_DLTCommentIconEnabled = "DolceTools_Hierarchy_DLTCommentEnabled";
            public static bool DLTCommentIconEnabled   
            {
                get => EditorPrefs.GetBool(PrefKey_DLTCommentIconEnabled, true);
                set => EditorPrefs.SetBool(PrefKey_DLTCommentIconEnabled, value);
            }
        }

        public static class Shortcut
        {
            private const string PrefKey_ToggleEditorOnlyActive = "DolceTools_ToggleEditorOnlyActive";
            public static bool ToggleEditorOnlyActive
            {
                get => EditorPrefs.GetBool(PrefKey_ToggleEditorOnlyActive, true);
                set => EditorPrefs.SetBool(PrefKey_ToggleEditorOnlyActive, value);
            }

            private const string PrefKey_AddDLTCommentToSelected = "DolceTools_AddDLTCommentToSelected";
            public static bool AddDLTCommentToSelected
            {
                get => EditorPrefs.GetBool(PrefKey_AddDLTCommentToSelected, true);
                set => EditorPrefs.SetBool(PrefKey_AddDLTCommentToSelected, value);
            }
        }
    }
}
