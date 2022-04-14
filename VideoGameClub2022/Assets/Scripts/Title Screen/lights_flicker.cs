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
        timeDelay = Random.Range(0.1f, 0.3f);
        yield return new WaitForSeconds(timeDelay);

        if (timeDelay >= 0.25f)
        {
            yield return new WaitForSeconds(0.6f);
        }

        this.gameObject.GetComponent<Light>().intensity = Random.Range(50f, 300f);
        this.gameObject.GetComponent<Light>().enabled = true;
        timeDelay = Random.Range(0.1f, 0.3f);
        yield return new WaitForSeconds(timeDelay);

        if (timeDelay >= 0.25f)
        {
            yield return new WaitForSeconds(0.6f);
        }
        
        isFlickering = false;
    }
}
