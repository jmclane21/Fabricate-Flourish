using UnityEngine;
using UnityEngine.AI;

// thank you chatgpt :3
public class TimedWanderingAIController : MonoBehaviour
{
    public float wanderRadius = 10f;            // Radius for wandering
    public float wanderTimer = 5f;               // Time between each wander
    public float totalWanderDuration = 30f;      // Total time the AI will wander
    public LayerMask obstacleMask;               // Layer mask for obstacles

    private NavMeshAgent agent;                  // Reference to NavMeshAgent
    private float timer;                          // Timer to track when to wander
    private float totalTime;                      // Timer for total wandering duration

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        timer = wanderTimer;
        totalTime = totalWanderDuration;          // Initialize total wandering time
    }

    private void Update()
    {
        // If total time has elapsed, stop wandering
        if (totalTime <= 0)
        {
            agent.isStopped = true;               // Stop the agent
            return;                                // Exit the Update method
        }

        timer += Time.deltaTime;                   // Update the wander timer
        totalTime -= Time.deltaTime;               // Decrease total time

        if (timer >= wanderTimer)
        {
            Wander();                              // Call the Wander method
            timer = 0;                            // Reset the timer
        }
    }

    private void Wander()
    {
        // Get a random point within the wander radius
        Vector3 randomDirection = Random.insideUnitSphere * wanderRadius;
        randomDirection += transform.position;   // Offset by the agent's current position

        // Find the nearest point on the NavMesh
        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomDirection, out hit, wanderRadius, NavMesh.AllAreas))
        {
            // Set the destination for the agent to move to
            agent.SetDestination(hit.position);
            Debug.Log("Wandering to: " + hit.position);
        }
    }
    
    private void OnDrawGizmos()
    {
        // Draw the wandering area
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, wanderRadius);
    }
}
