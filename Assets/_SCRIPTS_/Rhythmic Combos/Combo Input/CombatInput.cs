[System.Serializable]
public class CombatInput
{
    public int[] comboButtonIndexes;
    public float accuracy;

    public CombatInput(int[] comboButtonIndexes, float accuracy)
    {
        this.comboButtonIndexes = comboButtonIndexes;
        this.accuracy = accuracy;
    }
}