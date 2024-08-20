using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComprobarPuntoSoltarObjetoDisponible : MonoBehaviour
{
    public bool puntoDisponible = true;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        puntoDisponible = false;
    }

    private void OnTriggerStay(Collider other)
    {
        puntoDisponible = false;
    }

    private void OnTriggerExit(Collider other)
    {
        puntoDisponible = true;
    }
}
