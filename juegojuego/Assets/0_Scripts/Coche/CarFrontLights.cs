using UnityEngine;

public class CarFrontLights : MonoBehaviour
{
    private void Start()
    {
        InputManager.Instance.controles.Conduciendo.Luces.performed += contexto => TriggerLuces();
    }

    private void TriggerLuces()
    {
        this.gameObject.GetComponent<Light>().enabled = !this.gameObject.GetComponent<Light>().enabled;
    }
}
