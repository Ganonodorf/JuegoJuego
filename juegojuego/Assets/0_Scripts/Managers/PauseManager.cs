using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseManager : MonoBehaviour
{
    private GameState estadoAlQueVolver;

    private GameObject pausaGO;

    private void Start()
    {
        GestionarInputs();

        pausaGO = GameObject.FindGameObjectWithTag("Pausa");

        LinkearFunciones();
    }

    private void OnDestroy()
    {
        DesgestionarInputs();
    }

    private void LinkearFunciones()
    {
        pausaGO.transform.GetChild(1).transform.GetChild(0).gameObject.GetComponent<Button>().onClick.AddListener(BotonContinuar);
        pausaGO.transform.GetChild(1).transform.GetChild(1).gameObject.GetComponent<Button>().onClick.AddListener(BotonRespawn);
        //pausaGO.transform.GetChild(2).gameObject.GetComponent<Button>().onClick.AddListener(BotonReiniciar);
        pausaGO.transform.GetChild(1).transform.GetChild(3).gameObject.GetComponent<Button>().onClick.AddListener(BotonSalir);
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
        pausaGO.GetComponent<Image>().enabled = estado;

        pausaGO.transform.GetChild(0).gameObject.SetActive(estado);
        pausaGO.transform.GetChild(1).gameObject.SetActive(estado);

        if (estado) { pausaGO.transform.GetChild(1).transform.GetChild(0).gameObject.GetComponent<Button>().Select(); }
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
