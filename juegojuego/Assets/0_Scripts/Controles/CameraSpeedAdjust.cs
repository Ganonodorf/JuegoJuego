using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class CameraSpeedAdjust : MonoBehaviour
{
    [SerializeField] private Slider cameraSpeedSliderX;
    [SerializeField] private Slider cameraSpeedSliderY;
    [SerializeField] private CinemachineFreeLook cameraExteriores;
    [SerializeField] private CinemachineFreeLook cameraInteriores;


    public void SetCameraSpeed()
    {
        cameraExteriores.m_XAxis.m_MaxSpeed = cameraSpeedSliderX.value;
        cameraExteriores.m_YAxis.m_MaxSpeed = cameraSpeedSliderY.value;

        cameraInteriores.m_XAxis.m_MaxSpeed = cameraSpeedSliderX.value;
        cameraInteriores.m_YAxis.m_MaxSpeed = cameraSpeedSliderY.value;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


}
