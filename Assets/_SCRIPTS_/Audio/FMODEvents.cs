using UnityEngine;
using FMODUnity;

//this object contains all FMOD Audio Events we need in the scene 
public class FMODEvents : MonoBehaviour
{
    [field: Header("Rank System")]
    [field: SerializeField] public EventReference music {  get; private set; }
    public static FMODEvents instance { get; private set; }

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Found more than one instance of FMODEvents Script");
        }
        instance = this;
    }
}
