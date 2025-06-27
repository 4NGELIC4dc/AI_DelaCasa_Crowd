using UnityEngine;
using UnityEngine.AI;

public class CrowdSpawnner : MonoBehaviour
{
    [Header("Agent Prefabs")]
    public GameObject[] npcPrefabs; // Array for different NPC prefabs

    [Header("Spawn Settings")]
    public int numberOfAgents = 50;
    public float spawnRadius = 20f;

    private void Start()
    {
        for (int i = 0; i < numberOfAgents; i++)
        {
            SpawnRandomAgent();
        }
    }

    void SpawnRandomAgent()
    {
        if (npcPrefabs.Length == 0)
        {
            Debug.LogWarning("No NPC prefabs assigned in CrowdSpawnner.");
            return;
        }

        // Pick random prefab from the list
        GameObject prefab = npcPrefabs[Random.Range(0, npcPrefabs.Length)];

        // Pick random point around the spawner within spawnRadius
        Vector3 randomPos = transform.position + Random.insideUnitSphere * spawnRadius;
        randomPos.y = 0f;

        // Check if it is on the NavMesh (optional but recommended)
        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomPos, out hit, 2f, NavMesh.AllAreas))
        {
            Instantiate(prefab, hit.position, Quaternion.identity);
        }
    }
}
