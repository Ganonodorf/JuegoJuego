using System;
using UnityEngine;
using UnityEngine.UI;

public class ScrollRectVelocity : MonoBehaviour
{
    private float VELOCIDAD = 2.0f;

    private float currentVelocity = 0.0f;

    private bool scrollArribaPressed = false;
    private bool scrollAbajoPressed = false;

    private ScrollRect scrollRect;
    private void Start()
    {
        scrollRect = this.GetComponent<ScrollRect>();
        InputManager.Instance.controles.Dialogo.ScrollArriba.performed += contexto => ScrollArriba(true);
        InputManager.Instance.controles.Dialogo.ScrollAbajo.performed += contexto => ScrollAbajo(true);
        InputManager.Instance.controles.Dialogo.ScrollArriba.canceled += contexto => ScrollArriba(false);
        InputManager.Instance.controles.Dialogo.ScrollAbajo.canceled += contexto => ScrollAbajo(false);
    }

    private void OnDestroy()
    {
        InputManager.Instance.controles.Dialogo.ScrollArriba.performed -= contexto => ScrollArriba(true);
        InputManager.Instance.controles.Dialogo.ScrollAbajo.performed -= contexto => ScrollAbajo(true);
        InputManager.Instance.controles.Dialogo.ScrollArriba.canceled -= contexto => ScrollArriba(false);
        InputManager.Instance.controles.Dialogo.ScrollAbajo.canceled -= contexto => ScrollAbajo(false);
    }

    private void Update()
    {
        if (scrollArribaPressed)
        {
            scrollRect.verticalNormalizedPosition += VELOCIDAD * Time.deltaTime;
        }
        if (scrollAbajoPressed)
        {
            scrollRect.verticalNormalizedPosition -= VELOCIDAD * Time.deltaTime;
        }
    }

    private void ScrollArriba(bool pressed)
    {
        scrollArribaPressed = pressed;
    }

    private void ScrollAbajo(bool pressed)
    {
        scrollAbajoPressed = pressed;
    }
}