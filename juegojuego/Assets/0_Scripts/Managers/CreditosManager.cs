using Cinemachine;
using UnityEngine;
using UnityEngine.UI;

public class CreditosManager : MonoBehaviour
{
    [SerializeField] private GameObject creditosGO;
    [SerializeField] private GameObject exitButton;

    private GameObject camaraCreditos;

    private void Start()
    {
        camaraCreditos = GameObject.FindGameObjectWithTag("CreditosCamara");

        GameManager.OnGameStateChanged += GameManager_OnGameStateChanged;

        LinkearFunciones();

        GestionarInputs();
    }

    private void OnDestroy()
    {
        DesgestionarInputs();
    }

    private void GameManager_OnGameStateChanged(GameState nuevoEstadoJuego, GameState viejoEstadoJuego)
    {
        if (nuevoEstadoJuego == GameState.FinJuego)
        {
            AparecerMenu(true);

            camaraCreditos.GetComponent<CinemachineVirtualCamera>().enabled = true;
        }
    }

    private void LinkearFunciones()
    {
        exitButton.GetComponent<Button>().onClick.AddListener(BotonSalir);
    }

    public void BotonSalir()
    {
        Application.Quit();
    }

    private void AparecerMenu(bool estado)
    {
        creditosGO.SetActive(estado);

        if (estado) { exitButton.GetComponent<Button>().Select(); }
    }

    private void OptionallySelectFirst()
    {
        if(creditosGO && Cursor.visible)
        exitButton.GetComponent<Button>().Select();
    }

    private void GestionarInputs()
    {
        InputManager.Instance.controles.UI.MovimientoDer.performed += contexto => OptionallySelectFirst();
        InputManager.Instance.controles.UI.MovimientoIzq.performed += contexto => OptionallySelectFirst();
        InputManager.Instance.controles.UI.Arriba.performed += contexto => OptionallySelectFirst();
        InputManager.Instance.controles.UI.Abajo.performed += contexto => OptionallySelectFirst();
    }

    private void DesgestionarInputs()
    {
        InputManager.Instance.controles.UI.MovimientoDer.performed -= contexto => OptionallySelectFirst();
        InputManager.Instance.controles.UI.MovimientoIzq.performed -= contexto => OptionallySelectFirst();
        InputManager.Instance.controles.UI.Arriba.performed -= contexto => OptionallySelectFirst();
        InputManager.Instance.controles.UI.Abajo.performed -= contexto => OptionallySelectFirst();
    }
}
