using PixelCrushers.DialogueSystem;
using UnityEngine;

public class CarController : MonoBehaviour
{
    private GameObject RuedaDerechaGO;
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

    // GRAVITY increase
    private Rigidbody coche;
    [SerializeField]
    private float multiplicadorGravedad = 1.0f;

    private void Start()
    {
        GestionarInputs();
        coche = this.GetComponent<Rigidbody>();

        GameManager.OnGameStateChanged += GameManager_OnGameStateChanged;

        RuedaDerechaGO = GameObject.FindGameObjectWithTag("WheelFrontRight");
        RuedaIzquierdaGO = GameObject.FindGameObjectWithTag("WheelFrontLeft");
    }

    private void OnDestroy()
    {
        GameManager.OnGameStateChanged -= GameManager_OnGameStateChanged;

        DesgestionarInputs();
    }

    private void GameManager_OnGameStateChanged(GameState nuevoEstadoJuego, GameState viejoEsadoJuego)
    {
        if(this.gameObject != null)
        {
            if (nuevoEstadoJuego == GameState.Conduciendo)
            {
                // Para que no pueda interactuar con cosas del Dialogue System
                if (TryGetComponent(out ProximitySelector proximitySelector))
                {
                    proximitySelector.enabled = true;
                }
            }
            else
            {
                // Para que no pueda interactuar con cosas del Dialogue System
                if (TryGetComponent(out ProximitySelector proximitySelector))
                {
                    proximitySelector.enabled = false;
                }
            }
        }
    }

    private void FixedUpdate()
    {
        // Aumento gravedad
        coche.AddForce(9.81f * multiplicadorGravedad * -Vector3.up, ForceMode.Acceleration);
    }

    private void GestionarInputs()
    {
        InputManager.Instance.controles.Conduciendo.MovimientoLateral.performed += contexto => GirarRuedas(contexto.ReadValue<Vector2>().x);
        InputManager.Instance.controles.Conduciendo.MovimientoLateral.canceled += contexto => CentrarRuedas();

        InputManager.Instance.controles.Dialogo.MovimientoLateral.performed += contexto => GirarRuedas(contexto.ReadValue<Vector2>().x);
        InputManager.Instance.controles.Dialogo.MovimientoLateral.canceled += contexto => CentrarRuedas();
    }

    private void DesgestionarInputs()
    {
        InputManager.Instance.controles.Conduciendo.MovimientoLateral.performed -= contexto => GirarRuedas(contexto.ReadValue<Vector2>().x);
        InputManager.Instance.controles.Conduciendo.MovimientoLateral.canceled -= contexto => CentrarRuedas();

        InputManager.Instance.controles.Dialogo.MovimientoLateral.performed -= contexto => GirarRuedas(contexto.ReadValue<Vector2>().x);
        InputManager.Instance.controles.Dialogo.MovimientoLateral.canceled -= contexto => CentrarRuedas();
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
        if(RuedaDerechaGO != null)
        {
            if (RuedaDerechaGO.TryGetComponent(out WheelController rightWheelController))
            {
                rightWheelController.SetSteerAngle(ackermannAngleRight);
            }
        }

        if(RuedaIzquierdaGO != null)
        {
            if (RuedaIzquierdaGO.TryGetComponent(out WheelController leftWheelController))
            {
                leftWheelController.SetSteerAngle(ackermannAngleLeft);
            }
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
