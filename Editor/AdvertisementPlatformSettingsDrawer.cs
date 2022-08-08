using UnityEditor;
using UnityEngine;

namespace Jturesson.Advertisements.Editor
{
    [CustomPropertyDrawer(typeof(AdvertisementPlatformSettings))]
    public class AdvertisementPlatformSettingsDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var runtimePlatform = property.FindPropertyRelative("runtimePlatform");
            EditorGUI.PropertyField(position, property,
                new GUIContent(runtimePlatform.enumDisplayNames[runtimePlatform.enumValueIndex],
                    label.tooltip), true);
        }

        public override float GetPropertyHeight(SerializedProperty property,
            GUIContent label)
        {
            return EditorGUI.GetPropertyHeight(property, label, true);
        }
    }
}