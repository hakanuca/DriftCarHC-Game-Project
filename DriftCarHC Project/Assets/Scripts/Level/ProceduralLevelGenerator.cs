#if UNITY_EDITOR
using UnityEditor;
#endif
using System.Collections.Generic;
using UnityEngine;

public class ProceduralLevelGenerator : MonoBehaviour
{
    [SerializeField] private MapPoolManager mapPool;
    private HashSet<Vector2Int> spawnedTiles = new HashSet<Vector2Int>();
    private float tileSize = 200f;

    private Queue<GameObject> activeMaps = new Queue<GameObject>();
	private int maxActiveMaps = 10;

public void SpawnNextMap(Vector3 playerPosition, Vector3 forward)
{
    Vector2Int nextGrid = GetGridPosition(playerPosition + forward * tileSize);

    if (spawnedTiles.Contains(nextGrid)) return;

    GameObject newMap = mapPool.GetPooledMap();
    newMap.transform.position = new Vector3(nextGrid.x * tileSize, 0f, 0f);
    newMap.SetActive(true);

    spawnedTiles.Add(nextGrid);
    activeMaps.Enqueue(newMap);

    // Remove the oldest map if limit exceeded
    if (activeMaps.Count > maxActiveMaps)
    {
        GameObject oldMap = activeMaps.Dequeue();
        oldMap.SetActive(false);
    }
}



    private Vector2Int GetGridPosition(Vector3 position)
    {
        int gridX = Mathf.FloorToInt(position.x / tileSize);
        int gridZ = Mathf.FloorToInt(position.z / tileSize);
        return new Vector2Int(gridX, gridZ);
    }
}