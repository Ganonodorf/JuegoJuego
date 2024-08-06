using Cinemachine;
using PixelCrushers.DialogueSystem;
using System;
using System.Collections.Generic;
using UnityEngine;
using Constantes;

public class InventarioManager : MonoBehaviour
{
    // Variables p?blicas
    public static InventarioManager Instance;
    public static event Action<InventarioMensajes> OnInventarioChanged;

    // Variables privadas
    private List<GameObject> inventario;
    private GameObject jugador;
    private GameObject camaraInventario;
    private int indexObjetoResaltado;

    // M?todo usado para obtener el inventario
    public List<GameObject> GetInventario()
    {
        return inventario;
    }

    private void Awake()
    {
        HacerloInmortal();
    }

    void OnEnable()
    {
        // Cuando el controlador est? disponible, se registran en c?digo Lua las funciones que se van a llamar desde el dialog system
        RegistrarFuncionesLua();
    }

    void OnDisable()
    {
        // Cuando el controlador deje de estar disponible, se desregistran las funciones
        DesregistrarFuncionesLua();
    }

    private void Start()
    {
        InicializarVariables();
        BuscarGO();

        GestionarInputs();
    }

    private void GestionarInputs()
    {
        InputManager.Instance.controles.Conduciendo.AbrirInventario.performed += contexto => AbrirInventario();
        InputManager.Instance.controles.Inventario.CerrarInventario.performed += contexto => CerrarInventario();
        InputManager.Instance.controles.Inventario.MovimientoDer.performed += contexto => { if (inventario.Count > 0) NavegarInventarioDer(); };
        InputManager.Instance.controles.Inventario.MovimientoIzq.performed += contexto => { if (inventario.Count > 0) NavegarInventarioIzq(); };
        InputManager.Instance.controles.Inventario.MovimientoArriba.performed += contexto => { if (inventario.Count > 3) NavegarInventarioArriba(); };
        InputManager.Instance.controles.Inventario.MovimientoAbajo.performed += contexto => { if (inventario.Count > 3) NavegarInventarioAbajo(); };
        InputManager.Instance.controles.Inventario.SoltarDelInventario.performed += contexto => SoltarDelInventario();
    }

    private void RegistrarFuncionesLua()
    {
        Lua.RegisterFunction(nameof(AgregarAlInventario), this, SymbolExtensions.GetMethodInfo(() => AgregarAlInventario(string.Empty)));
        Lua.RegisterFunction(nameof(EntregarObjeto), this, SymbolExtensions.GetMethodInfo(() => EntregarObjeto(string.Empty, string.Empty)));
    }

    private void DesregistrarFuncionesLua()
    {
        // Note: If this script is on your Dialogue Manager & the Dialogue Manager is configured
        // as Don't Destroy On Load (on by default), don't unregister Lua functions.
        Lua.UnregisterFunction(nameof(AgregarAlInventario)); // <-- Only if not on Dialogue Manager.
        Lua.UnregisterFunction(nameof(EntregarObjeto)); // <-- Only if not on Dialogue Manager.
    }

    private void NavegarInventarioDer()
    {
        int indexSiguienteObjeto;

        indexSiguienteObjeto = indexObjetoResaltado + 1;

        if (indexSiguienteObjeto >= inventario.Count || inventario[indexSiguienteObjeto] == null)
        {
            indexSiguienteObjeto = 0;
        }

        DesIluminarObjeto(inventario[indexObjetoResaltado]);

        FocusearObjeto(inventario[indexSiguienteObjeto]);

        indexObjetoResaltado = indexSiguienteObjeto;
    }

    private void NavegarInventarioIzq()
    {
        int indexSiguienteObjeto;

        indexSiguienteObjeto = indexObjetoResaltado - 1;

        if (indexSiguienteObjeto < 0)
        {
            indexSiguienteObjeto = inventario.Count - 1;
        }

        DesIluminarObjeto(inventario[indexObjetoResaltado]);

        FocusearObjeto(inventario[indexSiguienteObjeto]);

        indexObjetoResaltado = indexSiguienteObjeto;
    }

