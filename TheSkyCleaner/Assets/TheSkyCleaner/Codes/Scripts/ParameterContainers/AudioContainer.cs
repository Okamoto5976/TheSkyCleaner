using UnityEngine;

[CreateAssetMenu(fileName = "AudioContainer", menuName = "Scriptable Objects/AudioContainer")]
public class AudioContainer : ScriptableObject
{
    [SerializeField] private AudioClip m_audioClip;
    [SerializeField] private float m_volume;

    public AudioClip AudioClip => m_audioClip;
    public float Volume => m_volume;
}
