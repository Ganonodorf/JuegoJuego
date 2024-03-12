using PixelCrushers.DialogueSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    private GameObject botonInventarioGO;
    private GameObject notificacionInventarioGO;

    private IEnumerator moverBotonInventarioRoutine;
    private IEnumerator moverNotificacionInventarioRoutine;

    private void Awake()
    {
        if(Instance == null)
        {
            DontDestroyOnLoad(gameObject);
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }

        // Nos suscribimos a los eventos
        GameManager.OnGameStateChanged += GameManager_OnGameStateChanged;
        InventarioManager.OnInventarioChanged += InventarioManager_OnInventarioChanged;
    }

    private void OnDestroy()
    {
        GameManager.OnGameStateChanged -= GameManager_OnGameStateChanged;
        InventarioManager.OnInventarioChanged -= InventarioManager_OnInventarioChanged;
    }

    private void Start()
    {
        botonInventarioGO = GameObject.Find(Constantes.NOMBRE_BOTON_INV_GO);
        notificacionInventarioGO = GameObject.Find(Constantes.NOMBRE_NOTIFICACION_INV_GO);
    }

    private void GameManager_OnGameStateChanged(GameState nuevoEstado, GameState anteriorEstado)
    {
        if((nuevoEstado == GameState.Inventario || nuevoEstado == GameState.Dialogo) &&
            anteriorEstado == GameState.Conduciendo)
        {
            if(moverBotonInventarioRoutine != null)
            {
                StopCoroutine(moverBotonInventarioRoutine);
            }
            moverBotonInventarioRoutine = OcultarBotonInventarioCoroutine();
            StartCoroutine(moverBotonInventarioRoutine);
        }

        else if (nuevoEstado == GameState.Conduciendo &&
                (anteriorEstado == GameState.Inventario || anteriorEstado == GameState.Dialogo))
        {
            if (moverBotonInventarioRoutine != null)
            {
                StopCoroutine(moverBotonInventarioRoutine);
            }
            moverBotonInventarioRoutine = MostrarBotonInventarioCoroutine();
            StartCoroutine(moverBotonInventarioRoutine);

            EjecutarNotificacion(string.Empty, OcultarNotificacionInventarioCoroutine());
        }
    }

    private void InventarioManager_OnInventarioChanged(InventarioMensajes mensaje)
    {
        switch (mensaje)
        {
            case InventarioMensajes.ObjetoAgregado:
                EjecutarNotificacion(Constantes.TEXTO_NOTIFICACION_OBJETO_RECOGIDO, MostrarYOcultarNotificacionInventarioCoroutine());
                break;
            case InventarioMensajes.ObjetoSoltado:
                EjecutarNotificacion(Constantes.TEXTO_NOTIFICACION_OBJETO_SOLTADO, MostrarYOcultarNotificacionInventarioCoroutine());
                break;
            case InventarioMensajes.ObjetoFocuseado:
                EjecutarNotificacion(Constantes.TEXTO_NOTIFICACION_OBJ_FOCUSEADO, MostrarNotificacionInventarioCoroutine());
                break;
            case InventarioMensajes.InventarioLleno:
                EjecutarNotificacion(Constantes.TEXTO_NOTIFICACION_INV_LLENO, MostrarYOcultarNotificacionInventarioCoroutine());
                break;
            default:
                break;
        }
    }

    private IEnumerator MostrarBotonInventarioCoroutine()
    {
        float posLocalActualBotonInv_Y = botonInventarioGO.transform.localPosition.y;
        float posFinalBotonInv_Y = Constantes.POSICION_BOTON_INV_MOSTRAR.y;

        while (posLocalActualBotonInv_Y < posFinalBotonInv_Y)
        {
            posLocalActualBotonInv_Y += Constantes.CANTIDAD_MOVIMIENTO_BOTON_INV;
            botonInventarioGO.transform.localPosition = new Vector3(botonInventarioGO.transform.localPosition.x,
                                                             posLocalActualBotonInv_Y,
                                                             botonInventarioGO.transform.localPosition.z);
            yield return new WaitForFixedUpdate();
        }
    }

    private IEnumerator OcultarBotonInventarioCoroutine()
    {
        float posLocalActualBotonInv_Y = botonInventarioGO.transform.localPosition.y;
        float posFinalBotonInv_Y = Constantes.POSICION_BOTON_INV_OCULTAR.y;

        while (posLocalActualBotonInv_Y > posFinalBotonInv_Y)
        {
            posLocalActualBotonInv_Y -= Constantes.CANTIDAD_MOVIMIENTO_BOTON_INV;
            botonInventarioGO.transform.localPosition = new Vector3(botonInventarioGO.transform.localPosition.x,
                                                             posLocalActualBotonInv_Y,
                                                             botonInventarioGO.transform.localPosition.z);
            yield return new WaitForFixedUpdate();
        }
    }
    
    private void EjecutarNotificacion(string mensaje, IEnumerator accionAEjecutar)
    {
        notificacionInventarioGO.GetComponentInChildren<TextMeshProUGUI>().text = mensaje;

        if (moverNotificacionInventarioRoutine != null)
        {
            StopCoroutine(moverNotificacionInventarioRoutine);
        }

        moverNotificacionInventarioRoutine = accionAEjecutar;
        StartCoroutine(moverNotificacionInventarioRoutine);
    }

    private IEnumerator MostrarNotificacionInventarioCoroutine()
    {
        float posLocalActualNotificacionInv_Y = notificacionInventarioGO.transform.localPosition.y;
        float posFinalNotificacionInv_Y = Constantes.POSICION_NOTIFICACION_INV_MOSTRAR.y;

        while (posLocalActualNotificacionInv_Y < posFinalNotificacionInv_Y)
        {
            posLocalActualNotificacionInv_Y += Constantes.CANTIDAD_MOVIMIENTO_NOTIFICACION_INV;
            notificacionInventarioGO.transform.localPosition = new Vector3(notificacionInventarioGO.transform.localPosition.x,
                                                                           posLocalActualNotificacionInv_Y,
                                                                           notificacionInventarioGO.transform.localPosition.z);
            yield return new WaitForFixedUpdate();
        }
    }

    private IEnumerator OcultarNotificacionInventarioCoroutine()
    {
        float posLocalActualNotificacionInv_Y = notificacionInventarioGO.transform.localPosition.y;
        float posFinalNotificacionInv_Y = Constantes.POSICION_NOTIFICACION_INV_OCULTAR.y;

        while (posLocalActualNotificacionInv_Y > posFinalNotificacionInv_Y)
        {
            posLocalActualNotificacionInv_Y -= Constantes.CANTIDAD_MOVIMIENTO_NOTIFICACION_INV;
            notificacionInventarioGO.transform.localPosition = new Vector3(notificacionInventarioGO.transform.localPosition.x,
                                                                           posLocalActualNotificacionInv_Y,
                                                                           notificacionInventarioGO.transform.localPosition.z);
            yield return new WaitForFixedUpdate();
        }
    }

    private IEnumerator MostrarYOcultarNotificacionInventarioCoroutine()
    {
        /* Ejemplo de corrutinas paralelas
         * Si por ejemplo pusiera:
         * Coroutine a = StartCoroutine(MostrarNotificacionInventarioCoroutine);
         * ...
         * Un cacho de código
         * ...
         * yield return a
         * Podría llamar a otra corrutina para que se vaya haciendo, mientras yo voy haciendo cosas aquí y esperar a que acabe a después.
         * Sin embargo, como en este caso necesitamos que la corrutina acabe (porque tiene que subir el panel) para esperar y luego bajarlo,
         * hay que hacerlo esperando.
         */
        
        // Espera a que MostrarNotificacionInventarioCoroutine acabe. Si simplemente llamaras a la funcion,
        // se haría la función en un frame, y no se buclaría como hacen las corrutinas.
        yield return StartCoroutine(MostrarNotificacionInventarioCoroutine());

        float temporizador = 0.0f;

        while (temporizador < Constantes.CANTIDAD_TIEMPO_NOTIFICACION_INV)
        {
            temporizador += Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }

        yield return StartCoroutine(OcultarNotificacionInventarioCoroutine());
    }
}