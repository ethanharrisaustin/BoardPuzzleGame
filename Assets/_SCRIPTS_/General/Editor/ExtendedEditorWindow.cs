using UnityEngine;
using UnityEditor;

public class ExtendedEditorWindow : EditorWindow
{
    protected SerializedObject serializedObject;
    protected SerializedProperty currentProperty;
    protected Object selectedObject;

    string selectedPropertyPath;
    protected SerializedProperty selectedPropterty;

    protected void DrawProperties(SerializedProperty property, bool drawChildren)
    {
        string lastPropPath = string.Empty;

        DrawProperties(property, drawChildren, ref lastPropPath);
    }

    protected void DrawProperties(SerializedProperty property, bool drawChildren, ref string lastPropPath)
    {
        foreach (SerializedProperty p in property)
        {
            DrawProperty(p, drawChildren, ref lastPropPath);
            /* 
            continue;
            if (p.isArray && p.propertyType == SerializedPropertyType.Generic) DrawFoldoutArray(p, drawChildren, ref lastPropPath);

            else DrawProperty(p, drawChildren, ref lastPropPath);
            */
        }
    }

    /*
    void DrawFoldoutArray(SerializedProperty p, bool drawChildren, ref string lastPropPath)
    {
        EditorGUILayout.BeginHorizontal();
        p.isExpanded = EditorGUILayout.Foldout(p.isExpanded, p.displayName);
        EditorGUILayout.EndHorizontal();

        if (p.isExpanded)
        {
            const int indentLevel = 2;

            EditorGUI.indentLevel += indentLevel;

            DrawProperties(p, drawChildren, ref lastPropPath);

            EditorGUI.indentLevel -= indentLevel;
        }
    }
    */

    void DrawProperty(SerializedProperty p, bool drawChildren, ref string lastPropPath)
    {
        if (lastPropPath != string.Empty && p.propertyPath.Contains(lastPropPath)) return;
        lastPropPath = p.propertyPath;

        EditorGUILayout.PropertyField(p, drawChildren);
    }

    protected void DrawSidebar(SerializedProperty prop)
    {
        foreach (SerializedProperty p in prop)
        {
            if (GUILayout.Button(p.displayName))
            {
                selectedPropertyPath = p.propertyPath;
            }
        }

        if (!string.IsNullOrEmpty(selectedPropertyPath))
        {
            selectedPropterty = serializedObject.FindProperty(selectedPropertyPath);
        }
    }

    protected Combo selectedCombo;
    protected void DrawSidebar(RhythmicCombos rhythmicCombos)
    {
        foreach (Combo c in rhythmicCombos.combos)
        {
            if (GUILayout.Button(c.name))
            {
                selectedCombo = c;
            }
        }

    }
}
