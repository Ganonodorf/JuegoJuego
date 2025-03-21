using Constantes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportTriggerZone : MonoBehaviour
{
    [SerializeField] private GameObject spawnPointGO;
    [SerializeField] private GameObject carPlayerGO;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.transform.name == Constantes.Player.NOMBRE_GO)
        {
            TeleportPlayerToSpawnPoint();
        }
    }

    public void TeleportPlayerToSpawnPoint()
    {
        carPlayerGO.transform.GetComponent<Rigidbody>().velocity = Vector3.zero;

        carPlayerGO.transform.GetComponent<Rigidbody>().position = new Vector3(spawnPointGO.transform.position.x,
                                                              spawnPointGO.transform.position.y,
                                                              spawnPointGO.transform.position.z);

        carPlayerGO.transform.GetComponent<Rigidbody>().rotation = new Quaternion(spawnPointGO.transform.rotation.x,
                                                                 spawnPointGO.transform.rotation.y,
                                                                 spawnPointGO.transform.rotation.z,
                                                                 spawnPointGO.transform.rotation.w);
    }
}
