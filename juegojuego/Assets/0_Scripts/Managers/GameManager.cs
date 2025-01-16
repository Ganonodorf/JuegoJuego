using System;
using System.Runtime.CompilerServices;
using PixelCrushers.DialogueSystem;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;

public class GameManager : MonoBehaviour
{
    // Con esto declaras que esta clase va a ser est?tica, es decir, va a estar siempre activa
    // y disponible para todos.
    // El static hace que la clase simplemente exista, siempre.
    public static GameManager Instance;

    // Este va a ser el estado actual de juego
    private GameState state;
    private GameState previousState;

    // Aqu? declaramos el evento, su nombre y que recibe un GameState
    public static event Action<GameState, GameState> OnGameStateChanged;

    [SerializeField] private GameState initialGameState;

    private bool finJuego;

    // En el awake decimos que si cuando el GameObject que tenga este script es creado
    // no existe ya un GameManager, asignamos Instance a ese GameObject.
    // Si no es as? y ya existe un GameManager, el que se acaba de crear se borra.
    private void Awake()
    {
        //HacerloInmortal();

        Instance = this;
    }

    private void Start()
    {
        //initialGameState = GameState.MenuInicio;

        finJuego = false;

        HacerCursorInvisible();

        UpdateGameState(initialGameState);
    }

    // Para devolver el GameState
    public GameState GetGameState()
    {
        return state;
    }

    // Para devolver el GameState
    public GameState GetPreviousGameState()
    {
        return previousState;
    }

    // Este m?todo se usa para cambiar el estado de juego por otros scripts y actualiza el estado actual.
    // Lo bueno que tiene es que si se hace, se manda una se?al que indica cu?l es el nuevo estado.
    public void UpdateGameState(GameState newState)
    {
        previousState = state;
        state = newState;

        if(newState == GameState.Conduciendo && finJuego) { newState = GameState.FinJuego; }

        switch (newState)
        {
            case GameState.MenuInicio:
                Debug.Log("Estado de juego: MenuInicio");
                break;
            case GameState.SecuenciaInicial:
                SecuenciaInicial();
                Debug.Log("Estado de juego: SecuenciaInicial");
                break;
            case GameState.PantallaPausa:
                Debug.Log("Estado de juego: PantallaPausa");
                break;
            case GameState.Conduciendo:
                Debug.Log("Estado de juego: Conduciendo");
                break;
            case GameState.Dialogo:
                Debug.Log("Estado de juego: Dialogo");
                break;
            case GameState.Inventario:
                Debug.Log("Estado de juego: Inventario");
                break;
            case GameState.SecuenciaFinal:
                Debug.Log("Estado de juego: SecuenciaFinal");
                SecuenciaFinal();
                break;
            case GameState.FinJuego:
                Debug.Log("Estado de juego: FinJuego");
                break;
            case GameState.Editor:
                Debug.Log("Estado de juego: Editor");
                break;
            default:
                break;
        }

        OnGameStateChanged?.Invoke(newState, previousState);
    }

    private void SecuenciaInicial()
    {
        DialogueManager.StartConversation("Intro cutscene");
    }

    private void SecuenciaFinal()
    {
        DialogueManager.StartConversation("Fin Cutscene");

        finJuego = true;
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

    public void UpdateGameStateDialogo()
    {
        UpdateGameState(GameState.Dialogo);
    }

    public void UpdateGameStateConduciendo()
    {
        UpdateGameState(GameState.Conduciendo);
    }

    private void HacerCursorInvisible()
    {
        Cursor.visible = false;
    }
}

// Enumerado que contiene los diferentes estados de juego
public enum GameState
{
    MenuInicio,
    SecuenciaInicial,
    PantallaPausa,
    Conduciendo,
    Dialogo,
    Inventario,
    SecuenciaFinal,
    FinJuego,
    Editor
}