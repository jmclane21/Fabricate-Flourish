using UnityEngine;
using UnityEngine.AI;

// thanks chatGPT :3
public class FSMController : MonoBehaviour
{
    private enum State
    {
        Idle,
        Wandering,
        Chasing
    }

    private State currentState;
    private NavMeshAgent agent;

    public Transform player;                // Reference to the player
    public float detectionRange = 10f;      // Range to detect the player
    public float wanderRadius = 5f;          // Radius for wandering
    public float wanderTimer = 2f;           // Time between wanders
    private float timer;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        currentState = State.Idle;          // Start in the Idle state
        timer = wanderTimer;
    }

    private void Update()
    {
        switch (currentState)
        {
            case State.Idle:
                HandleIdle();
                break;

            case State.Wandering:
                HandleWandering();
                break;

            case State.Chasing:
                HandleChasing();
                break;
        }
    }

    private void HandleIdle()
    {
        // Transition to Wandering after a timer
        timer += Time.deltaTime;
        if (timer >= wanderTimer)
        {
            currentState = State.Wandering;
            timer = 0;
        }

        // Check if the player is within detection range
        if (Vector3.Distance(transform.position, player.position) < detectionRange)
        {
            currentState = State.Chasing;
        }
    }

    private void HandleWandering()
    {
        // Move to a random point within the wander radius
        Wander();

        // Check if the player is within detection range
        if (Vector3.Distance(transform.position, player.position) < detectionRange)
        {
            currentState = State.Chasing;
        }

        // Transition back to Idle after a set time
        timer += Time.deltaTime;
        if (timer >= wanderTimer)
        {
            currentState = State.Idle;
            timer = 0;
        }
    }

    private void HandleChasing()
    {
        // Move towards the player
        agent.SetDestination(player.position);

        // Transition back to Idle if player is too far
        if (Vector3.Distance(transform.position, player.position) > detectionRange)
        {
            currentState = State.Idle;
        }
    }

    private void Wander()
    {
        Vector3 randomDirection = Random.insideUnitSphere * wanderRadius;
        randomDirection += transform.position;

        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomDirection, out hit, wanderRadius, NavMesh.AllAreas))
        {
            agent.SetDestination(hit.position);
            Debug.Log("Wandering to: " + hit.position);
        }
    }
    
    private void OnDrawGizmos()
    {
        // Draw the detection range
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }
}
