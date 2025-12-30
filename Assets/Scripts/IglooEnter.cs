using UnityEngine;

public class IglooEnter : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player iglooya girdi");

            // burada:
            // - vücut ýsýsýný doldur
            // - dýþ sesleri kapat
            // - iç mekân seslerini aç
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player igloodan çýktý");

            // burada:
            // - vücut ýsýsý tekrar düþmeye baþlar
            // - dýþ sesleri aç
        }
    }
}

