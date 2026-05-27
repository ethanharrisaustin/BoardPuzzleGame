using UnityEngine;
using System;
using UnityEngine.InputSystem;

public class Input : MonoBehaviour
{
    public static Action onCombat1, onCombat2, onCombat3, onCombat4;    

    public static Input instance;

    public static Vector2 movement;

    private PlayerInput playerInput;

    public enum Direction { none, north, east, south, west };

    public ControlScheme currentControlScheme;

    public static Action<ControlScheme> OnControlSchemeChangedEvent;

    public float delayBetweenCombatInputs = 0.1f;

    public enum ControlScheme { Controller, KeyboardMouse, XboxController, PlaystationController, NitendoSwitch,  Error }

    public static Vector2 mousePosition;

    public static bool mouseClicked;

    void Awake()
    {
        if (instance != null && instance != this) Destroy(gameObject);

        instance = this;
    }

    public static Input Main()
    {
        if (instance == null) instance = FindFirstObjectByType<Input>();
        
        return instance;
    }

    float combatInputTimer = 0f;
    void Update()
    {
        SetUpMousePosition();

        combatInputTimer -= Time.deltaTime;
    }

    void Init()
    {
        if (playerInput != null) return;

        playerInput = GetComponent<PlayerInput>();
    }

    public void OnControlSchemeChanged()
    {
        CheckCurrentControlScheme();
    }

    void CheckCurrentControlScheme()
    {
        Init();

        switch (playerInput.currentControlScheme)
        {
            case "Keyboard&Mouse": currentControlScheme = ControlScheme.KeyboardMouse; break;
            case "Gamepad": currentControlScheme = ControlScheme.Controller; break;
            case "XboxController": currentControlScheme = ControlScheme.XboxController; break;
            case "PlaystationController": currentControlScheme = ControlScheme.PlaystationController; break;
            case "NitendoSwitch": currentControlScheme = ControlScheme.NitendoSwitch; break;
            default: currentControlScheme = ControlScheme.Error; break;
        }

        OnControlSchemeChangedEvent?.Invoke(currentControlScheme);
    }

    public void OnCombat1(InputAction.CallbackContext context)
    {
        if (!context.performed || combatInputTimer > 0f) return;

        onCombat1?.Invoke();

        combatInputTimer = delayBetweenCombatInputs;
    }

    public void OnCombat2(InputAction.CallbackContext context)
    {
        if (!context.performed || combatInputTimer > 0f) return;
        
        onCombat2?.Invoke();

        combatInputTimer = delayBetweenCombatInputs;
    }

    public void OnCombat3(InputAction.CallbackContext context)
    {
        if (!context.performed || combatInputTimer > 0f) return;
        
        onCombat3?.Invoke();

        combatInputTimer = delayBetweenCombatInputs;
    }

    public void OnCombat4(InputAction.CallbackContext context)
    {
        if (!context.performed || combatInputTimer > 0f) return;
        
        onCombat4?.Invoke();

        combatInputTimer = delayBetweenCombatInputs;
    }


    public void OnMove(InputAction.CallbackContext context)
    {
        movement = context.ReadValue<Vector2>();
    }

    public static Direction MovementAsDirection(bool favourX = true)
    {
        bool xIsBiggest = favourX ? Mathf.Abs(movement.x) * 0.5f > Mathf.Abs(movement.y) :  Mathf.Abs(movement.x) >= Mathf.Abs(movement.y) * 0.5f;

        if (xIsBiggest)
        {
            if (Mathf.Abs(movement.x) < 0.2f) return Direction.none;
            if (movement.x > 0) return Direction.east;
            return Direction.west;
        }
        else
        {
            if (Mathf.Abs(movement.y) < 0.2f) return Direction.none;
            if (movement.y > 0) return Direction.north;
            return Direction.south;
        }
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            MouseDown();
        }

        else if (context.performed)
        {
            
        }

        else if (context.canceled)
        {
            MouseUp();
        }
    }

    void MouseDown()
    {
        if (ClickingManager.instance != null) ClickingManager.instance.MouseDown();

        if (UI_ItemBoard_CheckDragging.instance != null) UI_ItemBoard_CheckDragging.instance.MouseDown();
    }

    void MouseUp()
    {
        if (ClickingManager.instance != null) ClickingManager.instance.MouseUp();

        if (UI_ItemBoard_CheckDragging.instance != null) UI_ItemBoard_CheckDragging.instance.MouseUp();
    }

    void SetUpMousePosition()
    {
        mousePosition = Mouse.current.position.ReadValue();

        OnMouseMove();
    }

    Vector2 prevMousePos = Vector2.negativeInfinity;
    void OnMouseMove()
    {
        if (prevMousePos == mousePosition) return;

        Vector2 moveAmount = mousePosition - prevMousePos;

        prevMousePos = mousePosition;

        if (ClickingManager.instance != null) ClickingManager.instance.MouseMove(moveAmount, mousePosition);

        if (UI_ItemBoard_CheckDragging.instance != null) UI_ItemBoard_CheckDragging.instance.MouseMove();
    }
}
