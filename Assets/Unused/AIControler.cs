using UnityEngine;
using UnityEngine.AI;

// thank you chatgpt :3
public class AIController : MonoBehaviour
{
    public float detectionRadius = 5f;  // Detection radius
    public float viewAngle = 60f;        // Field of view angle
    public LayerMask targetMask;          // Layer for the player
    public LayerMask obstacleMask;        // Layer for obstacles

    private NavMeshAgent agent;           // Reference to the NavMeshAgent

    private void Start()
    {
        // Get the NavMeshAgent component
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        DetectAndMove();
    }

    private void DetectAndMove()
    {
        // Check for targets within the detection radius
        Collider[] targetsInViewRadius = Physics.OverlapSphere(transform.position, detectionRadius, targetMask);

        foreach (var target in targetsInViewRadius)
        {
            Transform targetTransform = target.transform;
            Vector3 directionToTarget = (targetTransform.position - transform.position).normalized;

            // Check if the target is within the view angle
            if (Vector3.Angle(transform.forward, directionToTarget) < viewAngle / 2)
            {
                float distanceToTarget = Vector3.Distance(transform.position, targetTransform.position);

                // Check for obstacles
                if (!Physics.Raycast(transform.position, directionToTarget, distanceToTarget, obstacleMask))
                {
                    Debug.Log("Player detected: " + targetTransform.name);
                    MoveTowards(targetTransform);
                }
                else
                {
                    Debug.Log("Obstacle detected between AI and target.");
                }
            }
        }
    }

    private void MoveTowards(Transform target)
    {
        if (agent != null)
        {
            agent.SetDestination(target.position); // Set destination to player position
            Debug.Log("Moving towards: " + target.name);
        }
    }

    private void OnDrawGizmos()
    {
        // Draw detection radius
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);

        // Draw field of view
        Vector3 leftBoundary = Quaternion.Euler(0, -viewAngle / 2, 0) * transform.forward * detectionRadius;
        Vector3 rightBoundary = Quaternion.Euler(0, viewAngle / 2, 0) * transform.forward * detectionRadius;

        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + leftBoundary);
        Gizmos.DrawLine(transform.position, transform.position + rightBoundary);
    }
}
