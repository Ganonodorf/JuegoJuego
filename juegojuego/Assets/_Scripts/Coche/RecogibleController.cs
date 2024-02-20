using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecogibleController : MonoBehaviour
{
    private GameObject RecogibleObject;

    private float EscalaReduccion = 2f;
    private float AlturaDelCoche = 2.5f;
    private float LongitudDelCoche = 6.0f;
    private Vector3 FuerzaLanzar = new Vector3(0.0f, 5.0f, 20.0f);

    private List<GameObject> ObjetosRecogidos;

    private void Start()
    {
        RecogibleObject = null;
        ObjetosRecogidos = new List<GameObject>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q) && RecogibleObject != null)
        {
            RecogerObjeto();
        }

        if(Input.GetKeyDown(KeyCode.E) && ObjetosRecogidos.Count > 0)
        {
            SoltarObjeto();
        }


    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.transform.tag == "Recogible" && RecogibleObject == null)
        {
            RecogibleObject = collision.gameObject;
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        if (collision.transform.tag == "Recogible" && RecogibleObject != null)
        {
            RecogibleObject = null;
        }
    }

    // Este método recoge el objeto y lo pone encima del coche
    private void RecogerObjeto()
    {
        // Lo hace kinematic para que no se mueva
        RecogibleObject.transform.GetComponent<Rigidbody>().isKinematic = true;

        // Reduce el objeto a la mitad
        RecogibleObject.transform.localScale = new Vector3(RecogibleObject.transform.localScale.x / EscalaReduccion,
                                                           RecogibleObject.transform.localScale.y / EscalaReduccion,
                                                           RecogibleObject.transform.localScale.z / EscalaReduccion);

        // Le cambia el padre para que sea el coche
        RecogibleObject.transform.parent = this.transform;

        // Lo pone encima del coche
        RecogibleObject.transform.position = new Vector3(this.transform.position.x,
                                                         this.transform.position.y + AlturaDelCoche,
                                                         this.transform.position.z);

        // Lo pone la rotación del coche
        RecogibleObject.transform.rotation = new Quaternion(this.transform.rotation.x,
                                                            this.transform.rotation.y,
                                                            this.transform.rotation.z,
                                                            this.transform.rotation.w);

        // Inabilita los colliders del objeto
        RecogibleObject.transform.GetComponent<SphereCollider>().enabled = false;
        RecogibleObject.transform.GetComponent<BoxCollider>().enabled = false;

        // Mete el objeto en la lista
        ObjetosRecogidos.Add(RecogibleObject);

        // Pone la variable a null para que no pueda seguir recogiéndolo una vez ya recogida
        RecogibleObject = null;
    }

    private void SoltarObjeto()
    {
        int totalObjetos = ObjetosRecogidos.Count;

        GameObject objetoSoltar = ObjetosRecogidos[totalObjetos - 1];

        // Lo pone delante del coche
        objetoSoltar.transform.localPosition = new Vector3(0.0f,
                                                           0.0f,
                                                           LongitudDelCoche);

        // Amplía el objeto al doble
        objetoSoltar.transform.localScale = new Vector3(objetoSoltar.transform.localScale.x * EscalaReduccion,
                                                        objetoSoltar.transform.localScale.y * EscalaReduccion,
                                                        objetoSoltar.transform.localScale.z * EscalaReduccion);

        // Le quita lo de kinematic para que le afecte la fuerza
        objetoSoltar.transform.GetComponent<Rigidbody>().isKinematic = false;

        /*

        // Le da velocidad para que salga disparado
        objetoSoltar.transform.GetComponent<Rigidbody>().AddRelativeForce(FuerzaLanzar);

        */

        // Habilita los colliders del objeto
        objetoSoltar.transform.GetComponent<SphereCollider>().enabled = true;
        objetoSoltar.transform.GetComponent<BoxCollider>().enabled = true;

        // Elimino el objeto de la lista
        ObjetosRecogidos.Remove(objetoSoltar);

        // Le cambia el padre para que sea el mundo
        objetoSoltar.transform.SetParent(null);
        
    }
}
