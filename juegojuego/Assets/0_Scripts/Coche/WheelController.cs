using System;
using System.Collections;
using UnityEngine;

public class WheelController : MonoBehaviour
{
    private Rigidbody carRigidbody;

    [Header("Suspension")]
    [SerializeField] private float springRestLength;
    [SerializeField] private float springMaxCompression;
    [SerializeField] private float springStiffness;
    [SerializeField] private float springDamper;

    private float lastFrameSpringLength;

    private Vector3 wheelVelocityLocalSpace;

    private float valorMovimientoFrontal;

    [Header("Wheel")]
    [SerializeField] private float steerAngle;
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

    public AudioClip InicioDerrape;
    public AudioClip BucleDerrape;
    public AudioClip FinDerrape;

    private IEnumerator sonidoCoroutine;


    void Start()
    {
        carRigidbody = transform.root.GetComponent<Rigidbody>();

        InicializarVariables();

        GestionarInputs();
    }
    void Update()
    {
        if (GameManager.Instance.GetGameState() == GameState.Conduciendo)
        {
            ActualizarGiroRueda();
            Debug.DrawRay(transform.position, -transform.up * (lastFrameSpringLength), Color.green);
        }
    }

    void FixedUpdate()
    {
        if (GameManager.Instance.GetGameState() == GameState.Conduciendo)
        {
            if (Physics.Raycast(transform.position, -transform.up, out RaycastHit hit, springRestLength))
            {
                WheelForceCalculations(hit);
            }
        }
    }

    private void Derrapar()
    {
        AplicarModificadoresDerrape();

        SonidoDerrape();

        HacerMarcasSuelo(true);

        HacerHumo(true);
    }

    private void DejarDerrapar()
    {
        InicializarVariables();

        SonidoFinDerrape();

        HacerMarcasSuelo(false);

        HacerHumo(false);
    }

    private void AplicarModificadoresDerrape()
    {
        maxForwardVelocityFactor = maxForwardVelocity * driftMaxForwardVelocityMultiplier;
        maxGripSidewaysVelocityFactor = maxGripSidewaysVelocity * driftMaxGripSidewaysMultiplier;
        motorForceFactor = motorForce * driftMotorForceMultiplier;
    }

    private void SonidoDerrape()
    {
        if (this.transform.tag == Constantes.Player.TAG_REAR_WHEELS)
        {
            AudioSource audioSourceRueda = GetComponentInChildren<AudioSource>();

            if (sonidoCoroutine != null)
            {
                StopCoroutine(sonidoCoroutine);
            }

            sonidoCoroutine = SonidoDerrapeCoroutine(audioSourceRueda);

            StartCoroutine(sonidoCoroutine);
        }
    }

    IEnumerator SonidoDerrapeCoroutine(AudioSource audioSourceRueda)
    {
        audioSourceRueda.clip = InicioDerrape;
        audioSourceRueda.Play();
        yield return new WaitForSeconds(audioSourceRueda.clip.length);
        audioSourceRueda.loop = true;
        audioSourceRueda.clip = BucleDerrape;
        audioSourceRueda.Play();
    }

    private void SonidoFinDerrape()
    {
        if (this.transform.tag == Constantes.Player.TAG_REAR_WHEELS)
        {
            StopCoroutine(sonidoCoroutine);

            AudioSource audioSourceRueda = GetComponentInChildren<AudioSource>();
            audioSourceRueda.clip = FinDerrape;
            audioSourceRueda.loop = false;
            audioSourceRueda.Play();
        }
    }

    private void HacerMarcasSuelo(bool hacerMarcas)
    {
        if (this.transform.tag == Constantes.Player.TAG_REAR_WHEELS)
        {
            if (hacerMarcas)
            {
                if (Physics.Raycast(transform.position, -transform.up, springRestLength))
                {
                    GetComponentInChildren<TrailRenderer>().emitting = hacerMarcas;
                }
            }
            else
            {
                GetComponentInChildren<TrailRenderer>().emitting = hacerMarcas;
            }
        }
    }

    private void HacerHumo(bool hacerHumo)
    {
        if (this.transform.tag == Constantes.Player.TAG_REAR_WHEELS)
        {
            if(hacerHumo)
            {
                GetComponentInChildren<ParticleSystem>().Play();
            }
            else
            {
                GetComponentInChildren<ParticleSystem>().Stop();
            }
        }
    }

    private void ResetValorMovimientoFrontal()
    {
        valorMovimientoFrontal = 0.0f;
    }

    private void RecogerValorMovimientoFrontal(float valorYMovimientoFrontal)
    {
        valorMovimientoFrontal = valorYMovimientoFrontal;
    }