    private void NavegarInventarioArriba()
    {
        int indexSiguienteObjeto;

        if ((indexObjetoResaltado == 0 || indexObjetoResaltado == 1 || indexObjetoResaltado == 2) &&
            inventario.Count >= 5 && indexObjetoResaltado == 2)
        {
            indexSiguienteObjeto = 4;
        }
        else
        {
            indexSiguienteObjeto = 3;
        }

        DesIluminarObjeto(inventario[indexObjetoResaltado]);

        FocusearObjeto(inventario[indexSiguienteObjeto]);

        indexObjetoResaltado = indexSiguienteObjeto;
    }

    private void NavegarInventarioAbajo()
    {
        int indexSiguienteObjeto;

        if ((indexObjetoResaltado == 3 || indexObjetoResaltado == 4) &&
            indexObjetoResaltado == 3)
        {
            indexSiguienteObjeto = 0;
        }
        else
        {
            indexSiguienteObjeto = 2;
        }

        DesIluminarObjeto(inventario[indexObjetoResaltado]);

        FocusearObjeto(inventario[indexSiguienteObjeto]);

        indexObjetoResaltado = indexSiguienteObjeto;
    }

    public void AgregarAlInventario(string nombreObjetoAgregar)
    {
        if(inventario.Count < Inventario.Manager.CAPACIDAD)
        {
            GameObject objetoAgregar = GameObject.Find(nombreObjetoAgregar);

            MeterObjetoAlCoche(objetoAgregar);

            // A?ade el objeto a la lista del inventario
            inventario.Add(objetoAgregar);

            // Pone la Variable del objeto en true <---<<
            DialogueLua.SetVariable(nombreObjetoAgregar, true);

            // Lanza el evento
            OnInventarioChanged?.Invoke(InventarioMensajes.ObjetoAgregado);

            // Hace que deje de ser usable y triggereable para que no se vuelva a recoger
            objetoAgregar.transform.GetComponent<Usable>().enabled = true;
            objetoAgregar.transform.GetComponent<DialogueSystemTrigger>().enabled = true;

            
        }
        else
        {
            // Lanza el evento
            OnInventarioChanged?.Invoke(InventarioMensajes.InventarioLleno);
        }
    }

    private void MeterObjetoAlCoche(GameObject objetoAgregar)
    {
        // Le cambia el padre para que sea el coche
        objetoAgregar.transform.SetParent(jugador.transform);

        // Inabilita los colliders del objeto

        Collider[] collidersObjetos;

        collidersObjetos = objetoAgregar.GetComponents<Collider>();

        foreach (Collider collider in collidersObjetos)
        {
            collider.enabled = false;
        }

        Debug.Log(objetoAgregar.transform.Find("groundCheck"));

        // Inabilita el groundCheck
        if (objetoAgregar.transform.Find("groundCheck") != null)
        {
            objetoAgregar.transform.Find("groundCheck").gameObject.SetActive(false);
        }
        

        //Inabilita la animación del objeto
        if (objetoAgregar.TryGetComponent<Animator>(out Animator animator))
        {
            animator.enabled = false;
        }

        if (objetoAgregar.TryGetComponent<Animation>(out Animation animation))
        {
            animation.enabled = false;
        }
     
        //Inabilita las luces
        if (objetoAgregar.transform.Find("luzObjeto") != null)
        {
            objetoAgregar.transform.Find("luzObjeto").gameObject.SetActive(false);
        }
        
        

        // Lo hace kinematic para que no se mueva
        objetoAgregar.transform.GetComponent<Rigidbody>().isKinematic = true;

        // Reduce el objeto a la mitad
        objetoAgregar.transform.localScale = new Vector3(objetoAgregar.transform.localScale.x / Inventario.Manager.ESCALA_REDUCCION,
                                                         objetoAgregar.transform.localScale.y / Inventario.Manager.ESCALA_REDUCCION,
                                                         objetoAgregar.transform.localScale.z / Inventario.Manager.ESCALA_REDUCCION);

        // Lo pone en la posici?n que le toque
        objetoAgregar.transform.localPosition = Inventario.Manager.POSICIONES[inventario.Count];

        // Lo pone una rotaci?n est?ndar para todos
        objetoAgregar.transform.rotation = new Quaternion(0.0f,
                                                          0.0f,
                                                          0.0f,
                                                          0.0f);

        // Hace que no sea usable para no poder accionar el dialog system
        objetoAgregar.transform.GetComponent<Usable>().enabled = false;
    }

