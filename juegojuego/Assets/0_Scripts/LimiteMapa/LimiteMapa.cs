using UnityEngine;

public class LimiteMapa : MonoBehaviour
{
    private void OnTriggerEnter(Collider collider)
    {
        if(collider.transform.name == Constantes.Player.NOMBRE_GO)
        {
            RespawnManager.Instance.MoveToActiveRespawnPoint();
        }
    }
}
