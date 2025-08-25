using Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PauseManager : MonoBehaviour
{
    private GameState estadoAlQueVolver;

    [SerializeField] private GameObject pausaGO;

    [SerializeField] private GameObject resumeButton;
    [SerializeField] private GameObject respawnButton;
    [SerializeField] private GameObject switchLanguageButton;
    [SerializeField] private GameObject exitButton;
    [SerializeField] private GameObject sliderX;
    [SerializeField] private GameObject sliderY;

    [SerializeField] private GameObject camaraExteriores;
    [SerializeField] private GameObject camaraInteriores;

    private void Start()
    {
        GestionarInputs();

        LinkearFunciones();
    }

    private void OnDestroy()
    {
        DesgestionarInputs();
    }

    private void LinkearFunciones()
    {
        resumeButton.GetComponent<Button>().onClick.AddListener(BotonContinuar);
        respawnButton.GetComponent<Button>().onClick.AddListener(BotonRespawn);
        //pausaGO.transform.GetChild(2).gameObject.GetComponent<Button>().onClick.AddListener(BotonReiniciar);
        switchLanguageButton.GetComponent<Button>().onClick.AddListener(BotonSwitchLanguage);
        exitButton.GetComponent<Button>().onClick.AddListener(BotonSalir);
    }

    private void Pausa()
    {
        AparecerPausa(true);

        estadoAlQueVolver = GameManager.Instance.GetGameState();
        GameManager.Instance.UpdateGameState(GameState.PantallaPausa);
    }

    public void BotonContinuar()
    {
        AparecerPausa(false);

        GameManager.Instance.UpdateGameState(estadoAlQueVolver);
    }

    public void BotonRespawn()
    {
        AparecerPausa(false);

        if(estadoAlQueVolver == GameState.Inventario)
        {
            InventarioManager.Instance.CerrarInventario();
        }
        else
        {
            GameManager.Instance.UpdateGameState(GameState.Conduciendo);
        }

        RespawnManager.Instance.MoveToActiveRespawnPoint();
    }

    public void BotonReiniciar()
    {
        GameManager.Instance.UpdateGameState(GameState.Conduciendo);

        estadoAlQueVolver = GameState.Conduciendo;

        AparecerPausa(false);

        InputManager.Instance.gameObject.SetActive(false);

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void BotonSwitchLanguage()
    {
        GameManager.Instance.SwitchLanguage();
    }

    public void BotonSalir()
    {
        Application.Quit();
    }

    private void AparecerPausa(bool estado)
    {
        pausaGO.SetActive(estado);

        OnOffCameraMovement(!estado);

        if (estado) { resumeButton.GetComponent<Button>().Select(); }
    }

    private void OnOffCameraMovement(bool state)
    {
        camaraExteriores.GetComponent<CinemachineInputProvider>().enabled = state;
        camaraInteriores.GetComponent<CinemachineInputProvider>().enabled = state;
    }

    private void GestionarInputs()
    {
        InputManager.Instance.controles.Conduciendo.Pausa.performed += contexto => Pausa();
        InputManager.Instance.controles.Inventario.Pausa.performed += contexto => Pausa();

        InputManager.Instance.controles.UI.UnPause.performed += contexto => BotonContinuar();

        InputManager.Instance.controles.UI.MovimientoDer.performed += contexto => OptionallySelectFirst();
        InputManager.Instance.controles.UI.MovimientoIzq.performed += contexto => OptionallySelectFirst();
        InputManager.Instance.controles.UI.Arriba.performed += contexto => OptionallySelectFirst();
        InputManager.Instance.controles.UI.Abajo.performed += contexto => OptionallySelectFirst();
    }

    private void DesgestionarInputs()
    {
        InputManager.Instance.controles.Conduciendo.Pausa.performed -= contexto => Pausa();
        InputManager.Instance.controles.Inventario.Pausa.performed -= contexto => Pausa();

        InputManager.Instance.controles.UI.UnPause.performed -= contexto => BotonContinuar();

        InputManager.Instance.controles.UI.MovimientoDer.performed -= contexto => OptionallySelectFirst();
        InputManager.Instance.controles.UI.MovimientoIzq.performed -= contexto => OptionallySelectFirst();
        InputManager.Instance.controles.UI.Arriba.performed -= contexto => OptionallySelectFirst();
        InputManager.Instance.controles.UI.Abajo.performed -= contexto => OptionallySelectFirst();
    }

    private void OptionallySelectFirst()
    {
        if (pausaGO.activeSelf
            && EventSystem.current.currentSelectedGameObject != resumeButton
            && EventSystem.current.currentSelectedGameObject != respawnButton
            && EventSystem.current.currentSelectedGameObject != switchLanguageButton
            && EventSystem.current.currentSelectedGameObject != exitButton
            && EventSystem.current.currentSelectedGameObject != sliderX
            && EventSystem.current.currentSelectedGameObject != sliderY
            )
            resumeButton.GetComponent<Button>().Select();
    }
}
