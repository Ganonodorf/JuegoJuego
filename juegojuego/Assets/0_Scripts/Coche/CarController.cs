using UnityEngine;

public class CarController : MonoBehaviour
{
    [SerializeField]
    private GameObject RuedaDerechaGO;
    [SerializeField]
    private GameObject RuedaIzquierdaGO;

    [Header("Car Specs")]
    // Distancia entre los ejes de las ruedas
    public float wheelBase;

    // Distancia entre las ruedas de atras
    public float rearTrack;

    // El radio de la circunferencía mínima que necesita un coche para
    // dar una vuelta de 180º
    public float turnRadius;

    public float ackermannAngleLeft;
    public float ackermannAngleRight;

    private void Start()
    {
        GestionarInputs();
    }

    private void GestionarInputs()
    {
        InputManager.Instance.controles.Conduciendo.MovimientoLateral.performed += contexto => GirarRuedas(contexto.ReadValue<Vector2>().x);
        InputManager.Instance.controles.Conduciendo.MovimientoLateral.canceled += contexto => CentrarRuedas();
    }

    private void GirarRuedas(float valorMovimientoHorizontal)
    {
        if (valorMovimientoHorizontal > 0)
        {
            ackermannAngleLeft = ExteriorWheelAckermann(valorMovimientoHorizontal);
            ackermannAngleRight = InteriorWheelAckermann(valorMovimientoHorizontal);
        }
        else
        {
            ackermannAngleLeft = InteriorWheelAckermann(valorMovimientoHorizontal);
            ackermannAngleRight = ExteriorWheelAckermann(valorMovimientoHorizontal);
        }

        AplicarValoresDeGiro();

    }

    private void CentrarRuedas()
    {
        ackermannAngleLeft = 0.0f;
        ackermannAngleRight = 0.0f;

        AplicarValoresDeGiro();
    }

    private void AplicarValoresDeGiro()
    {
        if(RuedaDerechaGO.TryGetComponent(out WheelController rightWheelController))
        {
            rightWheelController.SetSteerAngle(ackermannAngleRight);
        }

        if (RuedaIzquierdaGO.TryGetComponent(out WheelController leftWheelController))
        {
            leftWheelController.SetSteerAngle(ackermannAngleLeft);
        }
    }

    private float ExteriorWheelAckermann(float steerInput)
    {
        float ackermannValue;

        ackermannValue = Mathf.Rad2Deg* Mathf.Atan(wheelBase / (turnRadius + (rearTrack / 2))) * steerInput;

        return ackermannValue;
    }

    private float InteriorWheelAckermann(float steerInput)
    {
        float ackermannValue;

        ackermannValue = Mathf.Rad2Deg * Mathf.Atan(wheelBase / (turnRadius - (rearTrack / 2))) * steerInput;

        return ackermannValue;
    }

    
}
