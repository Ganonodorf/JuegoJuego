using PixelCrushers.DialogueSystem;
using UnityEngine;

public class RespawnManager : MonoBehaviour
{
    public static RespawnManager Instance;
    private GameObject[] puntosRespawn;

    private GameObject playerGO;

    private void Awake()
    {
        //HacerloInmortal();

        Instance = this;
    }
    private void Start()
    {
        BuscarGO();
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

    public void MoverARespawnMasCercano()
    {
        GameObject puntoRespawnMasCercano;
        //puntoRespawnMasCercano = BuscarPuntoRespawnMasCercano();

        if (DialogueLua.GetVariable("PuertaSegundaZonaAbierta").asBool == false)
        {
            puntoRespawnMasCercano = puntosRespawn[0];
        }
        else
        {
            puntoRespawnMasCercano = puntosRespawn[1];
        }

        playerGO.transform.GetComponent<Rigidbody>().velocity = Vector3.zero;

        playerGO.transform.SetPositionAndRotation(new Vector3(puntoRespawnMasCercano.transform.position.x,
                                                              puntoRespawnMasCercano.transform.position.y,
                                                              puntoRespawnMasCercano.transform.position.z),

                                                  new Quaternion(puntoRespawnMasCercano.transform.rotation.x,
                                                                 puntoRespawnMasCercano.transform.rotation.y,
                                                                 puntoRespawnMasCercano.transform.rotation.z,
                                                                 puntoRespawnMasCercano.transform.rotation.w));
    }

    private GameObject BuscarPuntoRespawnMasCercano()
    {
        GameObject puntoRespawnMasCercano = puntosRespawn[0];
        float distanciaMasCorta = Vector3.Distance(puntosRespawn[0].transform.position, playerGO.transform.position);

        foreach (GameObject respawn in puntosRespawn)
        {
            float nuevaDistancia = Vector3.Distance(respawn.transform.position, playerGO.transform.position);
            if (nuevaDistancia < distanciaMasCorta)
            {
                puntoRespawnMasCercano = respawn;
                distanciaMasCorta = nuevaDistancia;
            }
        }

        return puntoRespawnMasCercano;
    }

    private void BuscarGO()
    {
        puntosRespawn = GameObject.FindGameObjectsWithTag(Constantes.Juego.TAG_RESPAWN);

        playerGO = GameObject.FindGameObjectWithTag(Constantes.Player.TAG_PLAYER);
    }
}
