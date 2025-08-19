using Constantes;
using UnityEngine;

public class RespawnManager : MonoBehaviour
{
    public static RespawnManager Instance;
    private GameObject[] puntosRespawn;

    private GameObject playerGO;

    [SerializeField] private GameObject juanaRespawn;
    [SerializeField] private GameObject poligonoRespawn;
    [SerializeField] private GameObject exteriorRespawn;
    [SerializeField] private GameObject farewellRespawn;
    [SerializeField] private GameObject tunelRespawn;

    private GameObject activeRespawnPoint;

    

    public void UpdateActiveRespawn(RespawnPoint newRespawn)
    {
        switch (newRespawn)
        {
            case RespawnPoint.JuanaRespawn:
                activeRespawnPoint = juanaRespawn;
                break;
            case RespawnPoint.PoligonoRespawn:
                activeRespawnPoint = poligonoRespawn;
                break;
            case RespawnPoint.ExteriorRespawn:
                activeRespawnPoint = exteriorRespawn;
                break;
            case RespawnPoint.FarewellRespawn:
                activeRespawnPoint = farewellRespawn;
                break;
            case RespawnPoint.TunelRespawn:
                activeRespawnPoint = tunelRespawn;
                break;
            default:
                break;
        }
    }

    public void MoveToActiveRespawnPoint()
    {
        playerGO.transform.GetComponent<Rigidbody>().velocity = Vector3.zero;

        playerGO.transform.GetComponent<Rigidbody>().position = new Vector3(activeRespawnPoint.transform.position.x,
                                                              activeRespawnPoint.transform.position.y,
                                                              activeRespawnPoint.transform.position.z);

        playerGO.transform.GetComponent<Rigidbody>().rotation = new Quaternion(activeRespawnPoint.transform.rotation.x,
                                                                 activeRespawnPoint.transform.rotation.y,
                                                                 activeRespawnPoint.transform.rotation.z,
                                                                 activeRespawnPoint.transform.rotation.w);
    }

    public void UpdateActiveRespawnJuana()
    {
        UpdateActiveRespawn(RespawnPoint.JuanaRespawn);
    }

    public void UpdateActiveRespawnPoligono()
    {
        UpdateActiveRespawn(RespawnPoint.PoligonoRespawn);
    }

    public void UpdateActiveRespawnExterior()
    {
        UpdateActiveRespawn(RespawnPoint.ExteriorRespawn);
    }

    public void UpdateActiveRespawnFarewell()
    {
        UpdateActiveRespawn(RespawnPoint.FarewellRespawn);
    }

    public void UpdateActiveRespawnTunel()
    {
        UpdateActiveRespawn(RespawnPoint.TunelRespawn);
    }


    private void Awake()
    {
        //HacerloInmortal();

        Instance = this;
    }
    private void Start()
    {
        BuscarGO();
        UpdateActiveRespawnJuana();
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

    /*
    public void MoverARespawnMasCercano()
    {
        GameObject puntoRespawnMasCercano = BuscarPuntoRespawnMasCercano();

        playerGO.transform.GetComponent<Rigidbody>().velocity = Vector3.zero;

        playerGO.transform.GetComponent<Rigidbody>().position = new Vector3(puntoRespawnMasCercano.transform.position.x,
                                                              puntoRespawnMasCercano.transform.position.y,
                                                              puntoRespawnMasCercano.transform.position.z);

        playerGO.transform.GetComponent<Rigidbody>().rotation = new Quaternion(puntoRespawnMasCercano.transform.rotation.x,
                                                                 puntoRespawnMasCercano.transform.rotation.y,
                                                                 puntoRespawnMasCercano.transform.rotation.z,
                                                                 puntoRespawnMasCercano.transform.rotation.w);
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
    */

    private void BuscarGO()
    {
        //puntosRespawn = GameObject.FindGameObjectsWithTag(Constantes.Juego.TAG_RESPAWN);

        playerGO = GameObject.FindGameObjectWithTag(Constantes.Player.TAG_PLAYER);
    }
}


public enum RespawnPoint
{
    JuanaRespawn,
    PoligonoRespawn,
    ExteriorRespawn,
    FarewellRespawn,
    TunelRespawn
}

