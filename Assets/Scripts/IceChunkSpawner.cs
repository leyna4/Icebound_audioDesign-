using UnityEngine;

public class IceChunkSpawner : MonoBehaviour
{
    [Header("References")]
    public GameObject iceChunkPrefab;
    public Transform mainIcePlatform;

    [Header("Spawn Timing")]
    public float spawnInterval = 4.5f;   //  DAHA SEYREK
    public int maxIceOnScreen = 3;        //  DAHA AZ

    [Header("Screen Bounds")]
    public float minX = -7f;
    public float maxX = 7f;
    public float spawnYOffset = -6f;

    [Header("Main Ice Safe Zone")]
    public float mainIceHalfWidth = 1.5f;
    public float sideMargin = 1.5f;
    public float safeDistanceFromMainIce = 1.5f;

    [Header("Spacing Control")]
    public float minVerticalDistance = 1.8f; //  ice'ler arasý min mesafe

    void Start()
    {
        InvokeRepeating(nameof(SpawnIce), 1f, spawnInterval);
    }

    void SpawnIce()
    {
        // Ekranda fazla ice varsa spawnlama
        if (GameObject.FindGameObjectsWithTag("IceChunk").Length >= maxIceOnScreen)
            return;

        // Son spawnlanan ice'le mesafe kontrolü
        GameObject[] ices = GameObject.FindGameObjectsWithTag("IceChunk");
        foreach (GameObject ice in ices)
        {
            if (Mathf.Abs(ice.transform.position.y - spawnYOffset) < minVerticalDistance)
            {
                return; // Çok yakýn  spawn iptal
            }
        }

        bool spawnLeft = Random.value > 0.5f;
        float mainIceX = mainIcePlatform.position.x;

        float spawnX;

        if (spawnLeft)
        {
            spawnX = Random.Range(
                minX,
                mainIceX - mainIceHalfWidth - sideMargin
            );
        }
        else
        {
            spawnX = Random.Range(
                mainIceX + mainIceHalfWidth + sideMargin,
                maxX
            );
        }

        float spawnY = Mathf.Min(
            spawnYOffset,
            mainIcePlatform.position.y - safeDistanceFromMainIce
        );

        Vector3 spawnPos = new Vector3(spawnX, spawnY, 0f);

        GameObject iceObj = Instantiate(iceChunkPrefab, spawnPos, Quaternion.identity);

        IceChunk chunk = iceObj.GetComponent<IceChunk>();
        if (chunk != null)
        {
            chunk.mainIcePlatform = mainIcePlatform;
            chunk.mainIceHalfWidth = mainIceHalfWidth;
        }
    }
}
