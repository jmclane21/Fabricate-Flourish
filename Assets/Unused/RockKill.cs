using UnityEngine;
// thanks chatgpt!

public class RockKill : MonoBehaviour
{
     public Camera mainCamera;  // Reference to the main camera
    public float checkInterval = 1f;  // How often to check (in seconds)
    public float destroyDistance = 50f;  // Distance threshold to destroy rock

    private Renderer rockRenderer;
    private Transform playerTransform;

    void Start()
    {
        // Get the renderer component of the rock
        rockRenderer = GetComponent<Renderer>();

        // Find the player (assuming the player has a "Player" tag)
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;

        // Start checking the rock's visibility at intervals
        InvokeRepeating("CheckRockVisibility", checkInterval, checkInterval);
    }

    void CheckRockVisibility()
    {
        // Check if the rock is visible to the camera
        if (!rockRenderer.isVisible)
        {
            // Option 1: Destroy if not visible to the camera
            Destroy(gameObject);
        }
        else
        {
            // Option 2: Destroy if too far from the player
            float distanceToPlayer = Vector3.Distance(playerTransform.position, transform.position);
            if (distanceToPlayer > destroyDistance)
            {
                Destroy(gameObject);
            }
        }
    }
}
