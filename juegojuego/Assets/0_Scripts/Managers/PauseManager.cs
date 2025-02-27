using Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseManager : MonoBehaviour
{
    private GameState estadoAlQueVolver;

    [SerializeField] private GameObject pausaGO;

    [SerializeField] private GameObject resumeButton;
    [SerializeField] private GameObject respawnButton;
    [SerializeField] private GameObject exitButton;

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

        RespawnManager.Instance.MoverARespawnMasCercano();
    }

    public void BotonReiniciar()
    {
        GameManager.Instance.UpdateGameState(GameState.Conduciendo);

        estadoAlQueVolver = GameState.Conduciendo;

        AparecerPausa(false);

        InputManager.Instance.gameObject.SetActive(false);

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
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
    }

    private void DesgestionarInputs()
    {
        InputManager.Instance.controles.Conduciendo.Pausa.performed -= contexto => Pausa();
        InputManager.Instance.controles.Inventario.Pausa.performed -= contexto => Pausa();
    }
}
