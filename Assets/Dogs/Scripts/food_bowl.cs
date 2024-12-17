using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FoodBowl : MonoBehaviour
{
    [SerializeField] UnityEvent onTriggerEnter;
    [SerializeField] UnityEvent onTriggerExit;
    public GameObject dog;

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("racao"))
        {
            print("Collide with racao");
            fill_bowl();
        }
    }

    public void fill_bowl()
    {
        gameObject.transform.GetChild(0).gameObject.SetActive(true);

        dog.GetComponent<DogController>().GoEat(gameObject);
    }


    public void empty_bowl()
    {
        gameObject.transform.GetChild(0).gameObject.SetActive(false);

    }
    
}
