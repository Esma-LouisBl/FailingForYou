using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace YaTools.SimpleFolders.SimpleFolderColorizer.Editor
{
    public class FolderColorWindow : EditorWindow
    {
        [SerializeField] private VisualTreeAsset uiAsset;
        [SerializeField] private VisualTreeAsset colorButtonTemplate;

        private VisualElement quickSlotsContainer;
        private readonly List<Button> quickSlotButtons = new List<Button>();        
        private static readonly Color SlotBorderDrag = Color.white;
        private const float SlotBorderWidth = 2f;        

        private VisualElement dragGhost;
        private IVisualElementScheduledItem ghostJiggle;
        private bool isDraggingColor;
        private const float GhostSize = 40f;

        private VisualElement templatesContainer;
        private Toggle subfoldersToggle;
        private Toggle ignoreColoredToggle;
        private Toggle quickPopupToggle;
        private FolderColorSettings settings;
        private bool isManagingPresets = false;
        
        private const float JiggleAngle = 2f; 
        private const float JiggleFrequency = 3.5f;
        private const float JiggleBounce = 0.8f;

        private const float HoverScale = 1.2f;
        private const float HoverScaleDuration = 0.1f;

        private const float RainbowSpan = 0.7f;
        private const float RainbowEnd = 0.95f;
        private static Color HueToColor(float hue) => Color.HSVToRGB(hue, 0.5f, 1f);
        private static float GetRootHue(int index, int count) => count <= 1 ? 0f : Mathf.Lerp(0f, RainbowSpan, (float)index / (count - 1));
        private static Color GetRainbowColor(int index, int count) => HueToColor(GetRootHue(index, count));

        [MenuItem("Tools/YaTools/Simple Folder Colorizer %&#c")]
        [MenuItem("Assets/YaTools/Simple Folder Colorizer")]
        public static void ShowWindow()
        {
            var window = GetWindow<FolderColorWindow>(true, "Simple Folder Colorizer", true);            
            window.minSize = new Vector2(350, 370);            
            window.ShowUtility();
        }

        public void CreateGUI()
        {
            settings = FolderColorSettings.Instance;
            uiAsset.CloneTree(rootVisualElement);

            quickSlotsContainer = rootVisualElement.Q<VisualElement>("quick-colors");            
            templatesContainer = rootVisualElement.Q<VisualElement>("color-templates");
            subfoldersToggle = rootVisualElement.Q<Toggle>("subfolders-toggle");
            ignoreColoredToggle = rootVisualElement.Q<Toggle>("ignore-colored-toggle");
            quickPopupToggle = rootVisualElement.Q<Toggle>("quick-popup-toggle");

            Button btnReset = rootVisualElement.Q<Button>("color-remove-button");
            btnReset.clicked += ResetSelectedFolders;
            
            Button btnAddColor = rootVisualElement.Q<Button>("color-add-button");            
            btnAddColor.clicked += () =>
            {
                UnityEditor.PopupWindow.Show(btnAddColor.worldBound, new AddColorPopup(color =>
                {
                    Undo.RecordObject(settings, "Add Color Preset");

                    if (!settings.customPresets.Contains(color))                    
                        settings.customPresets.Add(color);
                    
                    ApplyColorToSelectedFolders(color);
                    RefreshColorTemplates();
                }));
            };

            Button btnManage = rootVisualElement.Q<Button>("manage-presets-button");
            btnManage.clicked += () =>
            {
                isManagingPresets = !isManagingPresets;
                btnManage.text = isManagingPresets ? "Done" : "Manage Presets...";
                btnManage.style.backgroundColor = isManagingPresets
                    ? new Color(0.25f, 0.45f, 0.7f, 1f)
                    : StyleKeyword.Null;
                RefreshAll();
            };

            Button btnRainbow = rootVisualElement.Q<Button>("color-rainbow-button");
            if (btnRainbow != null)
                btnRainbow.clicked += ApplyRainbowToSelectedFolders;

            if (subfoldersToggle != null)
            {
                subfoldersToggle.SetValueWithoutNotify(settings.applyToSubfolders);
                subfoldersToggle.RegisterValueChangedCallback(e =>
                {
                    settings.applyToSubfolders = e.newValue;
                    EditorUtility.SetDirty(settings);
                });
            }
            
            if(ignoreColoredToggle != null)
            {
                ignoreColoredToggle.SetValueWithoutNotify(settings.ignoreColored);
                ignoreColoredToggle.RegisterValueChangedCallback(e =>
                {
                    settings.ignoreColored = e.newValue;
                    EditorUtility.SetDirty(settings);
                });
            }

            if (quickPopupToggle != null)
            {
                quickPopupToggle.SetValueWithoutNotify(settings.quickPopupOnHold);
                quickPopupToggle.RegisterValueChangedCallback(e =>
                {                    
                    settings.quickPopupOnHold = e.newValue;
                    EditorUtility.SetDirty(settings);
                });
            }

            RefreshAll();

            Undo.undoRedoPerformed += OnUndoRedo;
        }

        private TemplateContainer CreateColorButton(Color color, Action onClick, bool showRemoveMark = false, bool draggable = false)
        {
            var root = colorButtonTemplate.Instantiate();
            var btn = root.Q<Button>("color-button");

            btn.style.backgroundColor = color;
            btn.tooltip = $"#{ColorUtility.ToHtmlStringRGB(color)}";

            var hoverColor = new Color(
                Mathf.Min(color.r * 1.2f, 1f),
                Mathf.Min(color.g * 1.2f, 1f),
                Mathf.Min(color.b * 1.2f, 1f),
                color.a);
            btn.RegisterCallback<MouseEnterEvent>(_ => btn.style.backgroundColor = hoverColor);
            btn.RegisterCallback<MouseLeaveEvent>(_ => btn.style.backgroundColor = color);

            AttachScaleHover(btn);

            if (showRemoveMark)
            {
                btn.text = "×";
                float brightness = 0.299f * color.r + 0.587f * color.g + 0.114f * color.b;
                btn.style.color = brightness > 0.5f ? Color.black : Color.white;
                AttachJiggleOnHover(btn, btn);
            }

            if (draggable)
                MakeDraggable(btn, color);

            btn.clicked += onClick;
            return root;
        }

        private void RefreshAll()
        {
            if (templatesContainer != null)
                RefreshColorTemplates();

            RefreshQuickSlots();
        }

        private void RefreshColorTemplates()
        {
            templatesContainer.Clear();

            for (int i = 0; i < settings.customPresets.Count; i++)
            {
                Color presetColor = settings.customPresets[i];
                int capturedIndex = i;

                Action onClick = isManagingPresets
                    ? () =>
                    {
                        Undo.RecordObject(settings, "Remove Color Preset");
                        settings.customPresets.RemoveAt(capturedIndex);
                        settings.Save();
                        RefreshColorTemplates();
                    }
                    : () => ApplyColorToSelectedFolders(presetColor);

                var colorButton = CreateColorButton(presetColor, onClick, isManagingPresets, draggable: !isManagingPresets);
                templatesContainer.Add(colorButton);
            }
        }

        private void MakeDraggable(VisualElement source, Color color)
        {
            bool pressed = false;
            Vector2 startPosition = default;
            
            source.RegisterCallback<PointerDownEvent>(e =>
            {
                if (e.button != 0) return;
                pressed = true;
                startPosition = e.position;
            }, TrickleDown.TrickleDown);

            source.RegisterCallback<PointerMoveEvent>(e =>
            {
                if (!pressed) return;

                if (!isDraggingColor)
                {
                    if ((e.pressedButtons & 1) == 0) { pressed = false; return; }
                    if (Vector2.Distance(e.position, startPosition) < 6f) return;

                    BeginColorDrag(source, e.pointerId, color);
                }

                UpdateColorDrag(e.position);
            }, TrickleDown.TrickleDown);

            source.RegisterCallback<PointerUpEvent>(e =>
            {
                pressed = false;
                if (!isDraggingColor) return;

                EndColorDrag(source, e.pointerId, e.position, color);
                e.StopImmediatePropagation();
            }, TrickleDown.TrickleDown);

            source.RegisterCallback<PointerCaptureOutEvent>(_ =>
            {
                pressed = false;
                if (isDraggingColor) CancelColorDrag();
            });
        }

        private void BeginColorDrag(VisualElement source, int pointerId, Color color)
        {
            isDraggingColor = true;
            
            if (!source.HasPointerCapture(pointerId))
                source.CapturePointer(pointerId);

            dragGhost = new VisualElement { pickingMode = PickingMode.Ignore };
            var s = dragGhost.style;
            s.position = Position.Absolute;
            s.width = GhostSize;
            s.height = GhostSize;
            s.backgroundColor = color;
            s.borderTopLeftRadius = 6f; 
            s.borderTopRightRadius = 6f;
            s.borderBottomLeftRadius = 6f;
            s.borderBottomRightRadius = 6f;
            s.borderTopWidth = 1f;
            s.borderRightWidth = 1f;
            s.borderBottomWidth = 1f;
            s.borderLeftWidth = 1f;
            var border = new Color(1f, 1f, 1f, 0.7f);
            s.borderTopColor = border;
            s.borderRightColor = border;
            s.borderBottomColor = border;
            s.borderLeftColor = border;

            rootVisualElement.Add(dragGhost);
            ghostJiggle = StartJiggle(dragGhost);

            HighlightSlots(-1);
        }

        private void UpdateColorDrag(Vector2 worldPosition)
        {
            if (dragGhost == null) return;
            
            Vector2 local = rootVisualElement.WorldToLocal(worldPosition);
            dragGhost.style.left = local.x - GhostSize / 2f;
            dragGhost.style.top = local.y - GhostSize / 2f;

            HighlightSlots(FindSlotIndexAt(worldPosition));
        }

        private void EndColorDrag(VisualElement source, int pointerId, Vector2 worldPosition, Color color)
        {
            int slotIndex = FindSlotIndexAt(worldPosition);

            CleanupColorDrag();

            if (source.HasPointerCapture(pointerId))
                source.ReleasePointer(pointerId);

            if (slotIndex < 0)
            {
                RefreshQuickSlots();
                return;
            }

            Undo.RecordObject(settings, "Set Quick Color");
            settings.SetQuickColor(slotIndex, color);
            settings.Save();
            RefreshQuickSlots();
        }

        private void CleanupColorDrag()
        {
            isDraggingColor = false;
            ghostJiggle?.Pause();
            ghostJiggle = null;
            dragGhost?.RemoveFromHierarchy();
            dragGhost = null;
        }

        private void CancelColorDrag()
        {
            CleanupColorDrag();
            RefreshQuickSlots();
        }

        private void ApplyColorToSelectedFolders(Color color)
        {
            FolderPainter.Apply(settings, Selection.assetGUIDs, color, subfoldersToggle.value, ignoreColoredToggle != null && ignoreColoredToggle.value);
        }

        private void ResetSelectedFolders()
        {
            FolderPainter.Clear(settings, Selection.assetGUIDs, subfoldersToggle.value);
        }

        private void ProcessSubfoldersRecursive(string parentPath, Action<string> processGuid)
        {
            foreach (string subfolderPath in AssetDatabase.GetSubFolders(parentPath))
            {
                processGuid(AssetDatabase.AssetPathToGUID(subfolderPath));
                ProcessSubfoldersRecursive(subfolderPath, processGuid);
            }
        }

        private void RefreshQuickSlots()
        {
            if (quickSlotsContainer == null) return;
            quickSlotsContainer.Clear();
            quickSlotButtons.Clear();

            for (int i = 0; i < FolderColorSettings.MaxQuickColors; i++)
            {
                int slotIndex = i;
                bool filled = i < settings.quickColors.Count;
                TemplateContainer root;

                if (filled)
                {
                    Color color = settings.quickColors[i];

                    Action onClick = isManagingPresets
                        ? () =>
                        {
                            Undo.RecordObject(settings, "Remove Quick Color");
                            settings.RemoveQuickColorAt(slotIndex);
                            settings.Save();
                            RefreshQuickSlots();
                        }
                    : () => ApplyColorToSelectedFolders(color);

                    root = CreateColorButton(color, onClick, isManagingPresets);
                }
                else
                {
                    root = colorButtonTemplate.Instantiate();
                }

                var btn = root.Q<Button>("color-button");
                btn.userData = filled;
                SetSlotHighlight(btn, 0);

                quickSlotButtons.Add(btn);
                quickSlotsContainer.Add(root);
            }
        }


        private static void AttachScaleHover(VisualElement element)
        {
            element.style.transitionProperty = new List<StylePropertyName> { "scale", "background-color" };
            element.style.transitionDuration = new List<TimeValue> { HoverScaleDuration, HoverScaleDuration };

            element.RegisterCallback<PointerEnterEvent>(_ => element.style.scale = new Scale(new Vector3(HoverScale, HoverScale, 1f)));
            element.RegisterCallback<PointerLeaveEvent>(_ => element.style.scale = new Scale(Vector3.one));
        }

        private static void SetBorder(IStyle s, StyleFloat width, StyleColor color)
        {
            s.borderTopWidth = width;
            s.borderRightWidth = width;
            s.borderBottomWidth = width;
            s.borderLeftWidth = width;
            s.borderTopColor = color;
            s.borderRightColor = color;
            s.borderBottomColor = color;
            s.borderLeftColor = color;
        }

        private static void SetSlotHighlight(VisualElement slot, int level)
        {
            var s = slot.style;
            bool filled = slot.userData is bool f && f;

            if (level == 0 && filled)
            {
                SetBorder(s, StyleKeyword.Null, StyleKeyword.Null);
                return;
            }

            Color borderColor = level > 0 ? SlotBorderDrag : new Color(1f, 1f, 1f, 0.15f);
            SetBorder(s, SlotBorderWidth, borderColor);

            s.scale = new Scale(level == 2 ? new Vector3(HoverScale, HoverScale, 1f) : Vector3.one);

            if (!filled)
            {
                float a = level == 0 ? 0.06f : (level == 1 ? 0.14f : 0.26f);
                s.backgroundColor = new Color(1f, 1f, 1f, a);
            }
        }

        private void HighlightSlots(int hoveredIndex)
        {
            for (int i = 0; i < quickSlotButtons.Count; i++)
                SetSlotHighlight(quickSlotButtons[i], i == hoveredIndex ? 2 : 1);
        }

        private int FindSlotIndexAt(Vector2 worldPosition)
        {
            for (int i = 0; i < quickSlotButtons.Count; i++)
                if (quickSlotButtons[i].worldBound.Contains(worldPosition)) return i;

            return -1;
        }

        private static IVisualElementScheduledItem StartJiggle(VisualElement element)
        {
            double startTime = EditorApplication.timeSinceStartup;
            float phase = UnityEngine.Random.value * Mathf.PI * 2f;
            float speed = JiggleFrequency * UnityEngine.Random.Range(0.9f, 1.1f);

            return element.schedule.Execute(() =>
            {
                float t = (float)(EditorApplication.timeSinceStartup - startTime);
                float wave = t * speed * Mathf.PI * 2f + phase;

                element.style.rotate = new Rotate(Angle.Degrees(Mathf.Sin(wave) * JiggleAngle));
                element.style.translate = new StyleTranslate(
                    new Translate(0f, Mathf.Sin(wave * 1.3f) * JiggleBounce, 0f));
            }).Every(16);
        }

        private static void ResetJiggle(VisualElement element)
        {
            element.style.rotate = StyleKeyword.Null;
            element.style.translate = StyleKeyword.Null;
        }

        private static void AttachJiggleOnHover(VisualElement hoverTarget, VisualElement jiggling)
        {
            IVisualElementScheduledItem jiggle = null;

            hoverTarget.RegisterCallback<PointerEnterEvent>(_ =>
            {
                jiggle?.Pause();
                jiggle = StartJiggle(jiggling);
            });

            hoverTarget.RegisterCallback<PointerLeaveEvent>(_ =>
            {
                jiggle?.Pause();
                ResetJiggle(jiggling);
            });
        }

        private void ApplyRainbowToSelectedFolders()
        {
            bool applyToSubfolders = subfoldersToggle.value;
            bool ignoreColored = ignoreColoredToggle != null && ignoreColoredToggle.value;

            var roots = Selection.assetGUIDs
                 .Select(AssetDatabase.GUIDToAssetPath)
                 .Where(AssetDatabase.IsValidFolder)
                 .OrderBy(p => p, StringComparer.OrdinalIgnoreCase)
                 .ToList();

            if (roots.Count == 0) return;

            Undo.RecordObject(settings, "Rainbow Color Folders");

            if (applyToSubfolders)
            {                
                var topLevel = roots.Where(r => !roots.Any(o => o != r && r.StartsWith(o + "/"))).ToList();

                for (int i = 0; i < topLevel.Count; i++)
                {
                    float hue = GetRootHue(i, topLevel.Count);
                    float bandEnd = i < topLevel.Count - 1 ? GetRootHue(i + 1, topLevel.Count) : RainbowEnd;
                    PaintRainbowFamily(topLevel[i], hue, bandEnd, ignoreColored);
                }
            }
            else
            {
                var paintable = ignoreColored
                    ? roots.Where(p => !settings.TryGetColor(AssetDatabase.AssetPathToGUID(p), out _)).ToList()
                    : roots;

                for (int i = 0; i < paintable.Count; i++)
                {
                    string guid = AssetDatabase.AssetPathToGUID(paintable[i]);
                    settings.SetFolderColor(guid, GetRainbowColor(i, paintable.Count));
                }
            }

            settings.Save();
        }

        private void PaintRainbowFamily(string path, float hue, float bandEnd, bool ignoreColored)
        {
            string guid = AssetDatabase.AssetPathToGUID(path);
            
            if (!(ignoreColored && settings.TryGetColor(guid, out _)))
                settings.SetFolderColor(guid, HueToColor(hue));

            var children = AssetDatabase.GetSubFolders(path)
                .OrderBy(p => p, StringComparer.OrdinalIgnoreCase)
                .ToArray();

            int n = children.Length;
            if (n == 0) return;

            for (int j = 0; j < n; j++)
            {
                float childHue = Mathf.Lerp(hue, bandEnd, (j + 1f) / (n + 1f));
                float childBandEnd = j < n - 1
                    ? Mathf.Lerp(hue, bandEnd, (j + 2f) / (n + 1f))
                    : bandEnd;

                PaintRainbowFamily(children[j], childHue, childBandEnd, ignoreColored);
            }
        }

        private void OnDisable() => Undo.undoRedoPerformed -= OnUndoRedo;
        private void OnUndoRedo() => RefreshAll();
    }
}