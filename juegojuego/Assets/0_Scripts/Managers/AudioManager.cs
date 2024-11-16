using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [SerializeField] private AudioClip cocheAbriendose;
    [SerializeField] private AudioClip cocheCerrandose;

    private AudioSource audioSource;

    [SerializeField] private GameObject lucesDelanteras;

    private void Awake()
    {
        HacerloInmortal();

        audioSource = GetComponent<AudioSource>();
    }

    // Start is called before the first frame update
    void Start()
    {
        GameManager.OnGameStateChanged += CambioEstadoJuego;
    }

    private void CambioEstadoJuego(GameState nuevoEstadoJuego, GameState viejoEstadoJuego)
    {
        if(nuevoEstadoJuego == GameState.Inventario)
        {
            audioSource.PlayOneShot(cocheAbriendose);
        }

        if(viejoEstadoJuego == GameState.Inventario)
        {
            audioSource.PlayOneShot(cocheCerrandose);
        }
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
}
