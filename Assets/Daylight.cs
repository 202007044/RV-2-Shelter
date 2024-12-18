using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Daylight : MonoBehaviour
{
    public Light directionalLight;
    public Material dayMaterial;
    public Material nightMaterial;
    public Renderer environmentRenderer;
    private Boolean isNight = false;

    // Start is called before the first frame update
    void Start()
    {
        if (directionalLight == null)
        {
            directionalLight = GetComponent<Light>();
        }

    }


    void Update()
    {
        
    }

    public void turnNight()
    {
        directionalLight.intensity = 0.0f;
        RenderSettings.skybox = nightMaterial;
        isNight = true;
        //print("Night");
    }

    public void turnDay()
    {
        directionalLight.intensity = 2.0f;
        RenderSettings.skybox = dayMaterial;

        isNight = false;
        //print("Day");
    }

}