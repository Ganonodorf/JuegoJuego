using PixelCrushers.DialogueSystem;
using UnityEngine;

public class RecogibleController : MonoBehaviour
{
    private GameObject RecogibleObject;

    private float EscalaReduccion = 2f;
    private float AlturaDelCoche = 2.5f;
    private float LongitudDelCoche = 6.0f;

    // Cuando el controlador esté disponible, se registran en código Lua las funciones que se van a llamar desde el dialog system
    void OnEnable()
    {
        Lua.RegisterFunction(nameof(RecogerObjeto), this, SymbolExtensions.GetMethodInfo(() => RecogerObjeto(string.Empty)));
        Lua.RegisterFunction(nameof(SoltarObjeto), this, SymbolExtensions.GetMethodInfo(() => SoltarObjeto(string.Empty)));
        Lua.RegisterFunction(nameof(EnablearObjeto), this, SymbolExtensions.GetMethodInfo(() => EnablearObjeto(string.Empty)));
        Lua.RegisterFunction(nameof(DisablearObjeto), this, SymbolExtensions.GetMethodInfo(() => DisablearObjeto(string.Empty)));
    }

    // Cuando el controlador deje de estar disponible, se desregistran las funciones
    void OnDisable()
    {
        // Note: If this script is on your Dialogue Manager & the Dialogue Manager is configured
        // as Don't Destroy On Load (on by default), don't unregister Lua functions.
        Lua.UnregisterFunction(nameof(RecogerObjeto)); // <-- Only if not on Dialogue Manager.
        Lua.UnregisterFunction(nameof(SoltarObjeto)); // <-- Only if not on Dialogue Manager.
        Lua.UnregisterFunction(nameof(EnablearObjeto)); // <-- Only if not on Dialogue Manager.
        Lua.UnregisterFunction(nameof(DisablearObjeto)); // <-- Only if not on Dialogue Manager.
    }

    // Este método recoge el objeto y lo pone encima del coche
    public void RecogerObjeto(string recogibleObjectName)
    {
        // Busca al objeto con ese nombre
        GameObject recogibleObject = GameObject.Find(recogibleObjectName);

        // Lo hace kinematic para que no se mueva
        recogibleObject.transform.GetComponent<Rigidbody>().isKinematic = true;

        // Reduce el objeto a la mitad
        recogibleObject.transform.localScale = new Vector3(recogibleObject.transform.localScale.x / EscalaReduccion,
                                                           recogibleObject.transform.localScale.y / EscalaReduccion,
                                                           recogibleObject.transform.localScale.z / EscalaReduccion);

        // Le cambia el padre para que sea el coche
        recogibleObject.transform.parent = this.transform;

        // Lo pone encima del coche
        recogibleObject.transform.position = new Vector3(this.transform.position.x,
                                                         this.transform.position.y + AlturaDelCoche,
                                                         this.transform.position.z);

        // Lo pone la rotación del coche
        recogibleObject.transform.rotation = new Quaternion(this.transform.rotation.x,
                                                            this.transform.rotation.y,
                                                            this.transform.rotation.z,
                                                            this.transform.rotation.w);

        // Inabilita los colliders del objeto
        recogibleObject.transform.GetComponent<BoxCollider>().enabled = false;

        // Hace que no sea usable para no poder accionar el dialog system
        recogibleObject.transform.GetComponent<Usable>().enabled = false;
    }

    public void SoltarObjeto(string soltableObjectName)
    {
        // Busca al objeto con ese nombre
        GameObject objetoSoltar = GameObject.Find(soltableObjectName);

        // Lo pone delante del coche
        objetoSoltar.transform.localPosition = new Vector3(0.0f,
                                                           0.0f,
                                                           LongitudDelCoche);

        // Amplía el objeto al doble
        objetoSoltar.transform.localScale = new Vector3(objetoSoltar.transform.localScale.x * EscalaReduccion,
                                                        objetoSoltar.transform.localScale.y * EscalaReduccion,
                                                        objetoSoltar.transform.localScale.z * EscalaReduccion);

        // Habilita los colliders del objeto
        objetoSoltar.transform.GetComponent<BoxCollider>().enabled = true;

        // Le cambia el padre para que sea el mundo
        objetoSoltar.transform.SetParent(null);
    }

    public void EnablearObjeto(string enableObjectName)
    {
        // Busca al objeto con ese nombre
        GameObject objetoEnablear = GameObject.Find(enableObjectName);

        // Hace que sea usable para poder recogerlo
        objetoEnablear.transform.GetComponent<Usable>().enabled = true;
    }

    public void DisablearObjeto(string enableObjectName)
    {
        // Busca al objeto con ese nombre
        GameObject objetoEnablear = GameObject.Find(enableObjectName);

        // Hace que sea usable para poder recogerlo
        objetoEnablear.transform.GetComponent<Usable>().enabled = false;
    }
}
