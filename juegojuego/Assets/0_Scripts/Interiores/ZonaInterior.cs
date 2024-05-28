using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZonaInterior : MonoBehaviour
{
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.transform.tag == Constantes.Player.TAG_PLAYER)
        {
            CamaraManager.Instance.JugadorHaEntrado();
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        if (collision.transform.tag == Constantes.Player.TAG_PLAYER)
        {
            CamaraManager.Instance.JugadorHaSalido();
        }
    }
}
