using PixelCrushers.DialogueSystem;
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

        // Nos suscribimos al evento
        GameManager.OnGameStateChanged += NuevoEstadoDeJuego;
    }

    private void OnDestroy()
    {
        GameManager.OnGameStateChanged -= NuevoEstadoDeJuego;
    }

    void OnEnable()
    {
        Lua.RegisterFunction(nameof(NuevoObjetoRecogido), this, SymbolExtensions.GetMethodInfo(() => NuevoObjetoRecogido()));
    }

    // Cuando el controlador deje de estar disponible, se desregistran las funciones
    void OnDisable()
    {
        // Note: If this script is on your Dialogue Manager & the Dialogue Manager is configured
        // as Don't Destroy On Load (on by default), don't unregister Lua functions.
        Lua.UnregisterFunction(nameof(NuevoObjetoRecogido)); // <-- Only if not on Dialogue Manager.
    }

    private void Start()
    {
        botonInventarioGO = GameObject.Find(Constantes.NOMBRE_BOTON_INV_GO);
        notificacionInventarioGO = GameObject.Find(Constantes.NOMBRE_NOTIFICACION_INV_GO);
    }

    private void NuevoEstadoDeJuego(GameState nuevoEstado, GameState anteriorEstado)
    {
        if (nuevoEstado == GameState.Inventario && anteriorEstado == GameState.Conduciendo)
        {
            if(moverBotonInventarioRoutine != null)
            {
                StopCoroutine(moverBotonInventarioRoutine);
            }
            moverBotonInventarioRoutine = OcultarBotonInventarioCoroutine();
            StartCoroutine(moverBotonInventarioRoutine);
        }

        else if (nuevoEstado == GameState.Dialogo && anteriorEstado == GameState.Conduciendo)
        {
            if (moverBotonInventarioRoutine != null)
            {
                StopCoroutine(moverBotonInventarioRoutine);
            }
            moverBotonInventarioRoutine = OcultarBotonInventarioCoroutine();
            StartCoroutine(moverBotonInventarioRoutine);
        }

        else if (nuevoEstado == GameState.Conduciendo && anteriorEstado == GameState.Inventario)
        {
            if (moverBotonInventarioRoutine != null)
            {
                StopCoroutine(moverBotonInventarioRoutine);
            }
            moverBotonInventarioRoutine = MostrarBotonInventarioCoroutine();
            StartCoroutine(moverBotonInventarioRoutine);
        }

        else if (nuevoEstado == GameState.Conduciendo && anteriorEstado == GameState.Dialogo)
        {
            if (moverBotonInventarioRoutine != null)
            {
                StopCoroutine(moverBotonInventarioRoutine);
            }
            moverBotonInventarioRoutine = MostrarBotonInventarioCoroutine();
            StartCoroutine(moverBotonInventarioRoutine);
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

    public void NuevoObjetoRecogido()
    {
        notificacionInventarioGO.GetComponentInChildren<TextMeshProUGUI>().text = Constantes.TEXTO_NOTIFICACION_OBJETO_RECOGIDO;

        if (moverNotificacionInventarioRoutine != null)
        {
            StopCoroutine(moverNotificacionInventarioRoutine);
        }

        moverNotificacionInventarioRoutine = MostrarYOcultarNotificacionInventarioCoroutine();
        StartCoroutine(moverNotificacionInventarioRoutine);
    }

    private IEnumerator MostrarYOcultarNotificacionInventarioCoroutine()
    {
        float posLocalActualNotificacionInv_Y = notificacionInventarioGO.transform.localPosition.y;
        float posFinalNotificacionInv_Y = Constantes.POSICION_NOTIFICACION_INV_MOSTRAR.y;
        float temporizador = 0.0f;

        while (posLocalActualNotificacionInv_Y < posFinalNotificacionInv_Y)
        {
            posLocalActualNotificacionInv_Y += Constantes.CANTIDAD_MOVIMIENTO_NOTIFICACION_INV;
            notificacionInventarioGO.transform.localPosition = new Vector3(notificacionInventarioGO.transform.localPosition.x,
                                                                           posLocalActualNotificacionInv_Y,
                                                                           notificacionInventarioGO.transform.localPosition.z);
            yield return new WaitForFixedUpdate();
        }

        while (temporizador < Constantes.CANTIDAD_TIEMPO_NOTIFICACION_INV)
        {
            temporizador += Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }

        posFinalNotificacionInv_Y = Constantes.POSICION_NOTIFICACION_INV_OCULTAR.y;

        while (posLocalActualNotificacionInv_Y > posFinalNotificacionInv_Y)
        {
            posLocalActualNotificacionInv_Y -= Constantes.CANTIDAD_MOVIMIENTO_NOTIFICACION_INV;
            notificacionInventarioGO.transform.localPosition = new Vector3(notificacionInventarioGO.transform.localPosition.x,
                                                                           posLocalActualNotificacionInv_Y,
                                                                           notificacionInventarioGO.transform.localPosition.z);
            yield return new WaitForFixedUpdate();
        }
    }
}