    public void SoltarDelInventario()
    {
        if(inventario.Count <= 0)
        {
            return;
        }

        // Recoge ese GameObject del inventario
        GameObject objetoSoltar = inventario[indexObjetoResaltado];

        SacarObjetoDelCoche(objetoSoltar);

        DesIluminarObjeto(objetoSoltar);

        inventario.Remove(objetoSoltar);

        ReordenarInventario();

        // Recoge el nombre del objeto y pone su Variable en false <---<<
        string nombreObjetoSoltar = objetoSoltar.name;
        DialogueLua.SetVariable(nombreObjetoSoltar, false);

        // Lanza el evento
        OnInventarioChanged?.Invoke(InventarioMensajes.ObjetoSoltado);
    }

    public void EntregarObjeto(string nombreObjetoEntregar, string nombrePersonajeReceptor)
    {
        // Encuentra ese GameObject
        GameObject objetoEntregar = GameObject.Find(nombreObjetoEntregar);

        // Lo saca del inventario
        inventario.Remove(objetoEntregar);
        ReordenarInventario();

        // Desactiva el Look At de la cámara del inventario
        camaraInventario.GetComponent<CinemachineVirtualCamera>().LookAt = null;

        // Pone su Variable en false
        DialogueLua.SetVariable(nombreObjetoEntregar, false);

        // Lanza el evento ---?
        OnInventarioChanged?.Invoke(InventarioMensajes.ObjetoSoltado);

        // Encontrar el personaje receptor
        GameObject personajeReceptor = GameObject.Find(nombrePersonajeReceptor);

        // Encontrar el punto receptor adecuado
        string rutaPuntoReceptor = "puntosReceptores/punto" + nombreObjetoEntregar;
        Transform puntoReceptor = personajeReceptor.transform.Find(rutaPuntoReceptor);

        //Hacer objeto hijo de ese punto y colocarlo allí
        objetoEntregar.transform.SetParent(puntoReceptor);
        objetoEntregar.transform.localPosition = new Vector3(0, 0, 0);


        // Ampl?a el objeto al doble
        objetoEntregar.transform.localScale = new Vector3(objetoEntregar.transform.localScale.x * Inventario.Manager.ESCALA_REDUCCION,
                                                          objetoEntregar.transform.localScale.y * Inventario.Manager.ESCALA_REDUCCION,
                                                          objetoEntregar.transform.localScale.z * Inventario.Manager.ESCALA_REDUCCION);

        // Hace que deje de ser usable y triggereable para que no se vuelva a recoger
        objetoEntregar.transform.GetComponent<Usable>().enabled = false;
        objetoEntregar.transform.GetComponent<DialogueSystemTrigger>().enabled = false;

        // Kinematiquiza el rigidbody para que se quede quieto
        objetoEntregar.transform.GetComponent<Rigidbody>().isKinematic = true;
    }

