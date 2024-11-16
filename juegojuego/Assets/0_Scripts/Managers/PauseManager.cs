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

    public void BotonSalir()
    {
        GameManager.Instance.UpdateGameState(GameState.Conduciendo);

        AparecerPausa(false);

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void AparecerPausa(bool estado)
    {
        pausaGO.GetComponent<Image>().enabled = estado;
        pausaGO.transform.GetChild(0).gameObject.SetActive(estado);
        pausaGO.transform.GetChild(1).gameObject.SetActive(estado);
        pausaGO.transform.GetChild(2).gameObject.SetActive(estado);
    }

    private void RecogerInfoInputs()
    {
        InputManager.Instance.controles.Conduciendo.Pausa.performed += contexto => Pausa();
        InputManager.Instance.controles.Inventario.Pausa.performed += contexto => Pausa();
        InputManager.Instance.controles.UI.Volver.performed += contexto => BotonContinuar();
    }
}
