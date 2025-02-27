using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarRespawn : MonoBehaviour
{
    public Transform respawnPoint;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Crashable"))
        {
            RespawnCar();
        }
    }

    void RespawnCar()
    {
        transform.position = respawnPoint.position;
        transform.rotation = respawnPoint.rotation;
    }
}