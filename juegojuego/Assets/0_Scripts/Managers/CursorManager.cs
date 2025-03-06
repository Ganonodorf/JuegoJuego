using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorManager : MonoBehaviour
{
    [SerializeField] private CursorState _cursor_state;

    // Start is called before the first frame update
    void Start()
    {
        GameManager.OnGameStateChanged += GameManager_OnGameStateChanged;

        GestionarInputs();
    }

    private void OnDestroy()
    {
        GameManager.OnGameStateChanged -= GameManager_OnGameStateChanged;

        DesgestionarInputs();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetAxis("Mouse X") != 0)
        {
            OptionallyActivateCursor();
        }
    }

    private void GameManager_OnGameStateChanged(GameState nuevoEstado, GameState anteriorEstado)
    {
        if (nuevoEstado == GameState.MenuInicio ||
            nuevoEstado == GameState.FinJuego ||
            nuevoEstado == GameState.PantallaPausa ||
            nuevoEstado == GameState.SecuenciaInicial ||
            nuevoEstado == GameState.SecuenciaFinal ||
             nuevoEstado == GameState.Dialogo)
        {
            ChangeCursorState(CursorState.Optional);
        }
        else
        {
            ChangeCursorState(CursorState.Deactivated);
        }
    }

    private void ChangeCursorState(CursorState _new_cursor_state)
    {
        switch (_new_cursor_state)
        {
            case CursorState.Optional:
                _cursor_state = CursorState.Optional;
                break;
            case CursorState.Deactivated:
                _cursor_state = CursorState.Deactivated;
                Cursor.visible = false;
                break;
        }
    }

    private void OptionallyDisableCursor()
    {
        if(_cursor_state == CursorState.Optional)
        {
            Cursor.visible = false;
        }
    }

    private void OptionallyActivateCursor()
    {
        if (_cursor_state == CursorState.Optional)
        {
            Cursor.visible = true;
        }
    }

    public enum CursorState
    {
        Deactivated,
        Optional
    }

    private void GestionarInputs()
    {
        InputManager.Instance.controles.Dialogo.MovimientoLateral.performed += contexto => OptionallyDisableCursor();
        InputManager.Instance.controles.Dialogo.Arriba.performed += contexto => OptionallyDisableCursor();
        InputManager.Instance.controles.Dialogo.Abajo.performed += contexto => OptionallyDisableCursor();
        InputManager.Instance.controles.Dialogo.Seleccion.performed += contexto => OptionallyDisableCursor();

        InputManager.Instance.controles.UI.MovimientoDer.performed += contexto => OptionallyDisableCursor();
        InputManager.Instance.controles.UI.MovimientoIzq.performed += contexto => OptionallyDisableCursor();
        InputManager.Instance.controles.UI.Arriba.performed += contexto => OptionallyDisableCursor();
        InputManager.Instance.controles.UI.Abajo.performed += contexto => OptionallyDisableCursor();
    }

    private void DesgestionarInputs()
    {
        InputManager.Instance.controles.Dialogo.MovimientoLateral.performed -= contexto => OptionallyDisableCursor();
        InputManager.Instance.controles.Dialogo.Arriba.performed -= contexto => OptionallyDisableCursor();
        InputManager.Instance.controles.Dialogo.Abajo.performed -= contexto => OptionallyDisableCursor();
        InputManager.Instance.controles.Dialogo.Seleccion.performed -= contexto => OptionallyDisableCursor();

        InputManager.Instance.controles.UI.MovimientoDer.performed -= contexto => OptionallyDisableCursor();
        InputManager.Instance.controles.UI.MovimientoIzq.performed -= contexto => OptionallyDisableCursor();
        InputManager.Instance.controles.UI.Arriba.performed -= contexto => OptionallyDisableCursor();
        InputManager.Instance.controles.UI.Abajo.performed -= contexto => OptionallyDisableCursor();
    }
}
