using UnityEngine;

public class LevelTrigger : MonoBehaviour
{
    private ProceduralLevelGenerator levelGenerator;
    private Transform player;

    private void Start()
    {
        levelGenerator = FindObjectOfType<ProceduralLevelGenerator>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.root.CompareTag("Player"))
        {
            Vector3 forward = player.forward.normalized;
            levelGenerator.SpawnNextMap(player.position, forward);
        }
    }
}