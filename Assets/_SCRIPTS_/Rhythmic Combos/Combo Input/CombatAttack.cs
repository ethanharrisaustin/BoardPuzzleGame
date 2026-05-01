using System.Collections.Generic;

[System.Serializable]
public class CombatAttack
{
    public Combo combo;
    public List<CombatInput> inputs;

    public float AverageAccuracy()
    {
        float total = 0f;
        for (int i = 0; i < inputs.Count; ++i) total += inputs[i].accuracy;
        return total / inputs.Count;
    }

    public CombatAttack(Combo combo, List<CombatInput> combatInputs)
    {
        this.combo = combo;
        inputs = combatInputs;
    }
}
