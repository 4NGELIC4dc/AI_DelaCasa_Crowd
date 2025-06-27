using UnityEngine;
using UnityEngine.AI;

public class WanderingAgent : MonoBehaviour
{
    public float wanderRadius = 10f;
    public float wanderTimer = 5f;

    private NavMeshAgent agent;
    private Animator animator;

    private float timer;
    private bool isResting = false;

    private float idleTimer = 0f;
    private float nextIdleTime;       // Random time between idles
    private float currentIdleDuration; // How long idle lasts

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        timer = wanderTimer;

        // Immediately start walking
        animator.SetBool("isWalking", true);
        agent.isStopped = false;

        // Initial walk target
        Wander();

        // Random time until next idle
        nextIdleTime = Random.Range(10f, 30f);
    }

    void Update()
    {
        // Walk or idle state logic
        if (isResting)
        {
            idleTimer += Time.deltaTime;

            if (idleTimer >= currentIdleDuration)
            {
                isResting = false;
                idleTimer = 0f;

                agent.isStopped = false;
                animator.SetBool("isWalking", true);

                timer = wanderTimer; // reset wandering
                nextIdleTime = Random.Range(10f, 30f);
            }
            return;
        }

        timer += Time.deltaTime;

        // Only start idle after nextIdleTime
        if (timer >= nextIdleTime)
        {
            isResting = true;
            currentIdleDuration = Random.Range(3f, 5f); // Per-agent rest duration
            idleTimer = 0f;

            agent.isStopped = true;
            animator.SetBool("isWalking", false);
            return;
        }

        // Regular wander move
        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            Wander(); // Choose new destination
        }

        // Sync animator with actual movement
        bool isMoving = agent.velocity.magnitude > 0.1f && !agent.isStopped;
        animator.SetBool("isWalking", isMoving);
    }

    private void Wander()
    {
        Vector3 newPos = RandomNavSphere(transform.position, wanderRadius, -1);
        agent.SetDestination(newPos);
    }

    public static Vector3 RandomNavSphere(Vector3 origin, float dist, int layermask)
    {
        Vector3 randDirection = Random.insideUnitSphere * dist;
        randDirection += origin;
        NavMesh.SamplePosition(randDirection, out NavMeshHit navHit, dist, layermask);
        return navHit.position;
    }
}
