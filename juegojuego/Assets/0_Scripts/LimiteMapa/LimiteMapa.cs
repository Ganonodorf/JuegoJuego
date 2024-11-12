using System;
using UnityEngine;

public class LimiteMapa : MonoBehaviour
{
    private GameObject[] puntosRespawn;

    private void Start()
    {
        BuscarGO();
    }


    private void OnTriggerEnter(Collider collider)
    {
        if(collider.transform.name == Constantes.Player.NOMBRE_GO)
        {
            MoverARespawnMasCercano(collider.gameObject);
        }
    }

    private void MoverARespawnMasCercano(GameObject goAMover)
    {
        GameObject puntoRespawnMasCercano = BuscarPuntoRespawnMasCercano(goAMover);

        goAMover.transform.SetPositionAndRotation(new Vector3(puntoRespawnMasCercano.transform.position.x,
                                                              puntoRespawnMasCercano.transform.position.y,
                                                              puntoRespawnMasCercano.transform.position.z),

                                                  new Quaternion(puntoRespawnMasCercano.transform.rotation.x,
                                                                 puntoRespawnMasCercano.transform.rotation.y,
                                                                 puntoRespawnMasCercano.transform.rotation.z,
                                                                 puntoRespawnMasCercano.transform.rotation.w));
    }

    private GameObject BuscarPuntoRespawnMasCercano(GameObject goABuscar)
    {
        GameObject puntoRespawnMasCercano = puntosRespawn[0];
        float distanciaMasCorta = Vector3.Distance(puntosRespawn[0].transform.position, goABuscar.transform.position);

        foreach (GameObject respawn in puntosRespawn)
        {
            float nuevaDistancia = Vector3.Distance(respawn.transform.position, goABuscar.transform.position);
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
    }
}
