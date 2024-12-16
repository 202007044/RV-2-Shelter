using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;


public class usable : MonoBehaviour
{


    public LayerMask layerMask;  // Layer mask to filter raycast targets
    public Transform source;     // Transform to cast ray from
    public float distance = 2.0f; // Max raycast distance

    private bool rayActivate = false; // Determines if raycasting is active

    // Start is called before the first frame update
    void Start()
    {
        XRGrabInteractable grabInteractable = GetComponent<XRGrabInteractable>();


        // Optional: Bind rayActivate to the grab state
        if (grabInteractable != null)
        {
            grabInteractable.selectEntered.AddListener(OnGrab);  // Activate ray when grabbed
            grabInteractable.selectExited.AddListener(OnRelease); // Deactivate ray when released
        }
        
    }

    void OnGrab(SelectEnterEventArgs args)
    {
        rayActivate = true; // Enable raycasting when grabbed
    }

    void OnRelease(SelectExitEventArgs args)
    {
        rayActivate = false; // Disable raycasting when released
    }

    // Update is called once per frame
    void Update()
    {
        if (rayActivate) // Perform raycast only when activated
        {
            RaycastCheck();
        }
    }

    void RaycastCheck()
    {
        RaycastHit hit;
        // Use Physics.Raycast with correct syntax
        bool hasHit = Physics.Raycast(source.position, source.forward, out hit, distance, layerMask);
        if (hasHit)
        {
            Debug.Log($"Hit object: {hit.transform.name}");
            // SendMessage to the hit object
            hit.transform.gameObject.SendMessage("fill_bowl", SendMessageOptions.DontRequireReceiver);
        }
    }

}
