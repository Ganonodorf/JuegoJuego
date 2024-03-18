using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarTurnAround : MonoBehaviour
{
    private Rigidbody rb;

    [SerializeField] private float maxTurnAroundVelocity;
    [SerializeField] private float minTurnAroundAngle;
    [SerializeField] private float turnAroundForce;

    [SerializeField] private AnimationCurve forceAccordingToInclination;



    // Start is called before the first frame update
    void Start()
    {
        rb = transform.root.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay(Collider other)
    {
        if (rb.velocity.magnitude < maxTurnAroundVelocity && Vector3.Angle(Vector3.up, transform.up) > minTurnAroundAngle)
        {
            float inclinationForceFactor = forceAccordingToInclination.Evaluate(Vector3.Angle(Vector3.up, -transform.up) / 180);
            rb.AddForceAtPosition(Input.GetAxis("Horizontal") * transform.right * turnAroundForce * inclinationForceFactor , transform.position);
        }
    }
}
