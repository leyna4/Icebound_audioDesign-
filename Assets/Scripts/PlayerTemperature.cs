using UnityEngine;

public class PlayerTemperature : MonoBehaviour
{
    public float maxTemperature = 100f;
    public float currentTemperature;

    [Header("Cold Settings")]
    public float coldDelay = 5f;          // Dýþarýda ne kadar sonra soðuk baþlasýn
    public float coldDecreaseRate = 5f;   // Maksimum düþüþ hýzý

    [Header("Heat Settings")]
    public float heatIncreaseRate = 20f;

    private float timeOutside = 0f;
    public bool isInIgloo = false;

    void Start()
    {
        currentTemperature = maxTemperature;
    }

    void Update()
    {
        // IGLOO ÝÇÝ — MUTLAK ÖNCELÝK
        if (isInIgloo)
        {
            timeOutside = 0f;

            currentTemperature += heatIncreaseRate * Time.deltaTime;
            currentTemperature = Mathf.Clamp(currentTemperature, 0, maxTemperature);

            return; //ÇOK ÖNEMLÝ: dýþarý kodu ASLA çalýþmasýn
        }

        //  DIÞARISI
        timeOutside += Time.deltaTime;

        if (timeOutside > coldDelay)
        {
            float coldProgress = (timeOutside - coldDelay) / coldDelay;
            coldProgress = Mathf.Clamp01(coldProgress);

            float currentColdRate = coldDecreaseRate * coldProgress;
            currentTemperature -= currentColdRate * Time.deltaTime;
        }

        currentTemperature = Mathf.Clamp(currentTemperature, 0, maxTemperature);
    }
}
