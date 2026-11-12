using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace YaTools.SimpleFolders.SimpleFolderColorizer.Editor
{
    public class QuickColorPickerWindow : EditorWindow
    {        
        private const float Radius = 66f;
        private const float ButtonSize = 40f;
        private const float StaggerMs = 25f;
        private const float OpenDurationSec = 0.3f;

        private static readonly float[] SlotAngles = { -90f, -45f, 0f, 45f, 135f, 180f, 225f };
        private const float ClearAngle = 90f;
        private static readonly Vector2 MenuOffset = new Vector2(0f, -15f);

        private static VisualElement current;

        public static void Open(EditorWindow host, Vector2 screenPosition, string[] targetGuids)
        {
            Close();

            var settings = FolderColorSettings.Instance;
            if (host == null || settings == null) return;

            var root = host.rootVisualElement;
            
            Vector2 center = screenPosition - host.position.position;
            center += MenuOffset;
            
            float margin = Radius + ButtonSize / 2f + 4f;
            Vector2 size = root.layout.size;
            if (!float.IsNaN(size.x) && size.x > margin * 2f)
                center.x = Mathf.Clamp(center.x, margin, size.x - margin);
            if (!float.IsNaN(size.y) && size.y > margin * 2f)
                center.y = Mathf.Clamp(center.y, margin, size.y - margin);

            var overlay = new VisualElement
            {
                name = "sfc-quick-overlay",
                focusable = true,
                pickingMode = PickingMode.Position
            };
            var os = overlay.style;
            os.position = Position.Absolute;
            os.left = 0f;
            os.top = 0f;
            os.right = 0f;
            os.bottom = 0f;
            
            overlay.RegisterCallback<PointerDownEvent>(e =>
            {
                if (ReferenceEquals(e.target, overlay))
                {
                    Close();
                    e.StopPropagation();
                }
            });
            overlay.RegisterCallback<KeyDownEvent>(e =>
            {
                if (e.keyCode == KeyCode.Escape) Close();
            });

            var buttons = new List<VisualElement>();
            var offsets = new List<Vector2>();

            int count = Mathf.Min(settings.quickColors.Count, SlotAngles.Length);
            for (int i = 0; i < count; i++)
            {
                Color color = settings.quickColors[i];
                var button = CreateButton(center, color, string.Empty,
                    $"#{ColorUtility.ToHtmlStringRGB(color)}", i, () =>
                    {
                        FolderPainter.Apply(settings, targetGuids, color,
                            settings.applyToSubfolders, settings.ignoreColored);
                    });

                overlay.Add(button);
                buttons.Add(button);
                offsets.Add(Offset(SlotAngles[i]));
            }

            var clear = CreateButton(center, new Color(0.33f, 0.33f, 0.33f, 1f), "×", "Clear color", count,
                () => FolderPainter.Clear(settings, targetGuids, settings.applyToSubfolders));
            overlay.Add(clear);
            buttons.Add(clear);
            offsets.Add(Offset(ClearAngle));

            if (count == 0)
            {
                var hint = new Label("No quick colors yet");
                hint.pickingMode = PickingMode.Ignore;
                hint.style.position = Position.Absolute;
                hint.style.left = center.x - 70f;
                hint.style.width = 140f;
                hint.style.top = center.y - Radius - 34f;
                hint.style.unityTextAlign = TextAnchor.MiddleCenter;
                hint.style.color = new Color(1f, 1f, 1f, 0.6f);
                hint.style.fontSize = 12f;
                overlay.Add(hint);
            }

            root.Add(overlay);
            current = overlay;
            overlay.Focus();
            
            overlay.schedule.Execute(() => PlayOpenAnimation(overlay, buttons, offsets)).StartingIn(30);
        }

        public static void Close()
        {
            current?.RemoveFromHierarchy();
            current = null;
        }

        private static Vector2 Offset(float angleDeg)
        {
            float rad = angleDeg * Mathf.Deg2Rad;
            return new Vector2(Mathf.Cos(rad), Mathf.Sin(rad)) * Radius;
        }

        private static Button CreateButton(Vector2 center, Color color, string label, string tooltip,
            int staggerIndex, Action onClick)
        {
            var button = new Button(() =>
            {
                onClick?.Invoke();
                Close();
            })
            {
                text = label,
                tooltip = tooltip
            };

            var s = button.style;
            s.position = Position.Absolute;
            s.width = ButtonSize;
            s.height = ButtonSize;
            s.left = center.x - ButtonSize / 2f;
            s.top = center.y - ButtonSize / 2f;          
            s.borderTopLeftRadius = 5f;
            s.borderTopRightRadius = 5f;
            s.borderBottomLeftRadius = 5f;
            s.borderBottomRightRadius = 5f;
            s.borderTopWidth = 2f;
            s.borderRightWidth = 2f;
            s.borderBottomWidth = 2f;
            s.borderLeftWidth = 2f;
            var border = new Color(0.15f, 0.15f, 0.15f, 1f);
            s.borderTopColor = border;
            s.borderRightColor = border;
            s.borderBottomColor = border;
            s.borderLeftColor = border;
            s.backgroundColor = color;
            s.color = Color.white;
            s.fontSize = 24f;
            
            s.opacity = 0f;
            s.scale = new Scale(new Vector3(0.3f, 0.3f, 1f));
            s.translate = new Translate(0f, 0f, 0f);

            s.transitionProperty = new List<StylePropertyName> { "translate", "scale", "opacity" };
            s.transitionDuration = new List<TimeValue> { OpenDurationSec, OpenDurationSec, 0.2f };
            s.transitionTimingFunction = new List<EasingFunction>
            {
                EasingMode.EaseOutBack, EasingMode.EaseOutBack, EasingMode.EaseOut
            };
            s.transitionDelay = new List<TimeValue> { new TimeValue(staggerIndex * StaggerMs, TimeUnit.Millisecond) };

            button.RegisterCallback<PointerEnterEvent>(_ => button.style.scale = new Scale(new Vector3(1.18f, 1.18f, 1f)));
            button.RegisterCallback<PointerLeaveEvent>(_ => button.style.scale = new Scale(Vector3.one));

            return button;
        }

        private static void PlayOpenAnimation(VisualElement overlay, List<VisualElement> buttons, List<Vector2> offsets)
        {
            for (int i = 0; i < buttons.Count; i++)
            {
                var s = buttons[i].style;
                s.translate = new Translate(offsets[i].x, offsets[i].y, 0f);
                s.scale = new Scale(Vector3.one);
                s.opacity = 1f;
            }
            
            overlay.schedule.Execute(() =>
            {
                foreach (var b in buttons)
                    b.style.transitionDelay = new List<TimeValue> { 0f };
            }).StartingIn(700);
        }
    }
}