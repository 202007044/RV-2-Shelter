using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BathTub : MonoBehaviour
{
    public GameObject pug;
    public GameObject german;
    public GameObject cur;
    public GameObject corgi;
    public GameObject chihuahua;

    public GameObject water;

    public GameObject bathingDog;

    public ParticleSystem bubbles;

    private bool isBathing = false;

   
    // Start is called before the first frame update
    void Start()
    {
        pug.SetActive(false);
        german.SetActive(false);
        cur.SetActive(false);
        corgi.SetActive(false);
        chihuahua.SetActive(false);   
        water.SetActive(false);
        bubbles.Stop();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Dog" && isBathing == false)
        {
            water.SetActive(true);
            isBathing = true;
            bathingDog = other.gameObject;
            bathingDog.SetActive(false);
            if(other.gameObject.name == "pug")
            {
                pug.SetActive(true);
                cur.SetActive(false);
                german.SetActive(false);
                corgi.SetActive(false);
                chihuahua.SetActive(false);
            }
            else if(other.gameObject.name == "germanshepherd")
            {
                german.SetActive(true);
                cur.SetActive(false);
                pug.SetActive(false);
                corgi.SetActive(false);
                chihuahua.SetActive(false);
            }
            else if(other.gameObject.name == "corgi")
            {
                corgi.SetActive(true);
                cur.SetActive(false);
                german.SetActive(false);
                pug.SetActive(false);
                chihuahua.SetActive(false);
            }
            else if(other.gameObject.name == "chihuahua")
            {
                chihuahua.SetActive(true);
                cur.SetActive(false);
                german.SetActive(false);
                corgi.SetActive(false);
                pug.SetActive(false);
            }
            else if(other.gameObject.name == "cur")
            {
                cur.SetActive(true);
                pug.SetActive(false);
                german.SetActive(false);
                corgi.SetActive(false);
                chihuahua.SetActive(false);
            }
        }
        else if(other.gameObject.tag == "Soap" && isBathing == true)
        {
            bubbles.Play();
            StartCoroutine(StopBubbles());
        }

    }

    IEnumerator StopBubbles()
    {
        yield return new WaitForSeconds(5);
        bubbles.Stop();
        isBathing = false;
        bathingDog.transform.position = new Vector3(bathingDog.transform.position.x+10, bathingDog.transform.position.y, bathingDog.transform.position.z);
        bathingDog.SetActive(true);
        pug.SetActive(false);
        german.SetActive(false);
        cur.SetActive(false);
        corgi.SetActive(false);
        chihuahua.SetActive(false);   
        water.SetActive(false);
    }
}
