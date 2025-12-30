using UnityEngine;

public class IglooTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Triggera giren: " + other.name);

        if (other.CompareTag("Player"))
        {
            Debug.Log("PLAYER IGLOOYA GÝRDÝ");
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerTemperature>().isInIgloo = false;
        }
    }
}
