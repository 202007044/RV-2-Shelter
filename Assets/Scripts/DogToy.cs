using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class DogToy : XRGrabInteractable
{
    private GameObject[] dogs;

    protected override void Awake()
    {
        base.Awake();
        dogs = GameObject.FindGameObjectsWithTag("Dog");
    }

    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        base.OnSelectEntered(args);
    }
    private void Update()
    {
    }

    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        base.OnSelectExited(args);
        PrintClosestDog();
    }

    private void PrintClosestDog()
    {
        if (dogs.Length == 0)
        {
            //debug.Log("No dogs found.");
            return;
        }

        GameObject closestDog = null;
        float closestDistance = float.MaxValue;

        foreach (GameObject dog in dogs)
        {
            float distance = Vector3.Distance(transform.position, dog.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestDog = dog;
            }
        }

        if (closestDog != null)
        {
            closestDog.GetComponent<DogController>().CatchToy(gameObject);
        }
    }
}