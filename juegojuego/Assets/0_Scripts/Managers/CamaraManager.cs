using Cinemachine;
using PixelCrushers.DialogueSystem;
using System.Linq;
using UnityEngine;

public class CamaraManager : MonoBehaviour
{
    public static CamaraManager Instance;

    private GameObject camaraExterioresGO;
    private GameObject camaraInterioresGO;
    private GameObject camaraCortesGO;
    private GameObject camaraInventarioGO;

    private void Awake()
    {
        //HacerloInmortal();

        Instance = this;
    }

    private void Start()
    {
        BuscarGO();

        RecogerInfoInputs();
    }
    void OnEnable()
    {
        RegistrarFuncionesLua();
    }

    void OnDisable()
    {
        DesregistrarFuncionesLua();
    }

    private void OnDestroy()
    {
        SoltarInfoInputs();
    }

    public void JugadorHaEntrado()
    {
        camaraExterioresGO.GetComponent<CinemachineFreeLook>().enabled = false;
        camaraInterioresGO.GetComponent<CinemachineFreeLook>().enabled = true;
    }

    public void JugadorHaSalido()
    {
        camaraExterioresGO.GetComponent<CinemachineFreeLook>().enabled = true;
        camaraInterioresGO.GetComponent<CinemachineFreeLook>().enabled = false;
    }

    public void FadeToBlack()
    {
        camaraCortesGO.GetComponent<Animator>().Play(Constantes.Camaras.ANIMACION_FADE_TO_BLACK);
        Debug.Log("black");
    }

    public void FadeToWhite()
    {
        camaraCortesGO.GetComponent<Animator>().Play(Constantes.Camaras.ANIMACION_FADE_TO_WHITE);
        Debug.Log("white");
    }

    public void MirarObjetoInventario(string nombreObjetoGO)
    {
        GameObject[] listaGO = GameObject.FindGameObjectsWithTag(Constantes.Objetos.TAG_OBJETO_RECOGIBLE);
        GameObject objetoAMirar = listaGO.Where(go => go.name == nombreObjetoGO).FirstOrDefault();

        camaraInventarioGO.GetComponent<CinemachineVirtualCamera>().enabled = true;
        camaraInventarioGO.GetComponent<CinemachineVirtualCamera>().LookAt = objetoAMirar.transform;
    }

    public void DejarMirarObjeto()
    {
        camaraInventarioGO.GetComponent<CinemachineVirtualCamera>().enabled = false;
    }

    private void AccionJugadorDerrapar()
    {
        //camaraExterioresGO.GetComponent<CinemachineFreeLook>().m_Lens.FieldOfView = 50.0f;
    }

    private void AccionJugadorDejarDeDerrapar()
    {
        //camaraExterioresGO.GetComponent<CinemachineFreeLook>().m_Lens.FieldOfView = 40.0f;
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

    private void BuscarGO()
    {
        camaraExterioresGO = GameObject.FindGameObjectWithTag(Constantes.Camaras.TAG_CAMARA_EXTERIORES);
        camaraInterioresGO = GameObject.FindGameObjectWithTag(Constantes.Camaras.TAG_CAMARA_INTERIORES);
        camaraCortesGO = GameObject.FindGameObjectWithTag(Constantes.Camaras.TAG_CAMARA_CORTES);
        camaraInventarioGO = GameObject.FindGameObjectWithTag(Constantes.Camaras.TAG_CAMARA_INVENTARIO);
    }

    private void RegistrarFuncionesLua()
    {
        Lua.RegisterFunction(nameof(FadeToBlack), this, SymbolExtensions.GetMethodInfo(() => FadeToBlack()));
        Lua.RegisterFunction(nameof(FadeToWhite), this, SymbolExtensions.GetMethodInfo(() => FadeToWhite()));
        Lua.RegisterFunction(nameof(MirarObjetoInventario), this, SymbolExtensions.GetMethodInfo(() => MirarObjetoInventario(string.Empty)));
        Lua.RegisterFunction(nameof(DejarMirarObjeto), this, SymbolExtensions.GetMethodInfo(() => DejarMirarObjeto()));
    }

    private void DesregistrarFuncionesLua()
    {
        Lua.UnregisterFunction(nameof(FadeToBlack));
        Lua.UnregisterFunction(nameof(FadeToWhite));
        Lua.RegisterFunction(nameof(MirarObjetoInventario), this, SymbolExtensions.GetMethodInfo(() => MirarObjetoInventario(string.Empty)));
        Lua.RegisterFunction(nameof(DejarMirarObjeto), this, SymbolExtensions.GetMethodInfo(() => DejarMirarObjeto()));
    }

    private void RecogerInfoInputs()
    {
        InputManager.Instance.controles.Conduciendo.Derrape.performed += contexto => AccionJugadorDerrapar();
        InputManager.Instance.controles.Conduciendo.Derrape.canceled += contexto => AccionJugadorDejarDeDerrapar();
    }
    private void SoltarInfoInputs()
    {
        InputManager.Instance.controles.Conduciendo.Derrape.performed -= contexto => AccionJugadorDerrapar();
        InputManager.Instance.controles.Conduciendo.Derrape.canceled -= contexto => AccionJugadorDejarDeDerrapar();
    }
}
