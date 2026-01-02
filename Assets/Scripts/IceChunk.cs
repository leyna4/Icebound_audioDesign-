using UnityEngine;
using System.Collections;

public class IceChunk : MonoBehaviour
{
    public bool hasEnemy;
    public GameObject enemyPrefab;
    public float enemySpawnDelay = 2f;

    public void AttachToPlatform(Transform platform)
    {
        Debug.Log("AttachToPlatform çaðrýldý");

        transform.SetParent(platform);
        transform.position = platform.position;

        IcePlatform icePlatform = platform.GetComponent<IcePlatform>();
        if (icePlatform != null)
        {
            icePlatform.AddChunk();
        }
        else
        {
            Debug.LogError("IcePlatform script YOK!");
        }

        if (hasEnemy && enemyPrefab != null)
        {
            StartCoroutine(SpawnEnemyDelayed());
        }
    }

    IEnumerator SpawnEnemyDelayed()
    {
        Debug.Log("Enemy spawn coroutine baþladý");
        yield return new WaitForSeconds(enemySpawnDelay);
        Instantiate(enemyPrefab, transform.position, Quaternion.identity);
    }
}
