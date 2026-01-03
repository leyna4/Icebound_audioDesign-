using UnityEngine;
using System.Collections;

public class IceChunk : MonoBehaviour
{
    public bool hasEnemy;
    public GameObject enemyPrefab;
    public float enemySpawnDelay = 2f;

    bool isAttached = false;

    public void StickToHook(Vector3 hookPos)
    {
        transform.position = hookPos;
    }

    public void AttachToPlatform(Transform platform)
    {
        if (isAttached) return;
        isAttached = true;

        transform.SetParent(platform);

        // platformun saðýna ekle
        int index = platform.childCount - 1;
        float offsetX = index * 1.5f;

        transform.localPosition = new Vector3(offsetX, 0f, 0f);

        if (hasEnemy && enemyPrefab != null)
        {
            StartCoroutine(SpawnEnemy());
        }
    }

    IEnumerator SpawnEnemy()
    {
        yield return new WaitForSeconds(enemySpawnDelay);
        Instantiate(enemyPrefab, transform.position, Quaternion.identity);
    }
}
