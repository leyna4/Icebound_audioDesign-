using UnityEngine;

public class IcePlatform : MonoBehaviour
{
    public int chunkCount = 0;
    public float sizeIncrease = 0.3f;

    public void AddChunk()
    {
        chunkCount++;

        transform.localScale +=
            new Vector3(sizeIncrease, sizeIncrease, 0);

        Debug.Log("Platform büyüdü. Chunk: " + chunkCount);
    }
}
