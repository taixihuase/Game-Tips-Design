using Core.UI;
using UnityEditor;
using UnityEditor.EventSystems;

namespace UIEditor
{
    [CustomEditor(typeof(EventTriggerListener), true)]
    public class EventTriggerListenerEditor : EventTriggerEditor
    {
        EventTriggerListener trigger;

        protected override void OnEnable()
        {
            base.OnEnable();

            trigger = serializedObject.targetObject as EventTriggerListener;
        }

        public override void OnInspectorGUI()
        {
            trigger.isThrough = EditorGUILayout.Toggle("Is Through", trigger.isThrough);
            EditorGUILayout.Space();

            base.OnInspectorGUI();
        }
    }
}
