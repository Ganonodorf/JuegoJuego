using PixelCrushers;
using System;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance;

    public ControlesPlayer controles;

    private void Awake()
    {
        SuscribirseEventos();

        InicializarControles();

        RegistrarControlesDyalogueSystem();

        //HacerloInmortal();

        Instance = this;
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
            case GameState.FinJuego:
                Debug.Log("Controles: FinJuego. A CAMBIAR CUANDO DECIDAMOS QU� HACER");
                ControlesFinJuego();
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
    }

    private void ControlesInventario()
    {
        controles.Conduciendo.Disable();
        controles.Inventario.Enable();
        controles.Dialogo.Disable();
        controles.UI.Disable();
    }

    private void ControlesDialogo()
    {
        controles.Conduciendo.Disable();
        controles.Inventario.Disable();
        controles.Dialogo.Enable();
        controles.UI.Disable();
    }

    private void ControlesUI()
    {
        controles.Conduciendo.Disable();
        controles.Inventario.Disable();
        controles.Dialogo.Disable();
        controles.UI.Enable();
    }

    private void ControlesFinJuego()
    {
        controles.Conduciendo.Disable();
        controles.Inventario.Disable();
        controles.Dialogo.Disable();
        controles.UI.Disable();
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
        controles.Conduciendo.Enable();
    }

    private void RegistrarControlesDyalogueSystem()
    {
        InputDeviceManager.RegisterInputAction("Accion", controles.Conduciendo.Accion);
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
