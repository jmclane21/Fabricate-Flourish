using UnityEngine;
using UnityEngine.AI;

// thank you chatgpt :3
public class AIDetection : MonoBehaviour
{
    public float detectionRadius = 5f; // Detection radius
    public float viewAngle = 60f; // Field of view angle
    public LayerMask targetMask; // Layer for the player
    public LayerMask obstacleMask; // Layer for obstacles

    private NavMeshAgent agent; // Reference to the NavMeshAgent

    private void Start()
    {
        // Get the NavMeshAgent component
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        DetectTargets();
    }

    private void DetectTargets()
    {
        Collider[] targetsInViewRadius = Physics.OverlapSphere(transform.position, detectionRadius, targetMask);
        Debug.Log("Targets found: " + targetsInViewRadius.Length);

        foreach (var target in targetsInViewRadius)
        {
            Transform targetTransform = target.transform;
            Vector3 directionToTarget = (targetTransform.position - transform.position).normalized;

            Debug.Log("Direction to Target: " + directionToTarget);
            Debug.Log("AI Forward Direction: " + transform.forward);
            Debug.Log("Angle to Target: " + Vector3.Angle(transform.forward, directionToTarget));

            // Check if the target is within the view angle
            if (Vector3.Angle(transform.forward, directionToTarget) < viewAngle / 2)
            {
                float distanceToTarget = Vector3.Distance(transform.position, targetTransform.position);
                Debug.Log("Distance to target: " + distanceToTarget);

                // Check for obstacles using Raycast
                if (!Physics.Raycast(transform.position, directionToTarget, distanceToTarget, obstacleMask))
                {
                    Debug.Log("Target detected: " + targetTransform.name);
                    MoveTowards(targetTransform); // Only move if detected
                }
                else
                {
                    Debug.Log("Obstacle detected between AI and target.");
                }
            }
            else
            {
                Debug.Log("Target is outside view angle.");
            }
        }
    }

    private void MoveTowards(Transform target)
    {
        // Check if the target is within the detection radius
        float distanceToTarget = Vector3.Distance(transform.position, target.position);
        if (distanceToTarget <= detectionRadius)
        {
            if (agent != null)
            {
                // Move towards the target's position only if detected
                agent.SetDestination(target.position); // Move towards the target's position
                Debug.Log("Moving towards: " + target.name);
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius); // Draw detection radius

        Vector3 leftBoundary = Quaternion.Euler(0, -viewAngle / 2, 0) * transform.forward * detectionRadius;
        Vector3 rightBoundary = Quaternion.Euler(0, viewAngle / 2, 0) * transform.forward * detectionRadius;

        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + leftBoundary); // Left boundary
        Gizmos.DrawLine(transform.position, transform.position + rightBoundary); // Right boundary
    }
}
