using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelController : MonoBehaviour
{
    private Rigidbody rb;

    public bool wheelFrontLeft;
    public bool wheelFrontRight;
    public bool wheelRearLeft;
    public bool wheelRearRight;


    [Header("Suspension")]
    public float restLength;
    public float springTravel;
    public float springStiffness;
    public float damperStiffness;

    private float minLength;
    private float maxLength;
    private float lastLength;
    private float springLength;
    private float springVelocity;
    private float springForce;
    private float damperForce;

    private Vector3 suspensionForce;
    private Vector3 wheelVelocityLS; // Local Space
    private float fx;
    private float fy;

    [Header("Wheel")]
    public float steerAngle;
    public float steerTime;
    public float wheelRadius;
    public float maxGripSidewaysVelocity;

    [Header("Motor")]
    public float maxForwardVelocity;
    [SerializeField] private float motorForce;
    [SerializeField] private float breakForce;
    public float forceVectorLength;

    [Header("Automatic Break")]
    [SerializeField] private float autoBreakMaxVelocity;
    [SerializeField] private float autoBreakForce;




    private float wheelAngle;

    public AnimationCurve sidewaysGripCurve;
    public AnimationCurve torqueCurve;
    [SerializeField] private AnimationCurve stiffnessCurve;

    // Start is called before the first frame update
    void Start()
    {
        rb = transform.root.GetComponent<Rigidbody>();

        minLength = restLength - springTravel;

    }

    // Update is called once per frame
    void Update()
    {
        wheelAngle = Mathf.Lerp(wheelAngle, steerAngle, steerTime * Time.deltaTime);
        transform.localRotation = Quaternion.Euler(Vector3.up * wheelAngle);

        Debug.DrawRay(transform.position, -transform.up * (springLength + wheelRadius), Color.green);
    }

    void FixedUpdate()
    {
        if (Physics.Raycast(transform.position, -transform.up, out RaycastHit hit, restLength + wheelRadius))
        {
            lastLength = springLength;
            springLength = hit.distance - wheelRadius;
            springLength = Mathf.Clamp(springLength, minLength, restLength);
            springVelocity = (lastLength - springLength) / Time.fixedDeltaTime;
            springForce = springStiffness * stiffnessCurve.Evaluate((restLength - springLength)/restLength);
            damperForce = damperStiffness * springVelocity;

            suspensionForce = (springForce + damperForce) * transform.up;

            wheelVelocityLS = transform.InverseTransformDirection(rb.GetPointVelocity(hit.point));

            // motor acceleration force
            if ((Input.GetAxis("Vertical") > 0 && wheelVelocityLS.z > 0) || (Input.GetAxis("Vertical") < 0 && wheelVelocityLS.z < 0))
            {
                fx = Input.GetAxis("Vertical") * torqueCurve.Evaluate(wheelVelocityLS.z / maxForwardVelocity) * motorForce;
            }else
            {
                fx = Input.GetAxis("Vertical") * breakForce;
            }

            //auto break force if low velocity and no input
            if((Input.GetAxis("Vertical") == 0) &&
                wheelVelocityLS.z > 0 && wheelVelocityLS.magnitude < autoBreakMaxVelocity)
            {
                fx = -autoBreakForce;
            } else if ((Input.GetAxis("Vertical") == 0) &&
                wheelVelocityLS.z < 0 && wheelVelocityLS.magnitude < autoBreakMaxVelocity)
            {
                fx = autoBreakForce;
            }

                // Sideways grip
                fy = wheelVelocityLS.x * sidewaysGripCurve.Evaluate(wheelVelocityLS.x / maxGripSidewaysVelocity) * springForce;

            rb.AddForceAtPosition(suspensionForce + (fx * transform.forward) + (fy * -transform.right), transform.position);

            Debug.DrawRay(transform.position, (suspensionForce + (fx * transform.forward) + (fy * -transform.right)) / forceVectorLength, Color.red);
            Debug.DrawRay(transform.position, suspensionForce / forceVectorLength, Color.yellow);

        }
    }
}
