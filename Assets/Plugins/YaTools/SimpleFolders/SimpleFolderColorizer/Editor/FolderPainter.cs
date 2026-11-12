using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace YaTools.SimpleFolders.SimpleFolderColorizer.Editor
{
    internal static class FolderPainter
    {
        public static void Apply(FolderColorSettings settings, IEnumerable<string> guids, Color color, bool includeSubfolders, bool ignoreColored)
        {
            if (settings == null) return;

            Undo.RecordObject(settings, "Color Folders");

            void TrySet(string guid)
            {
                if (ignoreColored && settings.TryGetColor(guid, out _)) return;

                settings.SetFolderColor(guid, color);
            }

            ForEachFolder(guids, includeSubfolders, TrySet);
            settings.Save();
        }   

        public static void Clear(FolderColorSettings settings, IEnumerable<string> guids, bool includeSubfolders)
        {
            if (settings == null) return;

            Undo.RecordObject(settings, "Reset Folder Colors");
            ForEachFolder(guids, includeSubfolders, settings.RemoveFolderColor);
            settings.Save();
        }

        private static void ForEachFolder(IEnumerable<string> guids, bool includeSubfolders, Action<string> action)
        {
            foreach (string guid in guids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                if (!AssetDatabase.IsValidFolder(path)) continue;

                action(guid);

                if (includeSubfolders)
                    VisitSubfolders(path, action);
            }
        }

        private static void VisitSubfolders(string parentPath, Action<string> action)
        {
            foreach (string subfolderPath in AssetDatabase.GetSubFolders(parentPath))
            {
                action(AssetDatabase.AssetPathToGUID(subfolderPath));
                VisitSubfolders(subfolderPath, action);
            }
        }
    }
}