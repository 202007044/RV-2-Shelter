using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class add_food : MonoBehaviour

{
    [SerializeField] UnityEvent onTriggerEnter;
    [SerializeField] UnityEvent onTriggerExit;

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("racao")) {
            print("Collide with racao");
            fill_bowl();
        }
    }

    public void fill_bowl() {
        gameObject.transform.GetChild(0).gameObject.SetActive(true);
    }

}
