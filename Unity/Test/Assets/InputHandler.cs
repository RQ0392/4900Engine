using UnityEngine;

// Ensure this script inherits from MonoBehaviour and is inside a class
public class PlayerController : MonoBehaviour
{
    // Declare the boolean variable at the class level
    private bool attackHeld = false;

    private void Update()
    {
        HandleAttackInput();
        HandleMovementInput();
    }

    private void HandleAttackInput()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            attackHeld = true;
            Debug.Log("ATTACK PRESSED");
        }

        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            attackHeld = false;
            Debug.Log("ATTACK RELEASED");
        }

        // Only log this if you specifically need to track the holding state per frame
        if (Input.GetKey(KeyCode.Mouse0))
        {
            Debug.Log("ATTACK HELD"); 
        }
    }

    private void HandleMovementInput()
    {
        float x = 0f;
        float y = 0f;

        // Manual input detection
        if (Input.GetKey(KeyCode.A)) x -= 1f;
        if (Input.GetKey(KeyCode.D)) x += 1f;
        if (Input.GetKey(KeyCode.S)) y -= 1f;
        if (Input.GetKey(KeyCode.W)) y += 1f;

        // Create the move vector and normalize it to fix the diagonal speed boost
        Vector2 move = new Vector2(x, y);
        if (move != Vector2.zero) Debug.Log($"Move {move}");

        float moveSpeed = 5f;
        transform.Translate(new Vector3(move.x, 0, move.y) * moveSpeed * Time.deltaTime);
    }
}