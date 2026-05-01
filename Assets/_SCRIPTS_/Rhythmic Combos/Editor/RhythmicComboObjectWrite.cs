using UnityEditor;
using UnityEngine;

public static class RhythmicComboObjectWrite
{
    public static void ApplyRhythmicCombo(RhythmicCombos rhythmicCombos)
    {
        RhythmicCombos newRhythmicCombo = ScriptableObject.CreateInstance<RhythmicCombos>();


        newRhythmicCombo.combos = new Combo[rhythmicCombos.combos.Length];
        for (int i = 0; i < newRhythmicCombo.combos.Length; ++i)
        {
            if (rhythmicCombos.combos[i].comboPieces == null) continue;

            newRhythmicCombo.combos[i] = new Combo();

            newRhythmicCombo.combos[i].name = rhythmicCombos.combos[i].name;
            newRhythmicCombo.combos[i].comboPieces = new ComboPiece[rhythmicCombos.combos[i].comboPieces.Length];

            for (int j = 0; j < newRhythmicCombo.combos[i].comboPieces.Length; ++j)
            {
                if (rhythmicCombos.combos[i].comboPieces[j] == null) continue;

                newRhythmicCombo.combos[i].comboPieces[j] = new ComboPiece();

                newRhythmicCombo.combos[i].comboPieces[j].beatBetweenPrevious = rhythmicCombos.combos[i].comboPieces[j].beatBetweenPrevious;
                newRhythmicCombo.combos[i].comboPieces[j].comboNumber = rhythmicCombos.combos[i].comboPieces[j].comboNumber;
            }
        }


        AssetDatabase.CreateAsset(newRhythmicCombo, "Assets/Resources/Rhythmic Combos/Rhythmic Combos.asset");
        AssetDatabase.Refresh();
    }

}
