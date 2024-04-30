using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinJuego : MonoBehaviour
{
    private void OnTriggerEnter(Collider collider)
    {
        if(collider.transform.name == Constantes.Player.NOMBRE_GO)
        {
            GameManager.Instance.UpdateGameState(GameState.FinJuego);
        }
    }
}
