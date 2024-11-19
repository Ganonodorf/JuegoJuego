using Cinemachine;
using UnityEngine;
using UnityEngine.UI;

public class MenuInicialManager : MonoBehaviour
{
    private GameObject menuInicialGO;

    private GameObject camaraMenu;

    private void Start()
    {
        menuInicialGO = GameObject.FindGameObjectWithTag("MenuInicial");
        camaraMenu = GameObject.FindGameObjectWithTag("CamaraMenuInicial");

        GameManager.OnGameStateChanged += GameManager_OnGameStateChanged;

        LinkearFunciones();
    }

    private void GameManager_OnGameStateChanged(GameState nuevoEstadoJuego, GameState viejoEstadoJuego)
    {
        if(nuevoEstadoJuego == GameState.MenuInicio)
        {
            AparecerMenu(true);

            camaraMenu.GetComponent<CinemachineFreeLook>().enabled = true;
        }
    }

    private void LinkearFunciones()
    {
        menuInicialGO.transform.GetChild(1).transform.GetChild(0).gameObject.GetComponent<Button>().onClick.AddListener(BotonJugar);
        menuInicialGO.transform.GetChild(1).transform.GetChild(1).gameObject.GetComponent<Button>().onClick.AddListener(BotonSalir);
    }

    public void BotonJugar()
    {
        AparecerMenu(false);

        camaraMenu.GetComponent<CinemachineFreeLook>().enabled = true;

        GameManager.Instance.UpdateGameState(GameState.SecuenciaInicial);
    }

    public void BotonSalir()
    {
        Application.Quit();
    }

    private void AparecerMenu(bool estado)
    {
        if (estado) { menuInicialGO.transform.GetChild(1).transform.GetChild(0).gameObject.GetComponent<Button>().Select(); }

        menuInicialGO.GetComponent<Image>().enabled = estado;

        menuInicialGO.transform.GetChild(1).transform.GetChild(0).gameObject.SetActive(estado);
        menuInicialGO.transform.GetChild(1).transform.GetChild(1).gameObject.SetActive(estado);
    }
}
