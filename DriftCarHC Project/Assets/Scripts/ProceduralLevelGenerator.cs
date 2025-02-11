using System.Collections;
using UnityEngine;

public class ProceduralLevelGenerator : MonoBehaviour
{
    [SerializeField] private GameObject roadPrefab;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("RoadTrigger"))
        {
            Instantiate(roadPrefab, new Vector3(0, 0, 0), Quaternion.identity);

        }
    }
    
}
