using UnityEngine;
using UnityEditor;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class RhythmicComboWindow : ExtendedEditorWindow
{
    RhythmicCombos rhythmicCombo;

    static Texture notSet, buttonA, buttonB, buttonX, buttonY;

    [MenuItem("Window/Rhythmic Combos")]
    public static void ShowWindow()
    {
        RhythmicComboWindow window = GetWindow<RhythmicComboWindow>("Rhythmic Combos");

        // RhythmicCombo rhythmicCombo = Resources.LoadAll<RhythmicCombo>("")[0];

        //window.serializedObject = new SerializedObject(rhythmicCombo);
    }

    void OnGUI()
    {
        ImportRhythmicCombo();

        DrawWindow();

        if (GUILayout.Button("Apply Changes"))
        {
            Apply();
        }
    }

    EditorSoundManager cachedEditorSoundManager = null;
    void DrawWindow()
    {
        currentProperty = serializedObject.FindProperty("combos");

        EditorGUILayout.BeginHorizontal();

        EditorGUILayout.BeginVertical("box", GUILayout.MaxWidth(150), GUILayout.ExpandHeight(true));

        DrawSidebar(rhythmicCombo);

        if (GUILayout.Button("Play Sample Sound"))
        {
            if (cachedEditorSoundManager == null) cachedEditorSoundManager = new EditorSoundManager();

            cachedEditorSoundManager.CreateMetronomeSounds();
        }

        EditorGUILayout.EndVertical();

        EditorGUILayout.BeginVertical("box", GUILayout.ExpandHeight(true));

        /*
        if (selectedPropterty != null)
        {
            DrawProperties(selectedPropterty, true);
        }
        else
        {
            EditorGUILayout.LabelField("Select an item from the list!");
        }
        */

        if (selectedCombo != null)
        {
            RComboCombinationDrawer.DrawCombination(ref selectedCombo);
        }
        else
        {

        }

        EditorGUILayout.EndVertical();
        EditorGUILayout.EndHorizontal();
    }

    void ImportRhythmicCombo()
    {
        if (serializedObject != null) return;

        rhythmicCombo = Resources.LoadAll<RhythmicCombos>("")[0];

        serializedObject = new SerializedObject(rhythmicCombo);
    }

    void Apply()
    {
        if (rhythmicCombo == null) return;

        serializedObject.ApplyModifiedProperties();

        //File.WriteAllBytes("Assets/Resources/Rhythmic Combos/Rhythmic Combos.asset", ObjectToByteArray(rhythmicCombo));

        RhythmicComboObjectWrite.ApplyRhythmicCombo(rhythmicCombo);
    }

    byte[] ObjectToByteArray(object obj)
    {
        if (obj == null)
            return null;
        BinaryFormatter bf = new BinaryFormatter();
        using (MemoryStream ms = new MemoryStream())
        {
            bf.Serialize(ms, obj);
            return ms.ToArray();
        }
    }

    public static Texture ComboButtonTexture(ComboButton comboButton)
    {
        LoadComboButtonTextures();

        switch (comboButton)
        {
            default: return notSet;

            case ComboButton.buttonA: return buttonA;
            case ComboButton.buttonB: return buttonB;
            case ComboButton.buttonX: return buttonX;
            case ComboButton.buttonY: return buttonY;
        }
    }

    static void LoadComboButtonTextures()
    {
        if (notSet != null) return;

        notSet = Resources.Load<Texture>("Custom Editor/Xbox Inputs/xbox_button_none");
        buttonA = Resources.Load<Texture>("Custom Editor/Xbox Inputs/xbox_button_color_a");
        buttonB = Resources.Load<Texture>("Custom Editor/Xbox Inputs/xbox_button_color_b");
        buttonX = Resources.Load<Texture>("Custom Editor/Xbox Inputs/xbox_button_color_x");
        buttonY = Resources.Load<Texture>("Custom Editor/Xbox Inputs/xbox_button_color_y");
    }
}