    private void WheelForceCalculations(RaycastHit hit)
    {
        float fuerzaFrontalRueda;
        float fuerzaLateralRueda;

        wheelVelocityLocalSpace = HitPointVelocityWheelLocaLSpace(hit.point);

        hitGroundAngle = GroundInclinationWorldSpace(hit.normal);

        fuerzaFrontalRueda = MotorAccelerationForce();

        fuerzaFrontalRueda = AutoBreak(fuerzaFrontalRueda);

        Vector3 suspensionForce = SuspensionForce(hit.distance);
        fuerzaLateralRueda = GripForce(suspensionForce.magnitude);

        AplicarFuerzas(suspensionForce, fuerzaFrontalRueda, fuerzaLateralRueda);

        Debug.DrawRay(transform.position, (suspensionForce + (fuerzaFrontalRueda * transform.forward) + (fuerzaLateralRueda * -transform.right)) / forceVectorLength, Color.red);
        Debug.DrawRay(transform.position, suspensionForce / forceVectorLength, Color.yellow);
    }

    private void AplicarFuerzas(Vector3 _suspensionForce, float _fuerzaFrontal, float _fuerzaLateral)
    {
        carRigidbody.AddForceAtPosition(_suspensionForce +
                                       (_fuerzaFrontal * transform.forward) +
                                       (_fuerzaLateral * -transform.right),
                                       transform.position);
    }

    private float GripForce(float suspensionForceMagnitude)
    {
        return wheelVelocityLocalSpace.x *
               sidewaysGripCurve.Evaluate(wheelVelocityLocalSpace.x / maxGripSidewaysVelocityFactor) *
               suspensionForceMagnitude;
    }

    private float MotorAccelerationForce()
    {
        float fuerza;

        if ((valorMovimientoFrontal > 0 && wheelVelocityLocalSpace.z > 0) || (valorMovimientoFrontal < 0 && wheelVelocityLocalSpace.z < 0))
        {
            fuerza = valorMovimientoFrontal * torqueCurve.Evaluate(wheelVelocityLocalSpace.z / maxForwardVelocityFactor) * inclinationToGripCurve.Evaluate(hitGroundAngle / maxGroundAngleToGrip) * motorForceFactor;
        }
        else
        {
            fuerza = valorMovimientoFrontal * inclinationToGripCurve.Evaluate(hitGroundAngle / maxGroundAngleToGrip) * breakForce;
        }

        return fuerza;
    }

    private float AutoBreak(float fuerza)
    {
        if (valorMovimientoFrontal == 0 && wheelVelocityLocalSpace.z > 0 && wheelVelocityLocalSpace.magnitude < autoBreakMaxVelocity)
        {
            return -autoBreakForce;
        }
        else if (valorMovimientoFrontal == 0 && wheelVelocityLocalSpace.z < 0 && wheelVelocityLocalSpace.magnitude < autoBreakMaxVelocity)
        {
            return autoBreakForce;
        }

        return fuerza;
    }

    private float GroundInclinationWorldSpace(Vector3 hitNormal)
    {
        return Vector3.Angle(Vector3.up, hitNormal);
    }

    private Vector3 HitPointVelocityWheelLocaLSpace(Vector3 hitPoint)
    {
        return transform.InverseTransformDirection(carRigidbody.GetPointVelocity(hitPoint));
    }

    private Vector3 SuspensionForce(float distanceToFloor)
    {
        float springMinLength = springRestLength - springMaxCompression;

        // Te delimita el valor si se sale de los limites
        float currentSpringLength = Mathf.Clamp(distanceToFloor, springMinLength, springRestLength);
        float springVelocity = (lastFrameSpringLength - currentSpringLength) / Time.fixedDeltaTime;
        float springForce = springStiffness * stiffnessCurve.Evaluate((springRestLength - currentSpringLength) / springRestLength);
        float damperForce = springDamper * springVelocity;

        Vector3 fuerzaSuspension = (springForce + damperForce) * transform.up;

        // comprobar que la fuerza de suspensión siempre es hacia arriba
        if ((springForce + damperForce) < 0)
        {
            fuerzaSuspension = new Vector3(0, 0, 0);
        }

        // Guardar el valor de la springLength para el siguiente frame
        lastFrameSpringLength = currentSpringLength;

        return fuerzaSuspension;
    }

    private void ActualizarGiroRueda()
    {
        wheelAngle = Mathf.Lerp(wheelAngle, steerAngle, steerTime * Time.deltaTime);
        transform.localRotation = Quaternion.Euler(Vector3.up * wheelAngle);
    }

    public float GetSteerAngle()
    {
        return steerAngle;
    }

    public void SetSteerAngle(float newSteerAngle)
    {
        steerAngle = newSteerAngle;
    }

    private void InicializarVariables()
    {
        maxForwardVelocityFactor = maxForwardVelocity;
        maxGripSidewaysVelocityFactor = maxGripSidewaysVelocity;
        motorForceFactor = motorForce;
    }

    private void GestionarInputs()
    {
        InputManager.Instance.controles.Conduciendo.MovimientoFrontal.performed += contexto => RecogerValorMovimientoFrontal(contexto.ReadValue<Vector2>().y);
        InputManager.Instance.controles.Conduciendo.MovimientoFrontal.canceled += contexto => ResetValorMovimientoFrontal();
        InputManager.Instance.controles.Conduciendo.Derrape.performed += contexto => Derrapar();
        InputManager.Instance.controles.Conduciendo.Derrape.canceled += contexto => DejarDerrapar();
    }
}
