using System.Collections.Generic;
using UnityEngine;

public class RB_ComboGhosting : MonoBehaviour
{
    public ObjectPool objectPool;
    RhythmicCombos rhythmicCombos;

    void Awake()
    {
        rhythmicCombos = RhythmicCombos.Get();
    }

    public void ShowComboGhosting(List<CombatInput> currentCombatInputs, out bool isValidCombo, out bool finishedCombo, out Combo potentialCombo)
    {
        List<Combo> potentialCombos = rhythmicCombos.GetPotentialCombos(currentCombatInputs);

        if (potentialCombos.Count == 0)
        {
            isValidCombo = false;
            finishedCombo = false;
            potentialCombo = null;
            return;
        }

        if (rhythmicCombos.CompletedCombo(currentCombatInputs, out potentialCombo))
        {
            isValidCombo = true;
            finishedCombo = true;
            return;
        }

        isValidCombo = true;
        finishedCombo = false;
        potentialCombo = potentialCombos[0];
    }
}
