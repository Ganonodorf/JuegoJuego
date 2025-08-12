using PixelCrushers.DialogueSystem;
using UnityEngine;

public class FinJuego : MonoBehaviour
{
    [SerializeField]
    private GameObject PlayerGO;
    
    [SerializeField]
    private GameObject MagdalenitaGO;
    
    private void OnTriggerEnter(Collider collider)
    {
        if(collider.transform.name == Constantes.Player.NOMBRE_GO)
        {
            PlayerGO.SetActive(false);
            MagdalenitaGO.transform.Rotate(Vector3.up, 180f);
            GameManager.Instance.UpdateGameState(GameState.SecuenciaFinal);
        }
    }

    public void FinVolverALaMugre()
    {
        for (int i = DialogueManager.instance.activeConversations.Count - 1; i >= 0; i--)
        {
            DialogueManager.instance.activeConversations[i].conversationController.Close();
        }
        
        PlayerGO.SetActive(false);
        GameManager.Instance.UpdateGameState(GameState.SecuenciaFinal);
    }
}
