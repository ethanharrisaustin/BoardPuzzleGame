using UnityEngine;
using UnityEngine.InputSystem;

public class CameraMovementInput : MonoBehaviour
{
    public static bool topLeft { get { return up && left; } }
    public static bool topRight { get { return up && right; } }
    public static bool bottomLeft { get { return down && left; } }
    public static bool bottomRight { get { return down && right; } }

    public static bool upLeft { get { return up && left; } }
    public static bool upRight { get { return up && right; } }
    public static bool downLeft { get { return down && left; } }
    public static bool downRight { get { return down && right; } }

    public static bool left, right, up, down;

    public void MoveLeft(InputAction.CallbackContext context)
    {
        if (context.started) left = true;
        else if (context.canceled) left = false;
    }

    public void MoveRight(InputAction.CallbackContext context)
    {
        if (context.started) right = true;
        else if (context.canceled) right = false;
    }

    public void MoveUp(InputAction.CallbackContext context)
    {
        if (context.started) up = true;
        else if (context.canceled) up = false;
    }

    public void MoveDown(InputAction.CallbackContext context)
    {
        if (context.started) down = true;
        else if (context.canceled) down = false;
    }
}
