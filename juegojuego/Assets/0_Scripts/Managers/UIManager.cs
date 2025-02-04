using System.Collections;
using TMPro;
using UnityEngine;
using Constantes;
using System;
using PixelCrushers.DialogueSystem;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    private GameObject botonInventarioGO;
    private GameObject notificacionInventarioGO;

    private IEnumerator moverBotonInventarioRoutine;
    private IEnumerator moverNotificacionInventarioRoutine;

    private string objetoRecogidoLocalizado;
    private string objetoSoltadoLocalizado;
    private string objetoFocuseadoLocalizado;
    private string inventarioLlenoLocalizado;
    

    private void Awake()
    {
        //HacerloInmortal();

        Instance = this;
    }

    private void OnDestroy()
    {
        DesuscribirseEventos();
    }

    private void Start()
    {
        BuscarGO();

        SuscribirseEventos();

        LocalizarMensajes();
    }

    private void LocalizarMensajes()
    {
        if(Localization.language == "en"){
            objetoRecogidoLocalizado = Constantes.Inventario.UI.TEXTO_NOTIFICACION_OBJETO_RECOGIDO_EN;
            objetoSoltadoLocalizado = Constantes.Inventario.UI.TEXTO_NOTIFICACION_OBJETO_SOLTADO_EN;
            objetoFocuseadoLocalizado = Constantes.Inventario.UI.TEXTO_NOTIFICACION_OBJ_FOCUSEADO_EN;
            inventarioLlenoLocalizado = Constantes.Inventario.UI.TEXTO_NOTIFICACION_INV_LLENO_EN;
        }
        else{
            objetoRecogidoLocalizado = Constantes.Inventario.UI.TEXTO_NOTIFICACION_OBJETO_RECOGIDO;
            objetoSoltadoLocalizado = Constantes.Inventario.UI.TEXTO_NOTIFICACION_OBJETO_SOLTADO;
            objetoFocuseadoLocalizado = Constantes.Inventario.UI.TEXTO_NOTIFICACION_OBJ_FOCUSEADO;
            inventarioLlenoLocalizado = Constantes.Inventario.UI.TEXTO_NOTIFICACION_INV_LLENO;
        }
    }

    private void GameManager_OnGameStateChanged(GameState nuevoEstado, GameState anteriorEstado)
    {
        if ((nuevoEstado == GameState.Inventario || nuevoEstado == GameState.Dialogo) &&
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

        else if(nuevoEstado == GameState.MenuInicio || nuevoEstado == GameState.FinJuego)
        {
            OcultarBotonInventario();
        }
    }

    private void InventarioManager_OnInventarioChanged(InventarioMensajes mensaje)
    {
        switch (mensaje)
        {
            case InventarioMensajes.ObjetoAgregado:
                EjecutarNotificacion(objetoRecogidoLocalizado, MostrarYOcultarNotificacionInventarioCoroutine());
                break;
            case InventarioMensajes.ObjetoSoltado:
                EjecutarNotificacion(objetoSoltadoLocalizado, MostrarYOcultarNotificacionInventarioCoroutine());
                break;
            case InventarioMensajes.ObjetoFocuseado:
                EjecutarNotificacion(objetoFocuseadoLocalizado, MostrarNotificacionInventarioCoroutine());
                break;
            case InventarioMensajes.InventarioLleno:
                EjecutarNotificacion(inventarioLlenoLocalizado, MostrarYOcultarNotificacionInventarioCoroutine());
                break;
            default:
                break;
        }
    }

    private IEnumerator MostrarBotonInventarioCoroutine()
    {
        float posLocalActualBotonInv_Y = botonInventarioGO.transform.localPosition.y;
        float posFinalBotonInv_Y = Inventario.UI.POSICION_BOTON_MOSTRAR.y;

        while (posLocalActualBotonInv_Y < posFinalBotonInv_Y)
        {
            posLocalActualBotonInv_Y += Inventario.UI.CANTIDAD_MOVIMIENTO_BOTON;
            botonInventarioGO.transform.localPosition = new Vector3(botonInventarioGO.transform.localPosition.x,
                                                             posLocalActualBotonInv_Y,
                                                             botonInventarioGO.transform.localPosition.z);
            yield return new WaitForFixedUpdate();
        }
    }

    private IEnumerator OcultarBotonInventarioCoroutine()
    {
        float posLocalActualBotonInv_Y = botonInventarioGO.transform.localPosition.y;
        float posFinalBotonInv_Y = Inventario.UI.POSICION_BOTON_OCULTAR.y;

        while (posLocalActualBotonInv_Y > posFinalBotonInv_Y)
        {
            posLocalActualBotonInv_Y -= Inventario.UI.CANTIDAD_MOVIMIENTO_BOTON;
            botonInventarioGO.transform.localPosition = new Vector3(botonInventarioGO.transform.localPosition.x,
                                                             posLocalActualBotonInv_Y,
                                                             botonInventarioGO.transform.localPosition.z);
            yield return new WaitForFixedUpdate();
        }
    }

    private void OcultarBotonInventario()
    {
        botonInventarioGO.transform.localPosition = new Vector3(botonInventarioGO.transform.localPosition.x,
                                                                Inventario.UI.POSICION_BOTON_OCULTAR.y,
                                                                botonInventarioGO.transform.localPosition.z);
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
        float posFinalNotificacionInv_Y = Inventario.UI.POSICION_NOTIFICACION_MOSTRAR.y;

        while (posLocalActualNotificacionInv_Y < posFinalNotificacionInv_Y)
        {
            posLocalActualNotificacionInv_Y += Inventario.UI.CANTIDAD_MOVIMIENTO_NOTIFICACION;
            notificacionInventarioGO.transform.localPosition = new Vector3(notificacionInventarioGO.transform.localPosition.x,
                                                                           posLocalActualNotificacionInv_Y,
                                                                           notificacionInventarioGO.transform.localPosition.z);
            yield return new WaitForFixedUpdate();
        }
    }

    private IEnumerator OcultarNotificacionInventarioCoroutine()
    {
        float posLocalActualNotificacionInv_Y = notificacionInventarioGO.transform.localPosition.y;
        float posFinalNotificacionInv_Y = Inventario.UI.POSICION_NOTIFICACION_OCULTAR.y;

        while (posLocalActualNotificacionInv_Y > posFinalNotificacionInv_Y)
        {
            posLocalActualNotificacionInv_Y -= Inventario.UI.CANTIDAD_MOVIMIENTO_NOTIFICACION;
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
         * Un cacho de c�digo
         * ...
         * yield return a
         * Podr�a llamar a otra corrutina para que se vaya haciendo, mientras yo voy haciendo cosas aqu� y esperar a que acabe a despu�s.
         * Sin embargo, como en este caso necesitamos que la corrutina acabe (porque tiene que subir el panel) para esperar y luego bajarlo,
         * hay que hacerlo esperando.
         */
        
        // Espera a que MostrarNotificacionInventarioCoroutine acabe. Si simplemente llamaras a la funcion,
        // se har�a la funci�n en un frame, y no se buclar�a como hacen las corrutinas.
        yield return StartCoroutine(MostrarNotificacionInventarioCoroutine());

        float temporizador = 0.0f;

        while (temporizador < Inventario.UI.CANTIDAD_TIEMPO_NOTIFICACION)
        {
            temporizador += Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }

        yield return StartCoroutine(OcultarNotificacionInventarioCoroutine());
    }

    private void HacerloInmortal()
    {
        if (Instance == null)
        {
            DontDestroyOnLoad(gameObject);
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    private void SuscribirseEventos()
    {
        // Nos suscribimos a los eventos
        GameManager.OnGameStateChanged += GameManager_OnGameStateChanged;
        InventarioManager.OnInventarioChanged += InventarioManager_OnInventarioChanged;
    }

    private void DesuscribirseEventos()
    {
        GameManager.OnGameStateChanged -= GameManager_OnGameStateChanged;
        InventarioManager.OnInventarioChanged -= InventarioManager_OnInventarioChanged;
    }

    private void BuscarGO()
    {
        botonInventarioGO = GameObject.FindGameObjectWithTag(Inventario.UI.TAG_ICONO_INVENTARIO);
        notificacionInventarioGO = GameObject.FindGameObjectWithTag(Inventario.UI.TAG_NOTIFICACION_INVENTARIO);
    }
}