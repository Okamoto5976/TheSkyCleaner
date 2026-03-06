using UnityEngine;

public class AudioHandler : MonoBehaviour
{
    private AudioSource m_AudioSource;
    private void Awake()
    {
        m_AudioSource = GetComponent<AudioSource>();
    }

    public void PlayOneShot(AudioContainer container)
    {
        m_AudioSource.PlayOneShot(container.AudioClip, container.Volume);
    }
}
