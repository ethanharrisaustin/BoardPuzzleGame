using UnityEngine;

public class MusicChangeTrigger : MonoBehaviour
{
    //allows selection of rank in editor
    [Header("Rank")]
    [SerializeField] private Rank currentRank;


    private void Update() //to be replaced with a public "set rank" function
    {
        AudioManager.instance.SetRank(currentRank);
    }
}
