using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace YaTools.SimpleFolders.SimpleFolderColorizer.Editor
{
    [InitializeOnLoad]
    public class FolderColorizer
    {
        private static Texture2D closedFolderTexture;
        private static Texture2D openedFolderTexture;
        private static Texture2D emptyFolderTexture;

        private const double HoldSeconds = 0.8;
        private const float HoldMoveTolerance = 6f;

        private static string holdGuid;
        private static double holdStartTime;
        private static Vector2 holdStartMouse;
        private static Vector2 holdStartScreen;

        private static EditorWindow holdHost;


        private static Dictionary<string, int> guidToInstanceId = new Dictionary<string, int>();
        private static HashSet<int> expandedSet = new HashSet<int>();
        private static Dictionary<string, bool> emptyFoldersCache = new Dictionary<string, bool>();

        private static readonly PropertyInfo expandedItemsProp = typeof(InternalEditorUtility).GetProperty("expandedProjectWindowItems", BindingFlags.Static | BindingFlags.Public);

        private static FolderColorSettings cachedSettings;

        static FolderColorizer()
        {
            EditorApplication.projectWindowItemOnGUI += OnProjectWindowItemGUI;
            EditorApplication.projectChanged += OnProjectChanged;            
        }

        private static void OnProjectChanged()
        {
            guidToInstanceId.Clear();
            emptyFoldersCache.Clear();
            cachedSettings = null;

            closedFolderTexture = null;
            openedFolderTexture = null;
            emptyFolderTexture = null;

            EditorApplication.RepaintProjectWindow();
        }

        private static void RefreshExpandedCache()
        {
            var items = expandedItemsProp?.GetValue(null) as int[];
            expandedSet = items != null ? new HashSet<int>(items) : new HashSet<int>();
        }

        private static void LoadTextures()
        {
            if (closedFolderTexture == null)
                closedFolderTexture = LoadTextureByName("SF_CustomFolder");

            if (openedFolderTexture == null)
                openedFolderTexture = LoadTextureByName("SF_CustomFolderOpened");

            if (emptyFolderTexture == null)
                emptyFolderTexture = LoadTextureByName("SF_CustomFolderEmpty");
        }

        private static Texture2D LoadTextureByName(string name)
        {
            string[] guids = AssetDatabase.FindAssets($"{name} t:Texture2D");

            if (guids.Length > 0)
                return AssetDatabase.LoadAssetAtPath<Texture2D>(
                    AssetDatabase.GUIDToAssetPath(guids[0]));

            return null;
        }

        private static int GetInstanceID(string guid, string path)
        {
            if (guidToInstanceId.TryGetValue(guid, out int id))
                return id;

            Object asset = AssetDatabase.LoadAssetAtPath<Object>(path);
            if (asset != null)
            {
#if UNITY_6000_4_OR_NEWER
                guidToInstanceId[guid] = asset.GetEntityId().GetHashCode();
#else
                guidToInstanceId[guid] = asset.GetInstanceID();
#endif
                return guidToInstanceId[guid];
            }
            return 0;
        }

        private static bool IsFolderExpanded(int instanceID) => expandedSet.Contains(instanceID);

        private static bool IsFolderEmpty(string guid, string path)
        {
            if (emptyFoldersCache.TryGetValue(guid, out bool isEmpty))
                return isEmpty;

            string fullPath = Path.GetFullPath(path);

            if (Directory.Exists(fullPath))
            {
                bool hasFiles = Directory.EnumerateFiles(fullPath).Any(f => !f.EndsWith(".meta"));
                bool hasDirs = Directory.EnumerateDirectories(fullPath).Any();
                isEmpty = !hasFiles && !hasDirs;
            }
            else
            {
                isEmpty = false;
            }

            emptyFoldersCache[guid] = isEmpty;
            return isEmpty;
        }

        private static void OnProjectWindowItemGUI(string guid, Rect selectionRect)
        {
            TrackHold(guid, selectionRect);

            if (Event.current.type != EventType.Repaint) return;

            RefreshExpandedCache();

            string path = AssetDatabase.GUIDToAssetPath(guid);
            if (!AssetDatabase.IsValidFolder(path)) return;

            if (cachedSettings == null)
                cachedSettings = FolderColorSettings.Instance;
            if (cachedSettings == null) return;

            if (!cachedSettings.TryGetColor(guid, out Color folderColor)) return;

            LoadTextures();
            if (closedFolderTexture == null) return;

            bool isListView = selectionRect.height <= 20f;
            bool isListViewMain = selectionRect.x == 14;
            Texture2D textureToDraw = closedFolderTexture;

            if (IsFolderEmpty(guid, path) && emptyFolderTexture != null)
            {
                textureToDraw = emptyFolderTexture;
            }
            else if (openedFolderTexture != null)
            {
                int instanceID = GetInstanceID(guid, path);
                if (IsFolderExpanded(instanceID) && isListView && !isListViewMain)
                    textureToDraw = openedFolderTexture;
            }

            Rect iconRect = isListView ? new Rect(selectionRect.x, selectionRect.y, selectionRect.height, selectionRect.height) : new Rect(selectionRect.x, selectionRect.y, selectionRect.width, selectionRect.width);

            if (isListViewMain)
                iconRect.x += 3;

            GUI.color = folderColor;
            GUI.DrawTexture(iconRect, textureToDraw);
            GUI.color = Color.white;            
        }

        private static void TrackHold(string guid, Rect selectionRect)
        {
            var settings = FolderColorSettings.Instance;
            if (settings != null && !settings.quickPopupOnHold)
                return;

            Event e = Event.current;

            if (e.rawType == EventType.MouseDown && e.button == 0 && selectionRect.Contains(e.mousePosition))
            {
                holdHost = EditorWindow.mouseOverWindow;

                if (!AssetDatabase.IsValidFolder(AssetDatabase.GUIDToAssetPath(guid))) return;

                holdGuid = guid;
                holdStartTime = EditorApplication.timeSinceStartup;
                holdStartMouse = e.mousePosition;
                holdStartScreen = GUIUtility.GUIToScreenPoint(e.mousePosition);

                EditorApplication.update -= HoldUpdate;
                EditorApplication.update += HoldUpdate;
                return;
            }

            if (holdGuid == null) return;

            if (e.rawType == EventType.MouseUp)
                CancelHold();
            else if (e.rawType == EventType.MouseDrag && Vector2.Distance(e.mousePosition, holdStartMouse) > HoldMoveTolerance)
                CancelHold();
        }

        private static void HoldUpdate()
        {
            if (holdGuid == null)
            {
                EditorApplication.update -= HoldUpdate;
                return;
            }
            
            var over = EditorWindow.mouseOverWindow;
            if (over == null || over != holdHost)
            {
                CancelHold();
                return;
            }

            if (EditorApplication.timeSinceStartup - holdStartTime < HoldSeconds) return;

            string guid = holdGuid;
            Vector2 screen = holdStartScreen;
            EditorWindow host = holdHost;
            CancelHold();

            string[] selected = Selection.assetGUIDs;
            string[] targets = selected.Contains(guid) ? selected : new[] { guid };

            QuickColorPickerWindow.Open(host, screen, targets);
        }

        private static void CancelHold()
        {
            holdGuid = null;
            holdHost = null;
            EditorApplication.update -= HoldUpdate;
        }
    }
}