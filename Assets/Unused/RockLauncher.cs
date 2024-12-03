using UnityEngine;
// thanks chatgpt!

public class RockLauncher : MonoBehaviour
{
    public float launchForce = 500f;  // The amount of force to apply when launching
    private Rigidbody rockRigidbody;

    void Start()
    {
        // Get the Rigidbody component attached to the rock
        rockRigidbody = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // Check if the player presses the 'F' key
        if (Input.GetKeyDown(KeyCode.F))
        {
            LaunchRock();
        }
    }

    void LaunchRock()
    {
        // Apply a force upwards and forward to "launch" the rock
        Vector3 launchDirection = transform.up + transform.forward;
        rockRigidbody.AddForce(launchDirection * launchForce);
    }
}
