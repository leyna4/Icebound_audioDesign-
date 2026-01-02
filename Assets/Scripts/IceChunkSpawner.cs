using UnityEngine;

public class IceChunkSpawner : MonoBehaviour
{
    public GameObject iceChunkPrefab;
    public float spawnInterval = 8f;
    public float spawnRadius = 8f;
    public int safeChunksAtStart = 3;

    int spawnedCount = 0;

    void Start()
    {
        InvokeRepeating(nameof(SpawnIceChunk), spawnInterval, spawnInterval);
    }

    void SpawnIceChunk()
    {
        Vector2 pos = (Vector2)transform.position +
                      Random.insideUnitCircle.normalized * spawnRadius;

        GameObject chunkObj = Instantiate(iceChunkPrefab, pos, Quaternion.identity);
        IceChunk chunk = chunkObj.GetComponent<IceChunk>();

        chunk.hasEnemy = spawnedCount >= safeChunksAtStart && Random.value < 0.3f;
        spawnedCount++;

        Debug.Log("IceChunk spawnlandý. Enemy var mý: " + chunk.hasEnemy);
    }
}
