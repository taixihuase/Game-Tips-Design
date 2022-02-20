using UnityEngine;
using Core.UI;
using UnityEditor;

namespace MyEditor.UI
{
    [CustomEditor(typeof(UITable), true)]
    [CanEditMultipleObjects]
    public class UITableEditor : Editor
    {
        SerializedProperty m_Vertical;
        SerializedProperty m_VerticalSpacing;
        SerializedProperty m_HorizontalSpacing;
        SerializedProperty m_MaxRow;
        SerializedProperty m_MaxColumn;
        SerializedProperty m_ChildAlignment;
        SerializedProperty m_ChildControlWidth;
        SerializedProperty m_ChildControlHeight;
        SerializedProperty m_ChildScaleWidth;
        SerializedProperty m_ChildScaleHeight;
        SerializedProperty m_ChildForceExpandWidth;
        SerializedProperty m_ChildForceExpandHeight;


        protected virtual void OnEnable()
        {
            m_Vertical = serializedObject.FindProperty("m_Vertical");
            m_VerticalSpacing = serializedObject.FindProperty("m_VerticalSpacing");
            m_HorizontalSpacing = serializedObject.FindProperty("m_HorizontalSpacing");
            m_MaxRow = serializedObject.FindProperty("m_MaxRow");
            m_MaxColumn = serializedObject.FindProperty("m_MaxColumn");
            m_ChildAlignment = serializedObject.FindProperty("m_ChildAlignment");
            m_ChildControlWidth = serializedObject.FindProperty("m_ChildControlWidth");
            m_ChildControlHeight = serializedObject.FindProperty("m_ChildControlHeight");
            m_ChildScaleWidth = serializedObject.FindProperty("m_ChildScaleWidth");
            m_ChildScaleHeight = serializedObject.FindProperty("m_ChildScaleHeight");
            m_ChildForceExpandWidth = serializedObject.FindProperty("m_ChildForceExpandWidth");
            m_ChildForceExpandHeight = serializedObject.FindProperty("m_ChildForceExpandHeight");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            EditorGUILayout.PropertyField(m_Vertical, true);
            EditorGUILayout.PropertyField(m_VerticalSpacing, true);
            EditorGUILayout.PropertyField(m_HorizontalSpacing, true);
            if (m_Vertical.boolValue)
            {
                EditorGUILayout.PropertyField(m_MaxRow, true);
            }
            else
            {
                EditorGUILayout.PropertyField(m_MaxColumn, true);
            }
            EditorGUILayout.PropertyField(m_ChildAlignment, true);

            Rect rect = EditorGUILayout.GetControlRect();
            rect = EditorGUI.PrefixLabel(rect, -1, EditorGUIUtility.TrTextContent("Control Child Size"));
            rect.width = Mathf.Max(50, (rect.width - 4) / 3);
            EditorGUIUtility.labelWidth = 50;
            ToggleLeft(rect, m_ChildControlWidth, EditorGUIUtility.TrTextContent("Width"));
            rect.x += rect.width + 2;
            ToggleLeft(rect, m_ChildControlHeight, EditorGUIUtility.TrTextContent("Height"));
            EditorGUIUtility.labelWidth = 0;

            rect = EditorGUILayout.GetControlRect();
            rect = EditorGUI.PrefixLabel(rect, -1, EditorGUIUtility.TrTextContent("Use Child Scale"));
            rect.width = Mathf.Max(50, (rect.width - 4) / 3);
            EditorGUIUtility.labelWidth = 50;
            ToggleLeft(rect, m_ChildScaleWidth, EditorGUIUtility.TrTextContent("Width"));
            rect.x += rect.width + 2;
            ToggleLeft(rect, m_ChildScaleHeight, EditorGUIUtility.TrTextContent("Height"));
            EditorGUIUtility.labelWidth = 0;

            rect = EditorGUILayout.GetControlRect();
            rect = EditorGUI.PrefixLabel(rect, -1, EditorGUIUtility.TrTextContent("Child Force Expand"));
            rect.width = Mathf.Max(50, (rect.width - 4) / 3);
            EditorGUIUtility.labelWidth = 50;
            ToggleLeft(rect, m_ChildForceExpandWidth, EditorGUIUtility.TrTextContent("Width"));
            rect.x += rect.width + 2;
            ToggleLeft(rect, m_ChildForceExpandHeight, EditorGUIUtility.TrTextContent("Height"));
            EditorGUIUtility.labelWidth = 0;

            serializedObject.ApplyModifiedProperties();
        }

        void ToggleLeft(Rect position, SerializedProperty property, GUIContent label)
        {
            bool toggle = property.boolValue;
            EditorGUI.BeginProperty(position, label, property);
            EditorGUI.BeginChangeCheck();
            int oldIndent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;
            toggle = EditorGUI.ToggleLeft(position, label, toggle);
            EditorGUI.indentLevel = oldIndent;
            if (EditorGUI.EndChangeCheck())
            {
                property.boolValue = property.hasMultipleDifferentValues ? true : !property.boolValue;
            }
            EditorGUI.EndProperty();
        }
    }
}