using UnityEngine;

public class AmbienceController : MonoBehaviour
{
    public AudioSource windAudio;
    public PlayerTemperature playerTemperature;

    void Update()
    {
        if (playerTemperature.isInIgloo)
        {
            if (windAudio.isPlaying)
                windAudio.Stop();
        }
        else
        {
            if (!windAudio.isPlaying)
                windAudio.Play();
        }
    }
}
