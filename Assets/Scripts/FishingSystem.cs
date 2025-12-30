using UnityEngine;

public class FishingSystem : MonoBehaviour
{
    public float minCatchTime = 5f;
    public float maxCatchTime = 10f;

    private float catchTimer;
    private bool fishReady = false;

    void Start()
    {
        SetNewTimer();
    }

    void Update()
    {
        if (fishReady) return;

        catchTimer -= Time.deltaTime;

        if (catchTimer <= 0)
        {
            fishReady = true;
            Debug.Log("Fish caught!");
            // BURAYA SES EKLENECEK
        }
    }

    void SetNewTimer()
    {
        catchTimer = Random.Range(minCatchTime, maxCatchTime);
        fishReady = false;
    }

    public bool IsFishReady()
    {
        return fishReady;
    }

    public void CollectFish()
    {
        fishReady = false;
        SetNewTimer();
    }
}
