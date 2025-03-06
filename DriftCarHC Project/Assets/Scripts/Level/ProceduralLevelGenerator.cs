using System;
using UnityEngine;

public class ProceduralLevelGenerator : MonoBehaviour
{
    [SerializeField] private GameObject[] roadPrefab;
    public static event Action<GameObject> OnRoadExpired;
    private int roadCounter = 0;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Trigger"))
        {
            for (int i = 0; i < roadPrefab.Length; i++)
            {
                // Find the endpoint object within the instantiated prefab
                var endpoint = GameObject.FindWithTag("Endpoint");
                if (endpoint != null)
                {
                    var spawnPosition = endpoint.transform.position;
                    // Move the instantiated object to the endpoint position
                    transform.position = spawnPosition;
                    Instantiate(roadPrefab[i], transform.position, Quaternion.identity);
                }
                roadCounter++;
                if (roadCounter % 3 == 0)
                {
                    for (int j = 0; j < roadPrefab.Length; j++)
                    {
                        RemoveRoad(roadPrefab[j]);
                    }
                }
            }
        }
    }

    private static void RemoveRoad(GameObject road)
    {
        OnRoadExpired?.Invoke(road);
        Destroy(road);
    }
}