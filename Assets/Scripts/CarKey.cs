using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class CarKey : XRSimpleInteractable
{
    public Vector3 startRotation;
    public Car car;

    public Animator animator;


    protected override void Awake()
    {
        base.Awake();    
    }

    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        base.OnSelectEntered(args);
        // animation trigger to rotate the key
        animator.SetTrigger("rotate");
        StartCoroutine(TurnCarOnAfterDelay(0.6f));
    }

    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        base.OnSelectExited(args);
        // Additional logic when the key is released
    }

    private IEnumerator TurnCarOnAfterDelay(float delay)
    {
        car.TurnOnCar();
        yield return new WaitForSeconds(delay);
        car.isOn = true;

    }
}