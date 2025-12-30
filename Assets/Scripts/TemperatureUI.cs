using UnityEngine;
using UnityEngine.UI;

public class TemperatureUI : MonoBehaviour
{
    public Slider temperatureSlider;
    public PlayerTemperature playerTemperature;

    void Update()
    {
        temperatureSlider.value = playerTemperature.currentTemperature;
    }
}

