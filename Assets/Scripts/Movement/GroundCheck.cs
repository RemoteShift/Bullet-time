using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    [Header("Slope Settings")]
    [SerializeField] private float minNormalY = 0.5f; // Threshold: y > 0.5 (slopes up to ~60 deg)

    [Header("Debug")]
    [SerializeField] private bool isGrounded;
    
    public bool IsGrounded => isGrounded;
    public Vector3 GroundNormal { get; private set; } = Vector3.up;

    private bool evaluatedThisFrame;

    public void SetGroundedState(bool grounded)
    {
        isGrounded = grounded;
        if (!grounded)
        {
            GroundNormal = Vector3.up;
        }
        evaluatedThisFrame = true;
    }

    private void FixedUpdate()
    {
        // Reset grounded status at the start of every physics tick.
        // If OnCollisionStay fires later this tick, it will set it back to true.
        if (!evaluatedThisFrame)
        {
            isGrounded = false;
        }
        
        evaluatedThisFrame = false;
    }

    private void OnCollisionStay(Collision collision)
    {
        var bestNormal = Vector3.zero;
        var bestNormalY = minNormalY;

        // Check every contact point in the collision
        for (int i = 0; i < collision.contactCount; i++)
        {
            ContactPoint contact = collision.GetContact(i);

            // contact.normal points AWAY from the surface into the player
            if (contact.normal.y > bestNormalY)
            {
                bestNormal = contact.normal;
                bestNormalY = contact.normal.y;
            }
        }

        if (bestNormalY > minNormalY)
        {
            isGrounded = true;
            GroundNormal = bestNormal;
            evaluatedThisFrame = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        // Optional safety: if we leave collision completely, reset immediately
        isGrounded = false;
    }
}