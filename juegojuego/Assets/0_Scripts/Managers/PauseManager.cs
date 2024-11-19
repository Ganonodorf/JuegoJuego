using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseManager : MonoBehaviour
{
    private GameState estadoAlQueVolver;

    private GameObject pausaGO;

    private void Start()
    {
        RecogerInfoInputs();

        pausaGO = GameObject.FindGameObjectWithTag("Pausa");

        //LinkearFunciones();
    }

    private void LinkearFunciones()
    {
        pausaGO.transform.GetChild(0).gameObject.GetComponent<Button>().onClick.AddListener(BotonContinuar);
        pausaGO.transform.GetChild(1).gameObject.GetComponent<Button>().onClick.AddListener(BotonRespawn);
        //pausaGO.transform.GetChild(2).gameObject.GetComponent<Button>().onClick.AddListener(BotonReiniciar);
        pausaGO.transform.GetChild(3).gameObject.GetComponent<Button>().onClick.AddListener(BotonSalir);
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
        
        Debug.Log("Boton respawn");
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
        if (estado) { pausaGO.transform.GetChild(0).gameObject.GetComponent<Button>().Select(); }

        pausaGO.GetComponent<Image>().enabled = estado;

        pausaGO.transform.GetChild(0).gameObject.SetActive(estado);
        pausaGO.transform.GetChild(1).gameObject.SetActive(estado);
        //pausaGO.transform.GetChild(2).gameObject.SetActive(estado);
        pausaGO.transform.GetChild(3).gameObject.SetActive(estado);

        /*
        foreach (RectTransform child in pausaGO.transform.GetComponentsInChildren<RectTransform>())
        {
            child.gameObject.SetActive(estado);
        }
        */
    }

    private void RecogerInfoInputs()
    {
        InputManager.Instance.controles.Conduciendo.Pausa.performed += contexto => Pausa();
        InputManager.Instance.controles.Inventario.Pausa.performed += contexto => Pausa();
        InputManager.Instance.controles.UI.Volver.performed += contexto => BotonContinuar();
    }

    private void SoltarInfoInputs()
    {
        InputManager.Instance.controles.Conduciendo.Pausa.performed -= contexto => Pausa();
        InputManager.Instance.controles.Inventario.Pausa.performed -= contexto => Pausa();
        InputManager.Instance.controles.UI.Volver.performed -= contexto => BotonContinuar();
    }
}
