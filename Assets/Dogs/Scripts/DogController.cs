using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

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
    public GameObject internalToy;
    public GameObject player;

    public BoxCollider dogCollider;

    private GameObject toy;
    private GameObject bowl;

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
        Eating,
        Catching,
        Returning,

        WigglingTail,
        GettingFood
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

        internalToy.SetActive(false);

        // Initialize timer and state
        SetRandomStateInterval();
        ChangeState(DogState.Idle);
    }

    void Update()
    {

        print(gameObject.name + ": " + currentState);
        stateTimer -= Time.deltaTime;
        if (stateTimer <= 0f && currentState != DogState.Walking && currentState != DogState.Running && currentState != DogState.Catching && currentState != DogState.Returning && currentState != DogState.GettingFood && currentState != DogState.Eating)
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
        if (currentState == DogState.Catching){
            if(!agent.pathPending && agent.remainingDistance<1.0){
                internalToy.SetActive(true);
                this.toy.SetActive(false);
                ReturnToy();
            }
            else{
                agent.SetDestination(toy.transform.position);
            }
        }
        if (currentState == DogState.GettingFood)
        {
            if (!agent.pathPending && agent.remainingDistance < 1.5)
            {
                Eat(this.bowl);
            }
            else
            {
                agent.SetDestination(this.bowl.transform.position);
            }
        }
        if (currentState == DogState.Returning){
            print(agent.remainingDistance);
            if(!agent.pathPending && agent.remainingDistance<6.0){
                internalToy.SetActive(false);
                this.toy.SetActive(true);
                toy.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + 1, gameObject.transform.position.z);
                ChangeState(DogState.Idle);
                print("Returned the toy");
            }
            else{
                agent.SetDestination(player.transform.position);
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

            case DogState.Catching:
                agent.isStopped = false;
                agent.speed = runningSpeed; // Adjust speed for running
                agent.acceleration = runningAcceleration; // Adjust acceleration for running
                animator.SetInteger("AnimationID", 4);
                break;

            case DogState.GettingFood:
                agent.isStopped = false;
                agent.speed = runningSpeed; // Adjust speed for running
                agent.acceleration = runningAcceleration; // Adjust acceleration for running
                animator.SetInteger("AnimationID", 4);
                break;

            case DogState.Returning:
                agent.isStopped = false;
                agent.speed = walkingSpeed; // Adjust speed for walking
                agent.acceleration = walkingAcceleration; // Adjust acceleration for walking
                animator.SetInteger("AnimationID", 2);
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
            case DogState.WigglingTail:
                agent.isStopped = true;  // Stop movement
                animator.SetInteger("AnimationID", 1);
                break;

            case DogState.Sitting:
                agent.isStopped = true;  // Stop movement
                animator.SetInteger("AnimationID", 7);
                break;
        }
    }

    public void CatchToy(GameObject toy)
    {
        ChangeState(DogState.Catching);
        this.toy = toy;
        agent.SetDestination(this.toy.transform.position);
        print(gameObject.name + " is catching the toy");
    }

    public void ReturnToy()
    {
        ChangeState(DogState.Returning);
        agent.SetDestination(player.transform.position);
        print(gameObject.name +" is returning the toy");
    }

    public void GoEat(GameObject bowl)
    {
        ChangeState(DogState.GettingFood);
        this.bowl = bowl;
        agent.SetDestination(this.bowl.transform.position);
        print(gameObject.name + " is going to eat");
    }

    public void Eat(GameObject bowl)
    {
        StartCoroutine(EatRoutine(bowl));
    }

    private IEnumerator EatRoutine(GameObject bowl)
    {
        // Transition to Eating state
        ChangeState(DogState.Eating);
        print(gameObject.name + " is eating");

        yield return new WaitForSeconds(5f);

        bowl.GetComponent<FoodBowl>().empty_bowl();
        print(gameObject.name + " finished eating");

        ChangeState(DogState.Idle);
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
    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Controller"))
        {
            print(gameObject.name +"collided with " + other.gameObject.name);
            ChangeState(DogState.WigglingTail);
        }
    }

    public void Bath()
    {
        gameObject.SetActive(false);
    }
    
}
