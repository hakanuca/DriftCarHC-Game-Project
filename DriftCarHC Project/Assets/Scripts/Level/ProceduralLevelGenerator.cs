using UnityEngine;

public class ProceduralLevelGenerator : MonoBehaviour
{
    [SerializeField] private MapPoolManager mapPool;
    private Transform lastEndPoint;
    
    public void SpawnNextMap()
    {
        GameObject newMap = mapPool.GetPooledMap();

        Transform startPoint = newMap.transform.Find("StartPoint");
        Transform endPoint = newMap.transform.Find("EndPoint");

        if (startPoint == null || endPoint == null)
        {
            Debug.LogError("StartPoint or EndPoint not found in map prefab!");
            return;
        }

        Vector3 offset = Vector3.zero;

        if (lastEndPoint != null)
        {
            // Align newMap’s StartPoint with the previous map’s EndPoint
            offset = lastEndPoint.position - startPoint.position;
        }

        newMap.transform.position += offset;

        // Ensure only X-axis movement (flat level)
        Vector3 correctedPos = newMap.transform.position;
        correctedPos.y = 0f;
        correctedPos.z = 0f;
        newMap.transform.position = correctedPos;

        lastEndPoint = endPoint;
    }
    
}