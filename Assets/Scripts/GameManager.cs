using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public int fishCount = 0;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void AddFish(int amount)
    {
        fishCount += amount;
        Debug.Log("Fish: " + fishCount);
    }
}

