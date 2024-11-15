using UnityEngine;

public class SonidoCampanas : MonoBehaviour
{
    private AudioSource audioSource;
    
    private int minValue = 60;
    private int maxValue = 180;

    private int waitingTime = 180;

    private float counter;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        counter += Time.deltaTime;

        if (counter >= waitingTime)
        {
            audioSource.Play();
            counter = 0;
            waitingTime = Random.Range(minValue, maxValue);
        }
    }
}
