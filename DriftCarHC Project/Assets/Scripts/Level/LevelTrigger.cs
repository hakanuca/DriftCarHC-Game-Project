using UnityEngine;

public class LevelTrigger : MonoBehaviour
{
    private ProceduralLevelGenerator levelGenerator;
    private Transform player;

    private void Start()
    {
        levelGenerator = FindObjectOfType<ProceduralLevelGenerator>();
        player = GameObject.FindGameObjectWithTag("Player").transform;

        DisableAllBlocks();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.root.CompareTag("Player"))
        {
            Debug.Log("Player triggered the level gate.");
            Vector3 forward = player.forward.normalized;
            levelGenerator.SpawnNextMap(player.position, forward);
            EnableAllBlocks();
        }
    }

    private void DisableAllBlocks()
    {
        GameObject[] blocks = GameObject.FindGameObjectsWithTag("Block");
        Debug.Log("Disabling blocks: " + blocks.Length);
        foreach (GameObject block in blocks)
        {
            block.SetActive(false);
        }
    }

    private void EnableAllBlocks()
    {
        GameObject[] blocks = GameObject.FindGameObjectsWithTag("Block");
        Debug.Log("Enabling blocks: " + blocks.Length);
        foreach (GameObject block in blocks)
        {
            block.SetActive(true);
        }
    }
}