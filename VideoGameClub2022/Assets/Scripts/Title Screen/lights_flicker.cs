using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lights_flicker : MonoBehaviour
{
    public bool isFlickering = false;
    public float timeDelay = 0;
    void Update()
    {

        if (!isFlickering)
        {
            StartCoroutine(flicker());
        } 
    }

    IEnumerator flicker()
    {
        isFlickering = true;
        this.gameObject.GetComponent<Light>().enabled = false;
        timeDelay = Random.Range(0.05f, 0.3f);
        yield return new WaitForSeconds(timeDelay);
        this.gameObject.GetComponent<Light>().enabled = true;
        timeDelay = Random.Range(0.05f, 0.3f);
        yield return new WaitForSeconds(timeDelay);
        isFlickering = false;
    }
}
