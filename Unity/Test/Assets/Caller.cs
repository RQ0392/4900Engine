using UnityEngine;

public class Caller : MonoBehaviour
{
    // Drag Receiver object here in the Inspector
    [SerializeField] private Receiver receiver;

    void Start()
    {
        // First, log the outgoing message
        Debug.Log("Hello Friend");

        // Verify reference is not null and call the function
        if (receiver != null)
        {
            receiver.OnCalled();
        }
    }
}