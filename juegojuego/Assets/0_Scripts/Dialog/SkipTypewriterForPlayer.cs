using UnityEngine;
using PixelCrushers.DialogueSystem;
using Unity.VisualScripting;
public class SkipTypewriterForPlayer : MonoBehaviour
{
    void OnConversationLine(Subtitle subtitle)
    {
        if (subtitle.speakerInfo.isPlayer && 
            subtitle.listenerInfo.Name != "Manu" && subtitle.listenerInfo.Name != "Paco") // Excepci�n para los fabricantes
        {
            subtitle.formattedText.text = @"\^" + subtitle.formattedText.text;
        }
    }
}