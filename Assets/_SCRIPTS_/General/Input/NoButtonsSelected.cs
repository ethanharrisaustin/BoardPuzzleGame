using UnityEngine;
using UnityEngine.EventSystems;


public class NoButtonsSelected : MonoBehaviour
{
    EventSystem eventSystem;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        eventSystem = EventSystem.current;
    }

    // Update is called once per frame
    void Update()
    {
        if (eventSystem == null) eventSystem = EventSystem.current;

        eventSystem.SetSelectedGameObject(null);
    }
}
