
using UnityEditor;
using UnityEngine;

public static class RComboCombinationDrawer
{
    static float spaceBetweenX = 30;

    static float baseHeight = 100f;
    static float normalHeightOffset = 0.7f;
        
    public static void DrawCombination(ref Combo combo)
    {
        float startXPos = 200;
        float startYPos = 30;

        // Length button
        SetLength(combo, ref startXPos, ref startYPos);

        for (int i = 0; i < combo.comboPieces.Length; ++i)
        {
            bool isBar = IsBar(i);

            if (combo.comboPieces[i].comboNumber != ComboButton.notSet)
                GUI.DrawTexture(new Rect(startXPos + i * spaceBetweenX - 4, startYPos + NoteYOffset(isBar) - 25, 20, 20), RhythmicComboWindow.ComboButtonTexture(combo.comboPieces[i].comboNumber));

            bool clickedOnButton = GUI.Button(new Rect(startXPos + i * spaceBetweenX, startYPos + NoteYOffset(isBar), 12f, NoteHeight(isBar)), "");

            if (!clickedOnButton) continue;

            EditorComboNoteWindow.ShowWindow(combo.comboPieces[i]); 

            
        }

    }

    static void SetLength(Combo combo, ref float startXPos, ref float startYPos)
    {
        int newComboLength = EditorGUI.DelayedIntField(new Rect(startXPos, startYPos, 300, 20), "Length: ", combo.comboPieces.Length);

        startYPos += 50;

        if (newComboLength == combo.comboPieces.Length) return;

        ComboPiece[] newComboPieces = new ComboPiece[newComboLength];
        for (int i = 0; i < Mathf.Min(newComboPieces.Length, combo.comboPieces.Length); ++i) newComboPieces[i] = combo.comboPieces[i];
        combo.comboPieces = newComboPieces; 
    }

    /*
    public static void DrawCombination()
    {
        EditorGUILayout.BeginHorizontal();

        GUILayout.Space(100);

        for (int i = 0; i < 8; ++i)
        {
            bool isBar = IsBar(i);

            GUILayout.Button("", GUILayout.Width(10), isBar ? GUILayout.Height(100) : GUILayout.Height(70));

            GUILayout.Space(20);

            
        }

        EditorGUILayout.EndHorizontal();
    }
    */

    static bool IsBar(int index)
    {
        return index % 4 == 0;
    }

    static float NoteHeight(bool isBar)
    {
        return baseHeight * (isBar ? 1f : normalHeightOffset);
    }

    static float NoteYOffset(bool isBar)
    {
        return isBar ? 0f : baseHeight * (1f - normalHeightOffset) * 0.5f;
    }
}
