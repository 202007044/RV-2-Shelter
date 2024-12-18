using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
public class WidgetButton : XRSimpleInteractable
{
    public bool isPositive = true;

    // list of dogs active in the scene
    public List<GameObject> dogs;

    public GameObject pug;

    public GameObject german;

    public GameObject cur;

    public GameObject corgi;

    public GameObject chihuahua;

    // list of not active dogs

    public List<GameObject> notActiveDogs;

    

    public TextMeshPro text;

    

    protected override void Awake()
    {
        base.Awake();
        dogs.Add(pug);
        dogs.Add(german);
        dogs.Add(cur);
        dogs.Add(corgi);
        dogs.Add(chihuahua);
        text.text = GetActiveDogCount().ToString();
        //print("Dogs: " + dogs.Count);
    }

    protected int GetActiveDogCount()
    {
        int activeDogCount = 0;
        foreach (var dog in dogs)
        {
            if (dog.activeInHierarchy)
            {
                activeDogCount++;
            }
        }
        return activeDogCount;
    }

    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {

        base.OnSelectEntered(args);
        if (isPositive)
        {
            foreach (var dog in dogs)
            {
                if (!dog.activeInHierarchy)
                {
                    dog.SetActive(true);
                    text.text = GetActiveDogCount().ToString();
                    break;
                }
            }
        }
        else
        {
            foreach (var dog in dogs)
            {
                if (dog.activeInHierarchy)
                {
                    dog.SetActive(false);
                    text.text = GetActiveDogCount().ToString();
                    break;
                }
            }
        }




    }

    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        base.OnSelectExited(args);
    }
}
