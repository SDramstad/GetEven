using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pulsing_Light : MonoBehaviour {

    private Light thisLight;
    private Material lightBulb;

    public float maxIntensity = 1f;
    public float minIntensity = 0f;

    public float pulseSpeed = 0.25f;
    private float targetIntensity = 1f;
    private float currentIntensity;



	// Use this for initialization
	void Start () {
        thisLight = GetComponent<Light>();
        lightBulb = GetComponent<Renderer>().material;
	}
	
	// Update is called once per frame
	void Update () {
        currentIntensity = Mathf.MoveTowards(thisLight.intensity, targetIntensity, Time.deltaTime * pulseSpeed);
        if (currentIntensity >= maxIntensity)
        {
            currentIntensity = maxIntensity;
            lightBulb.SetColor("_EmissionColor", Color.clear);
            targetIntensity = minIntensity;
        }
        else if (currentIntensity <= minIntensity)
        {
            currentIntensity = minIntensity;
            lightBulb.SetColor("_EmissionColor", Color.red);
            targetIntensity = maxIntensity;
        }
        thisLight.intensity = currentIntensity;
    }
}
