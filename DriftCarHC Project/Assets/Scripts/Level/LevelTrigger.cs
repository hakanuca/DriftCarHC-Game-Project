using UnityEngine;

public class LevelTrigger : MonoBehaviour
{
    private ProceduralLevelGenerator levelGenerator;

    private void Start()
    {
        levelGenerator = FindObjectOfType<ProceduralLevelGenerator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.root.CompareTag("Player")) 
        {
            levelGenerator.SpawnNextRoad();
        }
    }
}