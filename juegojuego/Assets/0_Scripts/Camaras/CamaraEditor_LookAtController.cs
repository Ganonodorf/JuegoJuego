using System;
using UnityEngine;

public class CamaraEditor_LookAtController : MonoBehaviour
{
    private Vector3 direccionMov;
    private Vector3 direccionRot;
    private Vector3 direccionRaton;
    private Vector3 direccionAltura;
    [SerializeField] private float velMov;
    [SerializeField] private float velRot;
    [SerializeField] private bool usarScrollBorde;
    [SerializeField] private float cantidadScroll;
    [SerializeField] private float velBajarY;

    // Start is called before the first frame update
    void Start()
    {
        GestionarInputs();

        direccionMov = Vector3.zero;
        direccionRot = Vector3.zero;
        direccionRaton = Vector3.zero;
    }

    private void OnDestroy()
    {
        DesgestionarInputs();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += direccionMov * velMov * Time.deltaTime;
        transform.eulerAngles += direccionRot * velRot * Time.deltaTime;

        if (usarScrollBorde)
        {
            ScrollBorde();
            transform.position += direccionRaton * velMov * Time.deltaTime;
        }

        transform.position += direccionAltura * velBajarY * Time.deltaTime;
    }

    private void MoverCamara(Vector2 input)
    {
        direccionMov = -transform.forward * input.x + transform.right * input.y;
    }

    private void RotarCamara(float input)
    {
        direccionRot = new Vector3(0f, input, 0f);
    }

    private void ScrollBorde()
    {
        if (Input.mousePosition.x < cantidadScroll) direccionRaton.x = -1f;
        if (Input.mousePosition.y < cantidadScroll) direccionRaton.z = -1f;
        if (Input.mousePosition.x > Screen.width - cantidadScroll) direccionRaton.x = 1f;
        if (Input.mousePosition.y > Screen.height - cantidadScroll) direccionRaton.z = 1f;
    }

    private void MoverAltura(float input)
    {
        direccionAltura = new Vector3(0f, input, 0f);
    }

    private void GestionarInputs()
    {
        InputManager.Instance.controles.CamaraEditor.Movimiento.performed += contexto => MoverCamara(contexto.ReadValue<Vector2>());
        InputManager.Instance.controles.CamaraEditor.Movimiento.canceled += contexto => MoverCamara(contexto.ReadValue<Vector2>());
        InputManager.Instance.controles.CamaraEditor.Rotacion.performed += contexto => RotarCamara(contexto.ReadValue<Vector2>().x);
        InputManager.Instance.controles.CamaraEditor.Rotacion.canceled += contexto => RotarCamara(contexto.ReadValue<Vector2>().x);
        InputManager.Instance.controles.CamaraEditor.MoverAltura.performed += contexto => MoverAltura(contexto.ReadValue<Vector2>().y);
        InputManager.Instance.controles.CamaraEditor.MoverAltura.canceled += contexto => MoverAltura(contexto.ReadValue<Vector2>().y);
    }

    private void DesgestionarInputs()
    {
        InputManager.Instance.controles.CamaraEditor.Movimiento.performed -= contexto => MoverCamara(contexto.ReadValue<Vector2>());
        InputManager.Instance.controles.CamaraEditor.Movimiento.canceled -= contexto => MoverCamara(contexto.ReadValue<Vector2>());
        InputManager.Instance.controles.CamaraEditor.Rotacion.performed -= contexto => RotarCamara(contexto.ReadValue<Vector2>().x);
        InputManager.Instance.controles.CamaraEditor.Rotacion.canceled -= contexto => RotarCamara(contexto.ReadValue<Vector2>().x);
        InputManager.Instance.controles.CamaraEditor.MoverAltura.performed -= contexto => MoverAltura(contexto.ReadValue<Vector2>().y);
        InputManager.Instance.controles.CamaraEditor.MoverAltura.canceled -= contexto => MoverAltura(contexto.ReadValue<Vector2>().y);
    }
}
