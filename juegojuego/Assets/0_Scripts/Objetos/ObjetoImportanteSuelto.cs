using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjetoImportanteSuelto : MonoBehaviour
{
    [SerializeField] private GameObject objeto;
    [SerializeField] private GameObject luzObjeto;


    // Start is called before the first frame update
    void Start()
    {
        objeto = this.transform.parent.gameObject;
        luzObjeto = objeto.transform.Find("luzObjeto").gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("ground"))
        {
            objeto.GetComponent<Rigidbody>().isKinematic = true;
            objeto.GetComponent<Animator>().enabled = true;
            luzObjeto.gameObject.SetActive(true);

        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        //if (other.gameObject.tag == "ground")
        {
            objeto.GetComponent<Rigidbody>().isKinematic = false;
            objeto.GetComponent<Animator>().enabled = false;
            luzObjeto.gameObject.SetActive(false);

        }
    }
}
