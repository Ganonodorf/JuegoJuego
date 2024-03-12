using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarFrontLights : MonoBehaviour
{
    [SerializeField] private GameObject breaklight;


    // Start is called before the first frame update
    void Start()
    {
        breaklight = this.gameObject;

    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.F) && breaklight.GetComponent<Light>().enabled == false)
        {
            breaklight.GetComponent<Light>().enabled = true;

        }
        else if(Input.GetKeyDown(KeyCode.F) && breaklight.GetComponent<Light>().enabled == true)
        {
            breaklight.GetComponent<Light>().enabled = false;
        }
    }
}
