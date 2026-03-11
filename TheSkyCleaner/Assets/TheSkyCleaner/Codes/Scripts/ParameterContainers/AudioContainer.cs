using UnityEngine;

[CreateAssetMenu(fileName = "AudioContainer", menuName = "Scriptable Objects/AudioContainer")]
public class AudioContainer : ScriptableObject
{
    [SerializeField] private AudioClip[] m_audioClip;
    [SerializeField] private float m_volume;

    public AudioClip AudioClip => m_audioClip[Random.Range(0, m_audioClip.Length)];
    public float Volume => m_volume;
}
