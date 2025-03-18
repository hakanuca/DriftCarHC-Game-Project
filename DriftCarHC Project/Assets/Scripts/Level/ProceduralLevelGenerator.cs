using System;
using UnityEngine;

public class ProceduralLevelGenerator : MonoBehaviour
{
    [SerializeField] private GameObject[] roadPrefabs;
    private int roadCounter = 0;
    [SerializeField] private float roadSpawnOnX = 100;
    private float lastRoadXPosition = 0;

    public static event Action<GameObject> OnRoadExpired;

    public void SpawnNextRoad()
    {
        GameObject newRoadPrefab = roadPrefabs[UnityEngine.Random.Range(0, roadPrefabs.Length)];
        GameObject newRoad = Instantiate(newRoadPrefab);

        Vector3 newPosition = new Vector3(lastRoadXPosition + roadSpawnOnX, 0, 0);
        newRoad.transform.position = newPosition;

        lastRoadXPosition = newPosition.x;

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