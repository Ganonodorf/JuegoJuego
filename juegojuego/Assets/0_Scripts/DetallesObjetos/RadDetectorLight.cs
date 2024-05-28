using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadDetectorLight : MonoBehaviour
{
    [SerializeField] private GameObject target;

    [SerializeField] private float minRange;
    [SerializeField] private float maxRange;

    [SerializeField] private float minIntensity;
    [SerializeField] private float maxIntensity;

    [SerializeField] private float maxDistanceToTarget;

    [SerializeField] AnimationCurve lightPowerCurve;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(DistanceToTarget(target) < maxDistanceToTarget)
        {
            float lightPower = lightPowerCurve.Evaluate(DistanceToTarget(target) / maxDistanceToTarget);
            SetLightIntensity(minIntensity + lightPower * maxIntensity);
            SetLightRange(minRange + lightPower * maxRange);
        } else
        {
            SetLightIntensity(minIntensity);
            SetLightRange(minRange);
        }
    }

    private float DistanceToTarget (GameObject target)
    {
        return Vector3.Distance(transform.position, target.transform.position);
         
    }

    private void SetLightIntensity (float lightIntensityValue)
    {
        GetComponent<Light>().intensity = lightIntensityValue;
    }

    private void SetLightRange (float lightRangeValue)
    {
        GetComponent<Light>().range = lightRangeValue;
    }


}
