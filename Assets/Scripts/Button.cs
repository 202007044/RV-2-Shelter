using System;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class GrabbableButton : XRGrabInteractable
{
    public Car car;

    // buttons can be start, settings or exit
    public string buttonType;



    private Vector3 initialPosition;
    

    protected override void Awake()
    {
        base.Awake();
        initialPosition = transform.localPosition;
    }

    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        base.OnSelectEntered(args);
        if(buttonType == "start")
        {
            car.isOn = true;
        }
        else if(buttonType == "settings")
        {
            Debug.Log("Settings button pressed");
        }
        else if(buttonType == "exit")
        {
            Debug.Log("Exit button pressed");
            //quit the game
            Application.Quit();
            //UnityEditor.EditorApplication.isPlaying = false;

        }
        
        Destroy(gameObject);
    }

    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        base.OnSelectExited(args);
    }
}