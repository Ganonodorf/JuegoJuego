using System.Collections;
using System.Collections.Generic;
using PixelCrushers.DialogueSystem;
using UnityEngine;

public class Coche_AnimacionFinal : MonoBehaviour
{
    [SerializeField]
    private TeleportTriggerZone teleportTriggerZone;
    
    [SerializeField]
    private GameObject animationCamera;
    
    public void EndFinalConversation()
    {
        for (int i = DialogueManager.instance.activeConversations.Count - 1; i >= 0; i--)
        {
            DialogueManager.instance.activeConversations[i].conversationController.Close();
        }
    }
    
    public void AnimationStarted()
    {
        GameManager.Instance.UpdateGameState(GameState.Dialogo);
        teleportTriggerZone.TeleportPlayerToSpawnPoint();
    }

    public void AnimationEnded()
    {
        animationCamera.SetActive(false);
        
        GameManager.Instance.UpdateGameState(GameState.Conduciendo);
    }
}