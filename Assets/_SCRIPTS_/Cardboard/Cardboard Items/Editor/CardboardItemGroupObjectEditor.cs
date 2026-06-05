using UnityEngine;
using UnityEditor;

namespace Cardboard
{
    [CustomEditor(typeof(CardboardItemGroupObject))]
    public class CardboardItemGroupObjectEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            SerializedProperty property = serializedObject.FindProperty("cardboardItemObjects");

            EditorGUILayout.PropertyField(property);

            serializedObject.ApplyModifiedProperties();
        }
    }
}