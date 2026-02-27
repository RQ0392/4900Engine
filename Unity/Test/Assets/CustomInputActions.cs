using UnityEngine;
using UnityEngine.InputSystem; // Need this for New Input System

public class PlayerController : MonoBehaviour
{
    // --- 1. Fix: Proper variable declarations ---
    private CustomInputActions input; // Use the specific class name, not 'object'
    private bool attackHeld;
    private Vector2 moveInput;

    private void Awake()
    {
        // Initialize the input actions
        input = new CustomInputActions();

        // --- 2. Fix: Subscribing to events ---
        // Your methods in Image 2 need to be linked here to work
        input.Player.Attack.started += AttackPressed;
        input.Player.Attack.canceled += AttackReleased;
    }

    private void OnEnable() => input.Player.Enable();
    private void OnDisable() => input.Player.Disable();

    // Match your logic from Image 2
    private void AttackPressed(InputAction.CallbackContext context)
    {
        attackHeld = true;
        Debug.Log("ATTACK PRESSED");
    }

    private void AttackReleased(InputAction.CallbackContext context)
    {
        attackHeld = false;
        Debug.Log("ATTACK RELEASED");
    }

    private void Update()
    {
        // --- 3. Fix: Read movement vector ---
        moveInput = input.Player.Move.ReadValue<Vector2>();

        if (moveInput != Vector2.zero)
        {
            Vector3 move = new Vector3(moveInput.x, 0, moveInput.y);
            transform.Translate(move * 5f * Time.deltaTime);
        }

        // Logic from Image 1, line 17
        if (attackHeld)
        {
            // Debug.Log("ATTACK HELD");
        }
    }
}