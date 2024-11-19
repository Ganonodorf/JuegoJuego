using UnityEngine;

public class CarBreakLights : MonoBehaviour
{
    private void Start()
    {
        InputManager.Instance.controles.Conduciendo.Derrape.performed += contexto => EncenderLuces(true);
        InputManager.Instance.controles.Conduciendo.Derrape.canceled += contexto => EncenderLuces(false);
    }

    private void OnDestroy()
    {
        InputManager.Instance.controles.Conduciendo.Derrape.performed -= contexto => EncenderLuces(true);
        InputManager.Instance.controles.Conduciendo.Derrape.canceled -= contexto => EncenderLuces(false);
    }

    private void EncenderLuces(bool estado)
    {
        if (this != null)
        {
            this.transform.GetChild(0).gameObject.SetActive(estado);
        }
    }
}
