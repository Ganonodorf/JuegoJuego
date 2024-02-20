using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Con esto declaras que esta clase va a ser est�tica, es decir, va a estar siempre activa
    // y disponible para todos
    public static GameManager Instance;

    // Este va a ser el estado actual de juego
    private GameState state;

    // Aqu� declaramos el evento, su nombre y que recive un GameState
    public static event Action<GameState> OnGameStateChanged;

    // En el awake decimos que si cuando el GameObject que tenga este script es creado
    // no existe ya un GameManager, asignamos Instance a ese GameObject.
    // Si no es as� y ya existe un GameManager, el que se acaba de crear se borra.
    private void Awake()
    {
        if(Instance == null)
        {
            DontDestroyOnLoad(gameObject);
            Instance = this;
        }
        else if(Instance != this)
        {
            Destroy(gameObject);
        }
    }

    // Para devolver el GameState
    public GameState GetGameState()
    {
        return state;
    }

    // Este m�todo se usa para cambiar el estado de juego por otros scripts y actualiza el estado actual.
    // Lo bueno que tiene es que si se hace, se manda una se�al que indica cu�l es el nuevo estado.
    public void UpdateGameState(GameState newState)
    {
        state = newState;

        switch(newState)
        {
            case GameState.Playing:
                //Hacer algo
            case GameState.Conversation:
                //Hacer algo
            default:
                break;
        }

        OnGameStateChanged?.Invoke(newState);
    }
}

// Enumerado que contiene los diferentes estados de juego
public enum GameState
{
    Playing,
    Conversation
}