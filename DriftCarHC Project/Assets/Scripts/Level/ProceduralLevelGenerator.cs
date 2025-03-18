using System;
using UnityEngine;

public class ProceduralLevelGenerator : MonoBehaviour
{
    [SerializeField] private GameObject[] roadPrefabs;
    private int roadCounter = 0;
    [SerializeField] private float roadSpawnOnX = 100;

    public static event Action<GameObject> OnRoadExpired;

    public void SpawnNextRoad()
    {
        GameObject firstRoad = GameObject.FindWithTag("Level");
        Vector3 newPosition = firstRoad.transform.position;

        GameObject newRoadPrefab = roadPrefabs[UnityEngine.Random.Range(0, roadPrefabs.Length)];

        newPosition.x += roadSpawnOnX;
        newPosition.y = 0;
        newPosition.z = 0;
        
        roadCounter++;
        if (roadCounter % 3 == 0)
        {
            RemoveOldRoads();
        }
    }

    private void RemoveOldRoads()
    {
        GameObject[] roads = GameObject.FindGameObjectsWithTag("Level");
        if (roads.Length > 3)
        {
            Destroy(roads[0]);
            OnRoadExpired?.Invoke(roads[0]);
        }
    }
}