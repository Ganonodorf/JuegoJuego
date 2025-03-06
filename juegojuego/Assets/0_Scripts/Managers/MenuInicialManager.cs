using Cinemachine;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MenuInicialManager : MonoBehaviour
{
    [SerializeField] private GameObject menuInicialGO;

    private GameObject camaraMenu;

    [SerializeField] private GameObject startButton;
    [SerializeField] private GameObject exitButton;


    private void Start()
    {
        camaraMenu = GameObject.FindGameObjectWithTag("MenuInicialCamara");

        GameManager.OnGameStateChanged += GameManager_OnGameStateChanged;

        startButton.GetComponent<Button>().Select();

        LinkearFunciones();

        GestionarInputs();
    }

    private void OnDestroy()
    {
        GameManager.OnGameStateChanged -= GameManager_OnGameStateChanged;

        DesgestionarInputs();
    }

    private void GameManager_OnGameStateChanged(GameState nuevoEstadoJuego, GameState viejoEstadoJuego)
    {
        if(nuevoEstadoJuego == GameState.MenuInicio)
        {
            AparecerMenu(true);

            camaraMenu.GetComponent<CinemachineVirtualCamera>().enabled = true;
        }
    }

    private void LinkearFunciones()
    {
        startButton.GetComponent<Button>().onClick.AddListener(BotonJugar);
        exitButton.GetComponent<Button>().onClick.AddListener(BotonSalir);
    }

    public void BotonJugar()
    {
        AparecerMenu(false);

        camaraMenu.GetComponent<CinemachineVirtualCamera>().enabled = false;

        GameManager.Instance.UpdateGameState(GameState.SecuenciaInicial);
    }

    public void BotonSalir()
    {
        Application.Quit();
    }

    private void AparecerMenu(bool estado)
    {
        menuInicialGO.SetActive(estado);

        if (estado) { startButton.GetComponent<Button>().Select(); }
    }

    private void OptionallySelectFirst()
    {
        if(menuInicialGO.activeSelf &&
           EventSystem.current.currentSelectedGameObject == null)
        startButton.GetComponent<Button>().Select();
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
