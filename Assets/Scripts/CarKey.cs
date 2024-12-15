using System;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class CarKey : XRGrabInteractable
{
    public Vector3 startRotation;

    protected override void Awake()
    {
        base.Awake();
        startRotation = transform.rotation.eulerAngles;
    }

    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        base.OnSelectEntered(args);
        // Additional logic when the key is grabbed
    }

    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        base.OnSelectExited(args);
        // Additional logic when the key is released
    }

    void Update()
    {
        // Lock the rotation of the key in the x and y axis
        transform.rotation = Quaternion.Euler(startRotation.x, startRotation.y, transform.rotation.eulerAngles.z);
    }
}