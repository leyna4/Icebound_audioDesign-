using UnityEngine;

public class HookController : MonoBehaviour
{
    public float hookRange = 10f;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit2D hit = Physics2D.Raycast(
                transform.position,
                transform.right,
                hookRange
            );

            if (hit.collider != null)
            {
                IceChunk chunk = hit.collider.GetComponent<IceChunk>();

                if (chunk != null)
                {
                    chunk.AttachToPlatform(
                        GameObject.Find("IcePlatform").transform
                    );
                }
            }
        }
    }
}
