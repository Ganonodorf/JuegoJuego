using Cinemachine;
using UnityEngine;

public class CamaraController : MonoBehaviour
{
    GameObject CamaraExterioresGO;
    GameObject CamaraInterioresGO;

    // Start is called before the first frame update
    void Start()
    {
        BuscarGO();
    }

    private void OnTriggerEnter(Collider collision)
    {
        if(collision.transform.name == Constantes.Player.NOMBRE_GO)
        {
            DesactivarExteriorActivarInterior();
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        if (collision.transform.name == Constantes.Player.NOMBRE_GO)
        {
            ActivarExteriorDesactivarInterior();
        }
    }

    private void DesactivarExteriorActivarInterior()
    {
        CamaraExterioresGO.GetComponent<CinemachineFreeLook>().enabled = false;
        CamaraInterioresGO.GetComponent<CinemachineFreeLook>().enabled = true;
    }

    private void ActivarExteriorDesactivarInterior()
    {
        CamaraExterioresGO.GetComponent<CinemachineFreeLook>().enabled = true;
        CamaraInterioresGO.GetComponent<CinemachineFreeLook>().enabled = false;
    }

    private void BuscarGO()
    {
        CamaraExterioresGO = GameObject.Find(Constantes.Camaras.CAMARA_EXTERIORES_NOMBRE_GO);
        CamaraInterioresGO = GameObject.Find(Constantes.Camaras.CAMARA_INTERIORES_NOMBRE_GO);
    }
}
