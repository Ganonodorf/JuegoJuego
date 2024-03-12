using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarBreakLights : MonoBehaviour
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
        if (Input.GetKey("space"))
        {
            breaklight.GetComponent<Light>().enabled = true;

        }
        else
        {
            breaklight.GetComponent<Light>().enabled = false;
        }
    }
}
