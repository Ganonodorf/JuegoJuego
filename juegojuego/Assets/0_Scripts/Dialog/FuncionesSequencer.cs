using UnityEngine;

namespace PixelCrushers.DialogueSystem.SequencerCommands
{
    public class SequencerCommandBajarPuertas : SequencerCommand
    {
        public void Start()
        {
            GameObject.FindGameObjectWithTag(Constantes.Objetos.TAG_PUERTA_LEVADIZA).GetComponent<Animator>().Play("Bajar");
            Stop();
        }
    }
    public class SequencerCommandFadeToBlack : SequencerCommand
    {
        public void Start()
        {
            GameObject.FindGameObjectWithTag(Constantes.Camaras.TAG_CAMARA_CORTES).GetComponent<Animator>().Play("FadeToBlack");
            Stop();
        }
    }
    public class SequencerCommandFadeToWhite : SequencerCommand
    {
        public void Start()
        {
            GameObject.FindGameObjectWithTag(Constantes.Camaras.TAG_CAMARA_CORTES).GetComponent<Animator>().Play("FadeToWhite");
            Stop();
        }
    }
}
