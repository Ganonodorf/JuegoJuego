using System.Collections;
using UnityEngine;

public class ScreenshotManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        InputManager.Instance.controles.Conduciendo.Foto.performed += contexto => HacerFoto();
        InputManager.Instance.controles.Inventario.Foto.performed += contexto => HacerFoto();
        InputManager.Instance.controles.UI.Foto.performed += contexto => HacerFoto();
        InputManager.Instance.controles.Dialogo.Foto.performed += contexto => HacerFoto();
        InputManager.Instance.controles.CamaraEditor.Foto.performed += contexto => HacerFoto();
    }

    private void HacerFoto()
    {
        StartCoroutine(TomarFoto());
    }

    private IEnumerator TomarFoto()
    {
        yield return new WaitForEndOfFrame();
        string currentTime = System.DateTime.Now.ToString("MM-dd-yy_HH-mm-ss");
        ScreenCapture.CaptureScreenshot(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop) + "/screenshot" + currentTime + ".png");
    }
}
