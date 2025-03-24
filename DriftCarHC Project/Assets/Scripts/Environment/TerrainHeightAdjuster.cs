using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class TerrainHeightAdjuster : MonoBehaviour
{
    public Terrain terrain;
    public GameObject[] staticObjects;

    private string saveFilePath;

    private void Awake()
    {
        saveFilePath = Application.persistentDataPath + "/terrainOriginalHeights.dat";
    }

    [ContextMenu("Save Original Heights")]
    public void SaveOriginalHeights()
    {
        TerrainData terrainData = terrain.terrainData;
        int resolution = terrainData.heightmapResolution;
        float[,] originalHeights = terrainData.GetHeights(0, 0, resolution, resolution);

        BinaryFormatter bf = new BinaryFormatter();
        using (FileStream file = File.Create(saveFilePath))
        {
            bf.Serialize(file, originalHeights);
        }

        Debug.Log($"Original terrain heights saved at: {saveFilePath}");
    }

    [ContextMenu("Adjust Terrain Heights")]
    public void AdjustTerrain()
    {
        TerrainData terrainData = terrain.terrainData;
        Vector3 terrainPos = terrain.transform.position;
        int resolution = terrainData.heightmapResolution;
        Vector3 terrainSize = terrainData.size;

        if (!File.Exists(saveFilePath))
        {
            SaveOriginalHeights();  // Auto-save original heights first
        }

        foreach (GameObject obj in staticObjects)
        {
            Renderer rend = obj.GetComponent<Renderer>();
            if (rend == null) continue;

            Bounds objBounds = rend.bounds;

            float relativeX = (objBounds.center.x - terrainPos.x) / terrainSize.x;
            float relativeZ = (objBounds.center.z - terrainPos.z) / terrainSize.z;

            int terrainX = Mathf.RoundToInt(relativeX * (resolution - 1));
            int terrainZ = Mathf.RoundToInt(relativeZ * (resolution - 1));

            float objSize = Mathf.Max(objBounds.extents.x, objBounds.extents.z);
            int radius = Mathf.CeilToInt((objSize / terrainSize.x) * resolution) + 2;

            float desiredHeight = Mathf.Clamp((objBounds.min.y - terrainPos.y) / terrainSize.y, 0f, 1f);

            int xStart = Mathf.Clamp(terrainX - radius, 0, resolution - 1);
            int zStart = Mathf.Clamp(terrainZ - radius, 0, resolution - 1);
            int xEnd = Mathf.Clamp(terrainX + radius, 0, resolution);
            int zEnd = Mathf.Clamp(terrainZ + radius, 0, resolution);

            int width = xEnd - xStart;
            int height = zEnd - zStart;

            float[,] heights = terrainData.GetHeights(xStart, zStart, width, height);

            for (int x = 0; x < width; x++)
            {
                for (int z = 0; z < height; z++)
                {
                    float distX = (x + xStart - terrainX);
                    float distZ = (z + zStart - terrainZ);
                    float distance = Mathf.Sqrt(distX * distX + distZ * distZ);

                    if (distance <= radius)
                    {
                        float smoothing = Mathf.SmoothStep(0, 1, distance / radius);
                        float targetHeight = Mathf.Lerp(desiredHeight, heights[z, x], smoothing);
                        heights[z, x] = Mathf.Min(heights[z, x], targetHeight);
                    }
                }
            }

            terrainData.SetHeights(xStart, zStart, heights);
        }

        terrain.Flush();
#if UNITY_EDITOR
        UnityEditor.EditorUtility.SetDirty(terrainData);
#endif
        Debug.Log("Terrain adjustments complete.");
    }

    [ContextMenu("Reset Terrain Heights")]
    public void ResetTerrain()
    {
        if (!File.Exists(saveFilePath))
        {
            Debug.LogError("No original terrain heights file found!");
            return;
        }

        BinaryFormatter bf = new BinaryFormatter();
        float[,] originalHeights;

        using (FileStream file = File.Open(saveFilePath, FileMode.Open))
        {
            originalHeights = (float[,])bf.Deserialize(file);
        }

        TerrainData terrainData = terrain.terrainData;
        terrainData.SetHeights(0, 0, originalHeights);
        terrain.Flush();

#if UNITY_EDITOR
        UnityEditor.EditorUtility.SetDirty(terrainData);
#endif

        Debug.Log("Terrain has been reset to original heights.");
    }
}
