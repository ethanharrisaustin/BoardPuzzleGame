using UnityEditor;
using UnityEngine;

public class EditorComboNoteWindow : EditorWindow
{
    static EditorComboNoteWindow window;

    static ComboPiece comboPiece;



    static ComboButton newComboButton;

    public static void ShowWindow(ComboPiece comboPiece)
    {
        window = GetWindow<EditorComboNoteWindow>("Editing Combo Input");

        SetWindowSize(350f, 170);

        EditorComboNoteWindow.comboPiece = comboPiece;

        newComboButton = comboPiece.comboNumber;
    }

    void OnGUI()
    {
        if (comboPiece == null || window == null)
        {
            Close();
            return;
        }

        GUI.DrawTexture(new Rect(350f / 2f - 25, 20, 50, 50), RhythmicComboWindow.ComboButtonTexture(newComboButton));

        GUILayout.Space(70);

        newComboButton = (ComboButton)EditorGUILayout.EnumPopup("Combo Input", newComboButton);

        GUILayout.Space(50);

        EditorGUILayout.BeginHorizontal();

        if (GUILayout.Button("Cancel"))
        {
            Close();
        }

        if (GUILayout.Button("Apply"))
        {
            comboPiece.comboNumber = newComboButton;
            Close();
        }

        EditorGUILayout.EndHorizontal();
    }

    static void SetWindowSize(float width, float height)
    {
        window.position = new Rect(Screen.width * 0.5f - width * 0.5f, Screen.height * 0.5f - height, width, height);

        window.maxSize = new Vector2(width, height);
        window.minSize = new Vector2(width, height);
    }

    
}
