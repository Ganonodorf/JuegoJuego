using PixelCrushers.DialogueSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogManager : MonoBehaviour
{
    // Con esto declaras que esta clase va a ser estática, es decir, va a estar siempre activa
    // y disponible para todos
    public static DialogManager Instance;

    private int c = 0;
    // En el awake decimos que si cuando el GameObject que tenga este script es creado
    // no existe ya un GameManager, asignamos Instance a ese GameObject.
    // Si no es así y ya existe un GameManager, el que se acaba de crear se borra.
    private void Awake()
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

    public void ObjectPicked(string objectPickedName)
    {
        if(objectPickedName == "ModemNegro")
        {
            DialogueLua.SetVariable("HaveModemNegro", true);
        }
    }
}
