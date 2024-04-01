using System;
using UnityEngine;

public class CarTurnAround : MonoBehaviour
{
    private Rigidbody carRigidbody;
    private float turnAroundValue;

    [SerializeField] private float maxTurnAroundVelocity;
    [SerializeField] private float minTurnAroundAngle;
    [SerializeField] private float turnAroundForce;
    [SerializeField] private AnimationCurve forceAccordingToInclination;

    private void ResetTurnAroundValue()
    {
        turnAroundValue = 0.0f;
    }

    private void SetTurnAroundValue(float ValorXMovimientoLateral)
    {
        turnAroundValue = ValorXMovimientoLateral;
    }

    void Start()
    {
        GestionarInputs();

        InicializarVariables();
    }

    private void OnTriggerStay(Collider other)
    {
        if (carRigidbody.velocity.magnitude < maxTurnAroundVelocity && Vector3.Angle(Vector3.up, transform.up) > minTurnAroundAngle)
        {
            float inclinationForceFactor = forceAccordingToInclination.Evaluate(Vector3.Angle(Vector3.up, -transform.up) / 180);
            carRigidbody.AddForceAtPosition(turnAroundValue * transform.right * turnAroundForce * inclinationForceFactor , transform.position);
        }
    }

    private void GestionarInputs()
    {
        InputManager.Instance.controles.Conduciendo.MovimientoLateral.performed += contexto => SetTurnAroundValue(contexto.ReadValue<Vector2>().x);
        InputManager.Instance.controles.Conduciendo.MovimientoLateral.canceled += contexto => ResetTurnAroundValue();
    }
    private void InicializarVariables()
    {
        carRigidbody = transform.root.GetComponent<Rigidbody>();
    }
}
