using PixelCrushers.DialogueSystem;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [SerializeField] private AudioClip cocheAbriendose;
    [SerializeField] private AudioClip cocheCerrandose;
    [SerializeField] private AudioClip pito;

    private AudioSource audioSource;

    private void Awake()
    {
        //HacerloInmortal();

        Instance = this;

        audioSource = GetComponent<AudioSource>();
    }

    // Start is called before the first frame update
    void Start()
    {
        GameManager.OnGameStateChanged += CambioEstadoJuego;

        InputManager.Instance.controles.Conduciendo.Pito.performed += contexto => HacerSonarElPito();
        InputManager.Instance.controles.Conduciendo.Pito.canceled += contexto => DejarDeSonarElPito();
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

    private void HacerSonarElPito()
    {
        audioSource.clip = pito;
        audioSource.loop = true;
        audioSource.Play();
    }

    private void DejarDeSonarElPito()
    {
        audioSource.Stop();
        audioSource.loop = false;
        audioSource.clip = null;
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
