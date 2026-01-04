using UnityEngine;

public class IcePlatform : MonoBehaviour
{
    public float growAmount = 0.15f;

    public void Grow()
    {
        transform.localScale += new Vector3(growAmount, growAmount, 0f);

    }
}
