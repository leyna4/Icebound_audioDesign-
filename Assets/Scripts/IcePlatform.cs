using UnityEngine;

public class IcePlatform : MonoBehaviour
{
    public int chunkCount = 0;

    public void AddChunk()
    {
        chunkCount++;
        Debug.Log("IcePlatform chunk sayýsý: " + chunkCount);
    }
}
