using UnityEngine;

public class IceStalkerAudio : MonoBehaviour
{
    public AudioSource audioSource;
    public Transform player;
    public float maxDistance = 6f;

    void Update()
    {
        float distance = Vector2.Distance(transform.position, player.position);

        if (distance <= maxDistance)
        {
            if (!audioSource.isPlaying)
                audioSource.Play();

            audioSource.volume = 1 - (distance / maxDistance);
        }
        else
        {
            if (audioSource.isPlaying)
                audioSource.Stop();
        }
    }
}