    private void SacarObjetoDelCoche(GameObject objetoSoltar)
    {
        // Hace que sea usable para poder accionar el dialog system
        objetoSoltar.transform.GetComponent<Usable>().enabled = true;

        // La rotacion no se la toca

        // Lo pone delante del coche
        objetoSoltar.transform.localPosition = new Vector3(0.0f,
                                                           Inventario.Manager.LONGITUD_COCHE + 3.0f,
                                                           Inventario.Manager.LONGITUD_COCHE);

        // Ampl?a el objeto al doble
        objetoSoltar.transform.localScale = new Vector3(objetoSoltar.transform.localScale.x * Inventario.Manager.ESCALA_REDUCCION,
                                                        objetoSoltar.transform.localScale.y * Inventario.Manager.ESCALA_REDUCCION,
                                                        objetoSoltar.transform.localScale.z * Inventario.Manager.ESCALA_REDUCCION);

        // Le quita kinematic para que se mueva
        objetoSoltar.transform.GetComponent<Rigidbody>().isKinematic = false;

        // Habilita los colliders del objeto
        Collider[] collidersObjetos;

        collidersObjetos = objetoSoltar.GetComponents<Collider>();

        foreach (Collider collider in collidersObjetos)
        {
            collider.enabled = true;
        }

        // Abilita el groundCheck que activará las animationces y las luces cuando toque el suelo
        if (objetoSoltar.transform.Find("groundCheck") != null)
        {
            objetoSoltar.transform.Find("groundCheck").gameObject.SetActive(true);
        }

        // Le cambia el padre para que sea el mundo
        objetoSoltar.transform.SetParent(null);
    }

    private void AbrirInventario()
    {
        GameManager.Instance.UpdateGameState(GameState.Inventario);

        camaraInventario.GetComponent<CinemachineVirtualCamera>().enabled = true;

        // Para que no pueda interactuar con cosas del Dialogue System
        if(jugador.TryGetComponent(out ProximitySelector proximitySelector))
        {
            proximitySelector.enabled = false;
        }

        if (inventario.Count > 0)
        {
            FocusearObjeto(inventario[indexObjetoResaltado]);
        }
    }

    private void CerrarInventario()
    {
        GameManager.Instance.UpdateGameState(GameState.Conduciendo);
        camaraInventario.GetComponent<CinemachineVirtualCamera>().LookAt = null;
        camaraInventario.GetComponent<CinemachineVirtualCamera>().enabled = false;

        // Para que pueda volver a interactuar con cosas del Dialogue System
        if (jugador.TryGetComponent(out ProximitySelector proximitySelector))
        {
            proximitySelector.enabled = true;
        }

        if (inventario.Count > 0)
        {
            DesIluminarObjeto(inventario[indexObjetoResaltado]);

            indexObjetoResaltado = 0;
        }
    }

    private void ReordenarInventario()
    {
        for (int i = 0; i <= inventario.Count - 1; i++)
        {
            inventario[i].transform.localPosition = Inventario.Manager.POSICIONES[i];
        }

        // Ilumina el primer objeto
        if (inventario.Count > 0)
        {
            indexObjetoResaltado = 0;

            FocusearObjeto(inventario[indexObjetoResaltado]);
        }
        else
        {
            camaraInventario.GetComponent<CinemachineVirtualCamera>().LookAt = null;
        }
    }

    private void FocusearObjeto(GameObject objetoAFocusear)
    {
        MiraAlObjeto(objetoAFocusear);

        IluminarObjeto(objetoAFocusear);

        OnInventarioChanged?.Invoke(InventarioMensajes.ObjetoFocuseado);
    }

    private void MiraAlObjeto(GameObject objetoAMirar)
    {
        camaraInventario.GetComponent<CinemachineVirtualCamera>().LookAt = objetoAMirar.transform;
    }

    private void IluminarObjeto(GameObject objetoAIluminar)
    {
        objetoAIluminar.GetComponent<Renderer>().material.EnableKeyword("_EMISSION");
        
        objetoAIluminar.GetComponent<Renderer>().material.SetColor("_EmissionColor", Inventario.Manager.COLOR_ILUMINADO);
    }

    private void DesIluminarObjeto(GameObject objetoADesIluminar)
    {
        objetoADesIluminar.GetComponent<Renderer>().material.DisableKeyword("_EMISSION");
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

    private void InicializarVariables()
    {
        inventario = new List<GameObject>();
        indexObjetoResaltado = 0;
    }

    private void BuscarGO()
    {
        jugador = GameObject.Find(Player.NOMBRE_GO);
        camaraInventario = GameObject.Find(Inventario.Manager.NOMBRE_CAMARA_GO);
    }
}

public enum InventarioMensajes
{
    ObjetoAgregado,
    ObjetoSoltado,
    ObjetoFocuseado,
    InventarioLleno
}