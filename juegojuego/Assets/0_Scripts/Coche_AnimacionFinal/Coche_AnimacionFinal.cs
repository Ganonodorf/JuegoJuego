using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coche_AnimacionFinal : MonoBehaviour
{
    [SerializeField]
    private TeleportTriggerZone teleportTriggerZone;
    
    [SerializeField]
    private GameObject animationCamera;

    public void AnimationEnded()
    {
        teleportTriggerZone.TeleportPlayerToSpawnPoint();
        animationCamera.SetActive(false);
        
    }
}
