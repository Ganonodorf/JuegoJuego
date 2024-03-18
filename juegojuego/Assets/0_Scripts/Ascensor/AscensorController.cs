using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AscensorController : MonoBehaviour
{
    private bool arriba = false;
    private float paso = 0.2f;
    private IEnumerator moverseCoroutine;

    private Vector3 arribaPos;
    private Vector3 abajoPos;

    private void Start()
    {
        moverseCoroutine = null;
        arribaPos = new Vector3(-349.679993f, 181.0f, 59.705677f);
        abajoPos = new Vector3(-316.410034f, -5.6f, 66.6768875f);
    }


    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.name == Constantes.Player.NOMBRE_GO)
        {
            Moverse();
        }
    }

    private void Moverse()
    {
        if (moverseCoroutine != null)
        {
            StopCoroutine(moverseCoroutine);
        }

        if (arriba)
        {
            moverseCoroutine = MoverseAbajoCoroutine();
            arriba = false;
        }
        else
        {
            moverseCoroutine = MoverseArribaCoroutine();
            arriba = true;
        }

        StartCoroutine(moverseCoroutine);
    }

    private IEnumerator MoverseArribaCoroutine()
    {
        this.gameObject.GetComponent<BoxCollider>().enabled = false;

        while (this.gameObject.transform.parent.position.y < arribaPos.y)
        {
            this.gameObject.transform.parent.position = new Vector3(this.gameObject.transform.parent.position.x,
                                                             this.gameObject.transform.parent.position.y + paso,
                                                             this.gameObject.transform.parent.position.z);

            yield return new WaitForFixedUpdate();
        }

        this.gameObject.GetComponent<BoxCollider>().enabled = true;
    }

    private IEnumerator MoverseAbajoCoroutine()
    {
        this.gameObject.GetComponent<BoxCollider>().enabled = false;

        while (this.gameObject.transform.parent.position.y > abajoPos.y)
        {
            this.gameObject.transform.parent.position = new Vector3(this.gameObject.transform.parent.position.x,
                                                             this.gameObject.transform.parent.position.y - paso,
                                                             this.gameObject.transform.parent.position.z);

            yield return new WaitForFixedUpdate();
        }

        this.gameObject.GetComponent<BoxCollider>().enabled = true;
    }

}
