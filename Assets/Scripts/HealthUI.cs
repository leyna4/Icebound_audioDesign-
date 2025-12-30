using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour
{
    public Slider healthSlider;
    public PlayerHealth playerHealth;

    void Update()
    {
        healthSlider.value = playerHealth.currentHealth;
    }
}
