using PixelCrushers;
using System;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance;

    public ControlesPlayer controles;

    private void Awake()
    {
        InicializarControles();

        RegistrarControlesDyalogueSystem();

        //HacerloInmortal();

        Instance = this;
    }

    private void Start()
    {
        SuscribirseEventos();
    }

    private void OnDestroy()
    {
        DesuscribirseEventos();

        DesregistrarControlesDyalogueSystem();
    }

    private void GameManager_OnGameStateChanged(GameState nuevoEstado, GameState antiguoEstado)
    {
        switch (nuevoEstado)
        {
            case GameState.MenuInicio:
                Debug.Log("Controles: UI");
                ControlesUI();
                break;
            case GameState.SecuenciaInicial:
                Debug.Log("Controles: Dialogo");
                ControlesDialogo();
                break;
            case GameState.Conduciendo:
                Debug.Log("Controles: Conduciendo");
                ControlesConduciendo();
                break;
            case GameState.Inventario:
                Debug.Log("Controles: Inventario");
                ControlesInventario();
                break;
            case GameState.Dialogo:
                Debug.Log("Controles: Dialogo");
                ControlesDialogo();
                break;
            case GameState.PantallaPausa:
                Debug.Log("Controles: UI");
                ControlesUI();
                break;
            case GameState.SecuenciaFinal:
                Debug.Log("Controles: SecuenciaFinal");
                ControlesDialogo();
                break;
            case GameState.FinJuego:
                Debug.Log("Controles: FinJuego");
                ControlesUI();
                break;
            case GameState.Editor:
                Debug.Log("Controles: Editor");
                ControlesEditor();
                break;
            default:
                break;
        }
    }

    private void ControlesConduciendo()
    {
        controles.Conduciendo.Enable();
        controles.Inventario.Disable();
        controles.Dialogo.Disable();
        controles.UI.Disable();
        controles.CamaraEditor.Disable();
    }

    private void ControlesInventario()
    {
        controles.Conduciendo.Disable();
        controles.Inventario.Enable();
        controles.Dialogo.Disable();
        controles.UI.Disable();
        controles.CamaraEditor.Disable();
    }

    private void ControlesDialogo()
    {
        controles.Conduciendo.Disable();
        controles.Inventario.Disable();
        controles.Dialogo.Enable();
        controles.UI.Disable();
        controles.CamaraEditor.Disable();
    }

    private void ControlesUI()
    {
        controles.Conduciendo.Disable();
        controles.Inventario.Disable();
        controles.Dialogo.Disable();
        controles.UI.Enable();
        controles.CamaraEditor.Disable();
    }

    private void ControlesEditor()
    {
        controles.Conduciendo.Disable();
        controles.Inventario.Disable();
        controles.Dialogo.Disable();
        controles.UI.Disable();
        controles.CamaraEditor.Enable();
    }

    private void HacerloInmortal()
    {
        if (Instance == null)
        {
            DontDestroyOnLoad(gameObject);
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    private void SuscribirseEventos()
    {
        GameManager.OnGameStateChanged += GameManager_OnGameStateChanged;
    }

    private void InicializarControles()
    {
        controles = new ControlesPlayer();
        controles.UI.Enable();
    }

    private void RegistrarControlesDyalogueSystem()
    {
        InputDeviceManager.RegisterInputAction("Accion", controles.Conduciendo.Accion);
        InputDeviceManager.RegisterInputAction("Seleccion", controles.UI.Accion);
    }

    private void DesuscribirseEventos()
    {
        GameManager.OnGameStateChanged -= GameManager_OnGameStateChanged;
    }

    private void DesregistrarControlesDyalogueSystem()
    {
        InputDeviceManager.UnregisterInputAction("Accion");
        InputDeviceManager.UnregisterInputAction("Seleccion");
    }
}
