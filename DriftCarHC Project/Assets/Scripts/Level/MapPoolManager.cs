using System.Collections.Generic;
using UnityEngine;

public class MapPoolManager : MonoBehaviour
{
    public static MapPoolManager Instance { get; private set; }
    [SerializeField] private GameObject[] mapPrefabs;
    [SerializeField] private int poolSize = 10;

    private List<GameObject> pooledMaps = new List<GameObject>();
    private int nextIndex = 0;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        
        for (int i = 0; i < poolSize; i++)
        {
            GameObject prefab = mapPrefabs[Random.Range(0, mapPrefabs.Length)];
            GameObject map = Instantiate(prefab);
            map.SetActive(false);
            pooledMaps.Add(map);
        }
    }

    public GameObject GetPooledMap()
    {
        GameObject map = pooledMaps[nextIndex];
        nextIndex = (nextIndex + 1) % poolSize;

        if (!map.activeInHierarchy)
            map.SetActive(true);

        return map;
    }
}