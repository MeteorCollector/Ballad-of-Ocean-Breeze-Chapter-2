using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flashlight : MonoBehaviour
{
    public float lifespan = 0.1f;
    public float initialIntensity = 10f;
    private float number;
    private Light lgt;
    void Start()
    {
        lgt = this.gameObject.GetComponent<Light>();
        lgt.intensity = initialIntensity;
    }

    void Update()
    {
        if (number > lifespan) Destroy(this.gameObject);
        number += Time.deltaTime;
        lgt.intensity = (21 * initialIntensity * lifespan) / (40 * (number + (lifespan / 20))); 
    }
}
