using Cinemachine;
using PixelCrushers.DialogueSystem;
using System.Collections.Generic;
using UnityEngine;

public class InventarioManager : MonoBehaviour
{
    // Variables públicas
    public static InventarioManager Instance;

    // Variables privadas
    private List<GameObject> inventario;
    private GameObject jugador;
    private GameObject camaraInventario;

    private void Awake()
    {
        if(Instance == null)
        {
            DontDestroyOnLoad(gameObject);
            Instance = this;
        }
        else if(Instance != this)
        {
            Destroy(gameObject);
        }
    }
    void OnEnable()
    {
        Lua.RegisterFunction(nameof(AgregarAlInventario), this, SymbolExtensions.GetMethodInfo(() => AgregarAlInventario(string.Empty)));
    }

    // Cuando el controlador deje de estar disponible, se desregistran las funciones
    void OnDisable()
    {
        // Note: If this script is on your Dialogue Manager & the Dialogue Manager is configured
        // as Don't Destroy On Load (on by default), don't unregister Lua functions.
        Lua.UnregisterFunction(nameof(AgregarAlInventario)); // <-- Only if not on Dialogue Manager.
    }

    private void Start()
    {
        inventario = new List<GameObject>();
        jugador = GameObject.Find(Constantes.NOMBRE_PLAYER_GO);
        camaraInventario = GameObject.Find(Constantes.NOMBRE_CAMARA_INV_GO);
    }

    private void Update()
    {
        if(GameManager.Instance.GetGameState() == GameState.Playing && Input.GetKeyDown(KeyCode.I))
        {
            AbrirInventario();
        }
        else if (GameManager.Instance.GetGameState() == GameState.Inventario && Input.GetKeyDown(KeyCode.I))
        {
            CerrarInventario();
        }
    }

    public List<GameObject> getInventario()
    {
        return inventario;
    }

    public void AgregarAlInventario(string nombreObjetoAgregar)
    {
        GameObject objetoAgregar = GameObject.Find(nombreObjetoAgregar);

        int cantidadObjetos = inventario.Count;

        if (cantidadObjetos >= Constantes.CAPACIDAD_INVENTARIO)
        {
            Debug.Log("No se pueden agregar más objetos");
            return;
        }

        // Añade el objeto a la lista del inventario
        inventario.Add(objetoAgregar);

        // Le cambia el padre para que sea el coche
        objetoAgregar.transform.SetParent(jugador.transform);
        //objetoAgregar.transform.parent = jugador.transform;

        // Inabilita los colliders del objeto
        objetoAgregar.transform.GetComponent<BoxCollider>().enabled = false;

        // Lo hace kinematic para que no se mueva
        objetoAgregar.transform.GetComponent<Rigidbody>().isKinematic = true;

        // Reduce el objeto a la mitad
        objetoAgregar.transform.localScale = new Vector3(objetoAgregar.transform.localScale.x / Constantes.ESCALA_REDUCCION,
                                                         objetoAgregar.transform.localScale.y / Constantes.ESCALA_REDUCCION,
                                                         objetoAgregar.transform.localScale.z / Constantes.ESCALA_REDUCCION);

        // Lo pone en la posición que le toque
        objetoAgregar.transform.localPosition = Constantes.POSICIONES_INVENTARIO[cantidadObjetos];

        // Lo pone una rotación estándar para todos
        objetoAgregar.transform.rotation = new Quaternion(0.0f,
                                                          0.0f,
                                                          0.0f,
                                                          0.0f);

        // Hace que no sea usable para no poder accionar el dialog system
        objetoAgregar.transform.GetComponent<Usable>().enabled = false;
    }

    private void AbrirInventario()
    {
        GameManager.Instance.UpdateGameState(GameState.Inventario);
        camaraInventario.GetComponent<CinemachineVirtualCamera>().enabled = true;
    }

    private void CerrarInventario()
    {
        GameManager.Instance.UpdateGameState(GameState.Playing);
        camaraInventario.GetComponent<CinemachineVirtualCamera>().enabled = false;
    }
}

public enum InventarioState
{
    ConHueco,
    Lleno
}