using Cinemachine;
using PixelCrushers.DialogueSystem;
using System;
using System.Collections.Generic;
using UnityEngine;

public class InventarioManager : MonoBehaviour
{
    // Variables públicas
    public static InventarioManager Instance;

    public static event Action<InventarioMensajes> OnInventarioChanged;

    // Variables privadas
    private List<GameObject> inventario;
    private GameObject jugador;
    private GameObject camaraInventario;
    private int indexObjetoResaltado;

    public List<GameObject> GetInventario()
    {
        return inventario;
    }

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
        indexObjetoResaltado = 0;
        jugador = GameObject.Find(Constantes.NOMBRE_PLAYER_GO);
        camaraInventario = GameObject.Find(Constantes.NOMBRE_CAMARA_INV_GO);
    }

    private void Update()
    {
        if(GameManager.Instance.GetGameState() == GameState.Conduciendo && Input.GetKeyDown(KeyCode.I))
        {
            AbrirInventario();
        }

        else if (GameManager.Instance.GetGameState() == GameState.Inventario)
        {
            if (Input.GetKeyDown(KeyCode.I))
            {
                CerrarInventario();
            }

            else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow) ||
                     Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
            {
                if (inventario.Count > 0)
                {
                    MoverInventarioHoriz();
                }
            }

            else if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow) ||
                     Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
            {
                if (inventario.Count > 3)
                {
                    MoverInvetarioVert();
                }
            }

            else if (Input.GetKeyDown(KeyCode.E))
            {
                if(inventario.Count > 0)
                {
                    SoltarDelInventario();
                }
            }
        }
    }

    public void AgregarAlInventario(string nombreObjetoAgregar)
    {
        GameObject objetoAgregar = GameObject.Find(nombreObjetoAgregar);

        if (inventario.Count >= Constantes.CAPACIDAD_INVENTARIO)
        {
            // Lanza el evento
            OnInventarioChanged?.Invoke(InventarioMensajes.InventarioLleno);

            return;
        }

        // Le cambia el padre para que sea el coche
        objetoAgregar.transform.SetParent(jugador.transform);

        // Inabilita los colliders del objeto
        foreach(Collider collider in objetoAgregar.transform)
        {
            collider.enabled = false;
        }

        // Lo hace kinematic para que no se mueva
        objetoAgregar.transform.GetComponent<Rigidbody>().isKinematic = true;

        // Reduce el objeto a la mitad
        objetoAgregar.transform.localScale = new Vector3(objetoAgregar.transform.localScale.x / Constantes.ESCALA_REDUCCION,
                                                         objetoAgregar.transform.localScale.y / Constantes.ESCALA_REDUCCION,
                                                         objetoAgregar.transform.localScale.z / Constantes.ESCALA_REDUCCION);

        // Lo pone en la posición que le toque
        objetoAgregar.transform.localPosition = Constantes.POSICIONES_INVENTARIO[inventario.Count];

        // Lo pone una rotación estándar para todos
        objetoAgregar.transform.rotation = new Quaternion(0.0f,
                                                          0.0f,
                                                          0.0f,
                                                          0.0f);

        // Hace que no sea usable para no poder accionar el dialog system
        objetoAgregar.transform.GetComponent<Usable>().enabled = false;

        // Añade el objeto a la lista del inventario
        inventario.Add(objetoAgregar);

        // Lanza el evento
        OnInventarioChanged?.Invoke(InventarioMensajes.ObjetoAgregado);
    }

    public void SoltarDelInventario()
    {
        // Recoge ese GameObject del inventario
        GameObject objetoSoltar = inventario[indexObjetoResaltado];

        // Hace que sea usable para poder accionar el dialog system
        objetoSoltar.transform.GetComponent<Usable>().enabled = true;

        // La rotacion no se la toca

        // Lo pone delante del coche
        objetoSoltar.transform.localPosition = new Vector3(0.0f,
                                                           0.0f,
                                                           Constantes.LONGITUD_COCHE);

        // Amplía el objeto al doble
        objetoSoltar.transform.localScale = new Vector3(objetoSoltar.transform.localScale.x * Constantes.ESCALA_REDUCCION,
                                                        objetoSoltar.transform.localScale.y * Constantes.ESCALA_REDUCCION,
                                                        objetoSoltar.transform.localScale.z * Constantes.ESCALA_REDUCCION);

        // Le quita kinematic para que se mueva
        objetoSoltar.transform.GetComponent<Rigidbody>().isKinematic = false;

        // Habilita los colliders del objeto
        foreach (Collider collider in objetoSoltar.transform)
        {
            collider.enabled = true;
        }

        // Le cambia el padre para que sea el mundo
        objetoSoltar.transform.SetParent(null);

        // Desilumina el objeto
        DesIluminarObjeto(objetoSoltar);

        // Lo quita de la lista de inventario
        inventario.Remove(objetoSoltar);

        // Reordenar inventario
        ReordenarInventario();

        // Lanza el evento
        OnInventarioChanged?.Invoke(InventarioMensajes.ObjetoSoltado);
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
    
    private void MoverInventarioHoriz()
    {
        int indexSiguienteObjeto;

        if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            indexSiguienteObjeto = indexObjetoResaltado + 1;

            if (indexSiguienteObjeto >= inventario.Count || inventario[indexSiguienteObjeto] == null)
            {
                indexSiguienteObjeto = 0;
            }
        }
        else
        {
            indexSiguienteObjeto = indexObjetoResaltado - 1;

            if (indexSiguienteObjeto < 0)
            {
                indexSiguienteObjeto = inventario.Count - 1;
            }
        }

        DesIluminarObjeto(inventario[indexObjetoResaltado]);

        FocusearObjeto(inventario[indexSiguienteObjeto]);

        indexObjetoResaltado = indexSiguienteObjeto;
    }

    private void MoverInvetarioVert()
    {
        int indexSiguienteObjeto = 0;

        if ((Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) &&
            (indexObjetoResaltado == 0 || indexObjetoResaltado == 1 || indexObjetoResaltado == 2))
        {
            if (inventario.Count >= 5 && indexObjetoResaltado == 2)
            {
                indexSiguienteObjeto = 4;
            }
            else
            {
                indexSiguienteObjeto = 3;
            }
        }

        if ((Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)) &&
            (indexObjetoResaltado == 3 || indexObjetoResaltado == 4))
        {
            if (indexObjetoResaltado == 3)
            {
                indexSiguienteObjeto = 0;
            }
            else
            {
                indexSiguienteObjeto = 2;
            }
        }

        DesIluminarObjeto(inventario[indexObjetoResaltado]);

        FocusearObjeto(inventario[indexSiguienteObjeto]);

        indexObjetoResaltado = indexSiguienteObjeto;
    }

    private void ReordenarInventario()
    {
        for (int i = 0; i <= inventario.Count - 1; i++)
        {
            inventario[i].transform.localPosition = Constantes.POSICIONES_INVENTARIO[i];
        }

        // Ilumina el primer objeto
        if (inventario.Count > 0)
        {
            indexObjetoResaltado = 0;

            FocusearObjeto(inventario[indexObjetoResaltado]);
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
        
        objetoAIluminar.GetComponent<Renderer>().material.SetColor("_EmissionColor", Constantes.COLOR_ILUMINADO);
    }

    private void DesIluminarObjeto(GameObject objetoADesIluminar)
    {
        objetoADesIluminar.GetComponent<Renderer>().material.DisableKeyword("_EMISSION");
    }
}

public enum InventarioMensajes
{
    ObjetoAgregado,
    ObjetoSoltado,
    ObjetoFocuseado,
    InventarioLleno
}