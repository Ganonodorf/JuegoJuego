using UnityEngine;

public class CarFrontLights : MonoBehaviour
{
    [SerializeField] private AudioClip sonidoEncenderLuces;
    [SerializeField] private AudioClip sonidoApagarLuces;

    private AudioSource audioSource;

    private void Start()
    {
        InputManager.Instance.controles.Conduciendo.Luces.performed += contexto => TriggerLuces();
        InputManager.Instance.controles.Dialogo.Luces.performed += contexto => TriggerLuces();
        audioSource = GetComponent<AudioSource>();
    }

    private void OnDestroy()
    {
        InputManager.Instance.controles.Conduciendo.Luces.performed -= contexto => TriggerLuces();
        InputManager.Instance.controles.Dialogo.Luces.performed -= contexto => TriggerLuces();
    }

    private void TriggerLuces()
    {
        if(this != null)
        {
            bool nuevoEstadoLuces = !this.gameObject.GetComponent<Light>().enabled;

            if (nuevoEstadoLuces)
            {
                audioSource.PlayOneShot(sonidoEncenderLuces);
            }
            else
            {
                audioSource.PlayOneShot(sonidoApagarLuces);
            }

            this.gameObject.GetComponent<Light>().enabled = nuevoEstadoLuces;
        }
    }
}
