using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;

public class AssetHandler
{
    [OnOpenAsset]
    public static bool OpenEditor(int instanceID, int _)
    {
        RhythmicCombos rhythmicCombo = EditorUtility.EntityIdToObject(instanceID) as RhythmicCombos;

        if (rhythmicCombo != null)
        {
            RhythmicComboWindow.ShowWindow();

            return true;
        }

        return false;
    }
}

[CustomEditor(typeof(RhythmicCombos))]
public class RhythmicComboCustomEditor : Editor
{
    
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        
        if (GUILayout.Button("Open"))
        {
            RhythmicComboWindow.ShowWindow();
        }
    }
    
}
