using System;
using UnityEngine;

public class LimiteMapa : MonoBehaviour
{
    private GameObject puntoRespawn;

    private void Start()
    {
        BuscarGO();
    }


    private void OnTriggerEnter(Collider collider)
    {
        if(collider.transform.name == Constantes.Player.NOMBRE_GO)
        {
            MoverARespawn(collider.gameObject);
        }
    }

    private void MoverARespawn(GameObject goAMover)
    {
        goAMover.transform.SetPositionAndRotation(new Vector3(puntoRespawn.transform.position.x,
                                                              puntoRespawn.transform.position.y,
                                                              puntoRespawn.transform.position.z),

                                                  new Quaternion(puntoRespawn.transform.rotation.x,
                                                                 puntoRespawn.transform.rotation.y,
                                                                 puntoRespawn.transform.rotation.z,
                                                                 puntoRespawn.transform.rotation.w));
    }

    private void BuscarGO()
    {
        puntoRespawn = GameObject.FindGameObjectWithTag(Constantes.Juego.TAG_RESPAWN);
    }
}
