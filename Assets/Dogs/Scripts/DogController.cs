using UnityEngine;
using UnityEngine.AI;

public class DogController : MonoBehaviour
{
    [SerializeField] private GameObject randomObject;

    public NavMeshAgent agent;
    private Animator animator;

    // Random Behavior
    public float minStateInterval = 3f; // Minimum time between state changes
    public float maxStateInterval = 7f; // Maximum time between state changes
    public Transform walkAreaCenter; // Center of walkable area
    public float walkAreaRadius = 30f; // Radius of walkable area

    public float walkingSpeed;
    public float walkingAcceleration;
    public float runningSpeed;
    public float runningAcceleration;

    private float stateTimer;          // Timer to track when to change state
    private DogState currentState;     // Current behavior state

    private AudioSource audioSource;

    private enum DogState
    {
        Idle,
        Running,
        Walking,
        Barking,
        Sitting,
        Eating
    }

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            Debug.LogError("AudioSource component not found on " + gameObject.name);
        }

        // Get the Animator component on the same GameObject
        animator = GetComponent<Animator>();
        if (animator == null)
        {
            Debug.LogError("Animator component not found on " + gameObject.name);
        }

        // Initialize timer and state
        SetRandomStateInterval();
        ChangeState(DogState.Idle);
    }

    void Update()
    {
        stateTimer -= Time.deltaTime;
        if (stateTimer <= 0f && currentState != DogState.Walking && currentState != DogState.Running)
        {
            // Randomly pick the next state
            DogState nextState = (DogState)Random.Range(1, 5);
            //Debug.Log("Dog State: " + gameObject.name + " " + nextState);
            ChangeState(nextState);

            // Set a new random interval for the next state change
            SetRandomStateInterval();
        }

        // Handle behaviors based on current state
        if (currentState == DogState.Walking || currentState == DogState.Running)
        {
            if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
            {
                //Debug.Log(gameObject.name + " arrived at position: Stop walking");

                // Stop walking and switch to idle
                ChangeState(DogState.Idle);
                SetRandomStateInterval(); // Reset timer for idle state
            }
        }
    }

    void ChangeState(DogState newState)
    {
        currentState = newState;
        Vector3 destination;

        // Stop any currently playing sound
        if (audioSource != null && audioSource.isPlaying)
        {
            audioSource.Stop();
        }

        switch (newState)
        {
            case DogState.Idle:
                agent.isStopped = true;  // Stop movement
                animator.SetInteger("AnimationID", 0); // Breathing Animation
                break;

            case DogState.Walking:
                agent.isStopped = false;
                agent.speed = walkingSpeed; // Adjust speed for walking
                agent.acceleration = walkingAcceleration; // Adjust acceleration for walking
                animator.SetInteger("AnimationID", 2);
                destination = GetRandomPointOnNavMesh();
                //Debug.Log("Random Destination: " + destination);
                agent.SetDestination(destination);
                break;

            case DogState.Running:
                agent.isStopped = false;
                agent.speed = runningSpeed; // Adjust speed for running
                agent.acceleration = runningAcceleration; // Adjust acceleration for running
                animator.SetInteger("AnimationID", 4);
                destination = GetRandomPointOnNavMesh();
                //Debug.Log("Random Destination: " + destination);
                agent.SetDestination(destination);
                break;

            case DogState.Eating:
                agent.isStopped = true;  // Stop movement
                animator.SetInteger("AnimationID", 5);
                break;

            case DogState.Barking:
                agent.isStopped = true;  // Stop movement
                animator.SetInteger("AnimationID", 6);

                // Play the barking sound
                if (audioSource != null && !audioSource.isPlaying)
                {
                    audioSource.Play();
                }
                break;

            case DogState.Sitting:
                agent.isStopped = true;  // Stop movement
                animator.SetInteger("AnimationID", 7);
                break;
        }
    }

    Vector3 GetRandomPointInArea()
    {
        // Generate a random distance within the radius
        float randomDistance = Random.Range(0, walkAreaRadius);
        Vector2 randomPoint = Random.insideUnitCircle * randomDistance;
        Vector3 destination = walkAreaCenter.position + new Vector3(randomPoint.x, 0, randomPoint.y);

        NavMeshHit hit;
        if (NavMesh.SamplePosition(destination, out hit, walkAreaRadius, NavMesh.AllAreas))
        {
            return hit.position; // Return a valid NavMesh position
        }
        return transform.position; // Fallback to current position
    }

    Vector3 GetRandomPointOnNavMesh()
    {
        // Pick a random point within the NavMesh bounds
        Vector3 randomPoint = walkAreaCenter.position + Random.insideUnitSphere * walkAreaRadius;
        randomPoint.y = walkAreaCenter.position.y; // Keep the point at the same height

        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomPoint, out hit, walkAreaRadius, NavMesh.AllAreas))
        {
            return hit.position;
        }
        return transform.position; // Fallback if no point is valid
    }


    void SetRandomStateInterval()
    {
        // Set a random value for the state timer
        stateTimer = Random.Range(minStateInterval, maxStateInterval);
    }
}
