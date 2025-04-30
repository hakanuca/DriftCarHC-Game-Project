#if UNITY_EDITOR
using UnityEditor;
#endif
using System.Collections.Generic;
using UnityEngine;

public class ProceduralLevelGenerator : MonoBehaviour
{
    [SerializeField] private MapPoolManager mapPool;
    private HashSet<Vector2Int> spawnedTiles = new HashSet<Vector2Int>();
    private float tileSize = 500f;

    public void SpawnNextMap(Vector3 playerPosition, Vector3 forward)
    {
        Vector2Int nextGrid = GetGridPosition(playerPosition + forward * tileSize);

        if (spawnedTiles.Contains(nextGrid)) return;

        GameObject newMap = mapPool.GetPooledMap();
        Vector3 spawnPosition = new Vector3(nextGrid.x * tileSize, 0f, nextGrid.y * tileSize);
        newMap.transform.position = spawnPosition;

        spawnedTiles.Add(nextGrid);
    }

    private Vector2Int GetGridPosition(Vector3 position)
    {
        int gridX = Mathf.FloorToInt(position.x / tileSize);
        int gridZ = Mathf.FloorToInt(position.z / tileSize);
        return new Vector2Int(gridX, gridZ);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        foreach (Vector2Int gridPos in spawnedTiles)
        {
            Vector3 center = new Vector3(gridPos.x * tileSize, 0f, gridPos.y * tileSize);
            Gizmos.DrawWireCube(center + new Vector3(tileSize / 2f, 0, tileSize / 2f), new Vector3(tileSize, 0.1f, tileSize));
        }
    }

}