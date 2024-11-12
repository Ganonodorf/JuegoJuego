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

    [SerializeField] private float animSpeedFactor;


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
            float lightAnimSpeed = 1 + lightPower * animSpeedFactor;
            SetAnimSpeed(lightAnimSpeed);

            transform.rotation = Quaternion.LookRotation(DirectionToTarget(target));
        } else
        {
            SetLightIntensity(minIntensity);
            SetLightRange(minRange);
            SetAnimSpeed(1);

            transform.rotation = Quaternion.LookRotation(Vector3.up);
        }
    }

    private float DistanceToTarget (GameObject target)
    {
        return Vector3.Distance(transform.position, target.transform.position);
         
    }

    private Vector3 DirectionToTarget (GameObject target)
    {
        return Vector3.Normalize(target.transform.position - transform.position);
    }

    private void SetLightIntensity (float lightIntensityValue)
    {
        GetComponent<Light>().intensity = lightIntensityValue;
    }

    private void SetLightRange (float lightRangeValue)
    {
        GetComponent<Light>().range = lightRangeValue;
    }

    private void SetAnimSpeed (float lightAnimSpeedValue)
    {
        GetComponent<Animator>().speed = lightAnimSpeedValue;
    }


}
