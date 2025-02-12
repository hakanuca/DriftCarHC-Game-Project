using System;
using UnityEngine;

public class ProceduralLevelGenerator : MonoBehaviour
{
    [SerializeField] private GameObject[] roadPrefab;
    public static event Action<GameObject> OnRoadExpired;
    private int roadCounter = 0;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            for (int i = 0; i < roadPrefab.Length; i++)
            {
                var gameObject = Instantiate(roadPrefab[i], new Vector3(0, 0, 0), Quaternion.identity);
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