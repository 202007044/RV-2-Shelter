using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : MonoBehaviour
{
    public bool isOn = false;
    public bool useCar = true;
    public GameObject player;
    public List<Transform> points;
    public float maxSpeed = 10f;
    public float acceleration = 0.1f;
    public float deceleration = 0.1f;
    private float currentSpeed = 0f;
    private int currentPointIndex = 0;
    
    private bool isStarted = false;

    
    private bool isEnded = false;

    // Start is called before the first frame update
    void Start()
    {
        if (points == null || points.Count == 0)
        {
            Debug.LogError("No points assigned for the car to follow.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(!isStarted && useCar)
        {
            player.transform.position = new Vector3(transform.position.x, transform.position.y - 3, transform.position.z + 1.7f);
        }
        if (isOn && points != null && points.Count > 0)
        {
            isStarted = true;
            MoveAlongPoints();
            player.transform.position = new Vector3(transform.position.x, transform.position.y - 3, transform.position.z + 1.7f);
            if (currentSpeed == 0 && isEnded)
            {
                player.transform.position = new Vector3(player.transform.position.x,0, player.transform.position.z+4.0f);
                isOn = false;
            }
        }
        //if current speed is 0, move the player to the left of the car
        
    }

    private void MoveAlongPoints()
    {
        Transform targetPoint = points[currentPointIndex];
        Vector3 direction = (targetPoint.position - transform.position).normalized;
        transform.position = Vector3.MoveTowards(transform.position, targetPoint.position, currentSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, targetPoint.position) < 0.1f)
        {
            currentPointIndex = (currentPointIndex + 1) % points.Count;
        }

        // Calculate the distance to the next point
        float distanceToNextPoint = Vector3.Distance(transform.position, targetPoint.position);
        float totalDistance = Vector3.Distance(points[0].position, points[points.Count - 1].position);

        // Decrease speed near the end
        if (currentPointIndex >= 2)
        {
            isEnded = true;
            currentSpeed = Mathf.Max(0, currentSpeed - deceleration * Time.deltaTime);
        }
        else
        {
            // Accelerate when far from the end
            currentSpeed = Mathf.Min(maxSpeed, currentSpeed + acceleration * Time.deltaTime);
        }
    }
}