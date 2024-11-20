using Cinemachine;
using UnityEngine;
using UnityEngine.UI;

public class CreditosManager : MonoBehaviour
{
    private GameObject creditosGO;

    private GameObject camaraCreditos;

    private void Start()
    {
        creditosGO = GameObject.FindGameObjectWithTag("Creditos");
        camaraCreditos = GameObject.FindGameObjectWithTag("CreditosCamara");

        GameManager.OnGameStateChanged += GameManager_OnGameStateChanged;

        LinkearFunciones();
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
        creditosGO.transform.GetChild(3).transform.GetChild(0).gameObject.GetComponent<Button>().onClick.AddListener(BotonSalir);
    }

    public void BotonSalir()
    {
        Application.Quit();
    }

    private void AparecerMenu(bool estado)
    {
        creditosGO.transform.GetChild(0).gameObject.SetActive(estado);
        creditosGO.transform.GetChild(1).gameObject.SetActive(estado);
        creditosGO.transform.GetChild(2).gameObject.SetActive(estado);
        creditosGO.transform.GetChild(3).gameObject.SetActive(estado);

        if (estado) { creditosGO.transform.GetChild(3).transform.GetChild(0).gameObject.GetComponent<Button>().Select(); }
    }
}
