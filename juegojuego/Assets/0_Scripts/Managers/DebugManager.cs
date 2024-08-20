using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugManager : MonoBehaviour
{
    public static DebugManager Instance;

    private GameObject DebugUIGO;

    private void Awake()
    {
        HacerloInmortal();

        BuscarGO();
    }

    // Start is called before the first frame update
    void Start()
    {

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
        DebugUIGO = GameObject.FindGameObjectWithTag(Constantes.DebugUI.TAG_DEBUG_UI);
    }
}
