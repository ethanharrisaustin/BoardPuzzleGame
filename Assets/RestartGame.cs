using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class RestartGame : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {
        Keyboard keyboard = Keyboard.current;

        bool ctrlKeyDown = keyboard.ctrlKey.isPressed;

        if (ctrlKeyDown && keyboard.rKey.wasPressedThisFrame)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
