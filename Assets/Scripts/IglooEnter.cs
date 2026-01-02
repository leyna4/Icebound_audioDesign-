using UnityEngine;

public class IglooEnter : MonoBehaviour
{
    public PlayerTemperature playerTemperature;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerTemperature.isInIgloo = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerTemperature.isInIgloo = false;
        }
    }
}
