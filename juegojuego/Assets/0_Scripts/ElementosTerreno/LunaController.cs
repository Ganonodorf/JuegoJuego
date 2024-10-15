using System.Collections;
using UnityEngine;

public class LunaController : MonoBehaviour
{
    private float tiempoActual;
    private float duracionRotacion;
    private float cantidadRotacionY;
    private float cantidadRotacionZ;
    private bool rotando;

    private Quaternion rotacionLunaInicial;
    private Quaternion rotacionLunaPuntoPartida;
    private Quaternion rotacionLunaObjetivo;

    private void Start()
    {
        InicializarVariables();
    }


    // Update is called once per frame
    void Update()
    {
        if (!rotando)
        {
            rotando = true;

            rotacionLunaPuntoPartida = new Quaternion(transform.rotation.x,
                                                      transform.rotation.y,
                                                      transform.rotation.z,
                                                      transform.rotation.w);

            float numAleatorio = Random.Range(0, 3);

            switch(numAleatorio)
            {
                case 0:
                    RotacionYPositiva();
                    break;
                case 1:
                    RotacionYNegativa();
                    break;
                case 2:
                    RotacionZPositiva();
                    break;
                case 3:
                    RotacionZNegativa();
                    break;
            }

            StartCoroutine("RotarLuna");
        }
    }

    private IEnumerator RotarLuna()
    {
        while (tiempoActual < duracionRotacion)
        {
            transform.rotation = Quaternion.Slerp(rotacionLunaPuntoPartida, rotacionLunaObjetivo, tiempoActual / duracionRotacion);
            tiempoActual += Time.deltaTime;
            yield return null;
        }
        transform.rotation = rotacionLunaObjetivo;

        tiempoActual = 0;
        rotando = false;
    }

    private void RotacionYPositiva()
    {
        rotacionLunaObjetivo = transform.rotation * Quaternion.Euler(0.0f, cantidadRotacionY, 0.0f);
        
        /*
        rotacionLunaObjetivo = new Quaternion(rotacionLunaInicial.x,
                                              rotacionLunaInicial.y + cantidadRotacion,
                                              rotacionLunaInicial.z,
                                              rotacionLunaInicial.w);
        */
    }

    private void RotacionYNegativa()
    {
        rotacionLunaObjetivo = transform.rotation * Quaternion.Euler(0.0f, -cantidadRotacionY, 0.0f);
        /*
        rotacionLunaObjetivo = new Quaternion(rotacionLunaInicial.x,
                                              rotacionLunaInicial.y - cantidadRotacion,
                                              rotacionLunaInicial.z,
                                              rotacionLunaInicial.w);
        */
    }

    private void RotacionZPositiva()
    {
        rotacionLunaObjetivo = transform.rotation * Quaternion.Euler(0.0f, 0.0f, cantidadRotacionZ);
        /*
        rotacionLunaObjetivo = new Quaternion(rotacionLunaInicial.x,
                                              rotacionLunaInicial.y,
                                              rotacionLunaInicial.z + cantidadRotacion,
                                              rotacionLunaInicial.w);
        */
    }

    private void RotacionZNegativa()
    {
        rotacionLunaObjetivo = transform.rotation * Quaternion.Euler(0.0f, 0.0f, -cantidadRotacionZ);
        /*
        rotacionLunaObjetivo = new Quaternion(rotacionLunaInicial.x,
                                              rotacionLunaInicial.y,
                                              rotacionLunaInicial.z - cantidadRotacion,
                                              rotacionLunaInicial.w);
        */
    }

    private void InicializarVariables()
    {
        tiempoActual = 0.0f;
        duracionRotacion = 2.5f;
        cantidadRotacionY = 15.0f;
        cantidadRotacionZ = 5.0f;
        rotando = false;

        rotacionLunaInicial = new Quaternion(transform.rotation.x,
                                             transform.rotation.y,
                                             transform.rotation.z,
                                             transform.rotation.w);
    }
}
