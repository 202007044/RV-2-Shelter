using UnityEngine;

public class SoundPlayer : MonoBehaviour
{
    private AudioSource audioSource;
    
    void Start()
    {
        // Get the AudioSource component
        audioSource = GetComponent<AudioSource>();
        audioSource.volume = 30.0f;
    }

    public void PlaySound()
    {
        if (audioSource != null)
        {
            audioSource.Play();
        }
    }
}
