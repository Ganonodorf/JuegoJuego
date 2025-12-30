using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MusicVolumeChange : MonoBehaviour
{
    [SerializeField] private Slider musicVolumeSlider;
    [SerializeField] private AudioSource musicSource;

   

    public void SetMusicVolume()
    {
        musicSource.volume = musicVolumeSlider.value;
    }
}
