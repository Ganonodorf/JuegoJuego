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
    [SerializeField] private float restLength;
    [SerializeField] private float springTravel;
    [SerializeField] private float springStiffness;
    [SerializeField] private float damperStiffness;

    private float springLength;

    private float fx;
    private float fy;

    [Header("Wheel")]
    public float steerAngle;
    [SerializeField] private float steerTime;
    [SerializeField] private float wheelRadius;
    [SerializeField] private float maxGripSidewaysVelocity;
    private float maxGripSidewaysVelocityFactor;
    [SerializeField] private float maxGroundAngleToGrip;
    private float hitGroundAngle;


    [Header("Motor")]
    [SerializeField] private float maxForwardVelocity;
    private float maxForwardVelocityFactor;
    [SerializeField] private float motorForce;
    private float motorForceFactor;
    [SerializeField] private float breakForce;
    [SerializeField] private float forceVectorLength;

    [Header("Automatic Break")]
    [SerializeField] private float autoBreakMaxVelocity;
    [SerializeField] private float autoBreakForce;

    [Header("Drifting Multipliers")]
    [SerializeField] private float driftMaxGripSidewaysMultiplier;
    [SerializeField] private float driftMaxForwardVelocityMultiplier;
    [SerializeField] private float driftMotorForceMultiplier;







    private float wheelAngle;

    [Header("Curvas")]
    public AnimationCurve sidewaysGripCurve;
    public AnimationCurve torqueCurve;
    [SerializeField] private AnimationCurve stiffnessCurve;
    [SerializeField] private AnimationCurve inclinationToGripCurve;

    // Start is called before the first frame update
    void Start()
    {
        rb = transform.root.GetComponent<Rigidbody>();

        

    }

    // Update is called once per frame
    void Update()
    {
        if(GameManager.Instance.GetGameState() == GameState.Conduciendo)
        {
            wheelAngle = Mathf.Lerp(wheelAngle, steerAngle, steerTime * Time.deltaTime);
            transform.localRotation = Quaternion.Euler(Vector3.up * wheelAngle);

            Debug.DrawRay(transform.position, -transform.up * (springLength), Color.green);
        }
    }

    void FixedUpdate()
    {
        if (GameManager.Instance.GetGameState() == GameState.Conduciendo)
        {
            if (Physics.Raycast(transform.position, -transform.up, out RaycastHit hit, restLength))
            {
                Vector3 suspensionForce = SuspensionForce(hit.distance);

                
                // Get the velocity (Local Space) of the hitpoint of the wheels
                Vector3 wheelVelocityLS = transform.InverseTransformDirection(rb.GetPointVelocity(hit.point));

                // if drifting button
                if (Input.GetKey("space"))
                {
                    maxForwardVelocityFactor = maxForwardVelocity * driftMaxForwardVelocityMultiplier;
                    maxGripSidewaysVelocityFactor = maxGripSidewaysVelocity * driftMaxGripSidewaysMultiplier;
                    motorForceFactor = motorForce * driftMotorForceMultiplier;
                }
                else
                {
                    maxForwardVelocityFactor = maxForwardVelocity;
                    maxGripSidewaysVelocityFactor = maxGripSidewaysVelocity;
                    motorForceFactor = motorForce;
                }

                // calculate inclination angle of the floor hit
                hitGroundAngle = Vector3.Angle(Vector3.up, hit.normal);

                // motor acceleration force
                if ((Input.GetAxis("Vertical") > 0 && wheelVelocityLS.z > 0) || (Input.GetAxis("Vertical") < 0 && wheelVelocityLS.z < 0))
                {
                    fx = Input.GetAxis("Vertical") * torqueCurve.Evaluate(wheelVelocityLS.z / maxForwardVelocityFactor) * inclinationToGripCurve.Evaluate(hitGroundAngle / maxGroundAngleToGrip) * motorForceFactor;
                }
                else
                {
                    fx = Input.GetAxis("Vertical") * inclinationToGripCurve.Evaluate(hitGroundAngle / maxGroundAngleToGrip) * breakForce;
                }

                //auto break force if low velocity and no input
                if ((Input.GetAxis("Vertical") == 0) &&
                    wheelVelocityLS.z > 0 && wheelVelocityLS.magnitude < autoBreakMaxVelocity)
                {
                    fx = -autoBreakForce;
                }
                else if ((Input.GetAxis("Vertical") == 0) &&
                    wheelVelocityLS.z < 0 && wheelVelocityLS.magnitude < autoBreakMaxVelocity)
                {
                    fx = autoBreakForce;
                }

                // Sideways grip
                fy = wheelVelocityLS.x * sidewaysGripCurve.Evaluate(wheelVelocityLS.x / maxGripSidewaysVelocityFactor) * suspensionForce.magnitude;

                rb.AddForceAtPosition(suspensionForce + (fx * transform.forward) + (fy * -transform.right), transform.position);

                Debug.DrawRay(transform.position, (suspensionForce + (fx * transform.forward) + (fy * -transform.right)) / forceVectorLength, Color.red);
                Debug.DrawRay(transform.position, suspensionForce / forceVectorLength, Color.yellow);
            }
        }
    }


    private Vector3 SuspensionForce(float distanciaHit)
    {
        float minLength = restLength - springTravel;

        float lastLength = springLength;
        springLength = distanciaHit;
        springLength = Mathf.Clamp(springLength, minLength, restLength);
        float springVelocity = (lastLength - springLength) / Time.fixedDeltaTime;
        float springForce = springStiffness * stiffnessCurve.Evaluate((restLength - springLength) / restLength);
        float damperForce = damperStiffness * springVelocity;

        Vector3 fuerzaSuspension = (springForce + damperForce) * transform.up;

        // comprobar que la fuerza de suspensión siempre es hacia arriba
        if ((springForce + damperForce) < 0)
        {
            fuerzaSuspension = new Vector3(0, 0, 0);
        }

        return fuerzaSuspension;
    }
}
