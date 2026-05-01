using UnityEngine;

public static class GetRhythmicCombos
{
    static RhythmicCombos cachedRhythmicCombos = null;

    public static RhythmicCombos Get()
    {
        //if (cachedRhythmicCombos == null) 
        cachedRhythmicCombos = Resources.LoadAll<RhythmicCombos>("")[0];

        return cachedRhythmicCombos;
    }
}
