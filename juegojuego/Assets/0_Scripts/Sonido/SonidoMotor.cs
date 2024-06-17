using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SonidoMotor : MonoBehaviour
{
    public float minVel;
    public float maxVel;
    private float actualVel;

    public float minPitch;
    public float maxPitch;
    private float actualPitch;

    private AudioSource motorAudioSource;
    private Rigidbody jugadorRigidBody;

    private void Start()
    {
        motorAudioSource = GetComponent<AudioSource>();
        jugadorRigidBody = GameObject.FindGameObjectWithTag(Constantes.Player.TAG_PLAYER).GetComponent<Rigidbody>();
    }

    private void Update()
    {
        AcutalizarSonido();
    }

    private void AcutalizarSonido()
    {
        actualVel = jugadorRigidBody.velocity.magnitude;
        actualPitch = actualVel / 50.0f;

        if(actualVel < minVel)
        {
            motorAudioSource.pitch = minPitch;
        }

        if(actualVel > minVel && actualVel < maxVel)
        {
            motorAudioSource.pitch = minPitch + actualPitch;
        }

        if(actualVel > maxVel)
        {
            motorAudioSource.pitch = maxPitch;
        }
    }
}
