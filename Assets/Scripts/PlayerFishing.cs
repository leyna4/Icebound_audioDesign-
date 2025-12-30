using UnityEngine;

public class PlayerFishing : MonoBehaviour
{
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("FishingRod"))
        {
            FishingSystem fishing = other.GetComponent<FishingSystem>();

            if (fishing != null && fishing.IsFishReady())
            {
                if (Input.GetKeyDown(KeyCode.E))
                {
                    fishing.CollectFish();
                    GameManager.Instance.AddFish(1);
                }
            }
        }
    }
}
