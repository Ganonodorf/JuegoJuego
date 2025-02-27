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
}
