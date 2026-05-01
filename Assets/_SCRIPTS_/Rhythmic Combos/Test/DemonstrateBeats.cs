using UnityEngine;

public class DemonstrateBeats : MonoBehaviour
{
    [Space(100)]
    public ComboButton addInput;


    [Space(200)]

    public GameObject pref;
    public RhythmicCombos[] rhythmicCombos;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rhythmicCombos = Resources.LoadAll<RhythmicCombos>("");
    }

    // Update is called once per frame
    void Update()
    {

    }
    

}
