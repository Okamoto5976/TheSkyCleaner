using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/AudioSource")]
public class AudioSourceSO : ScriptableObject
{
    public List<AudioSourceMusic> BGMList;
    public List<AudioSourceMusic> SEList;
    [System.Serializable]
    public class AudioSourceMusic
    {
        [SerializeField] string audiosource;
        [SerializeField] AudioClip clip;
        [SerializeField] float volum;
        [SerializeField] bool loop;

        public string Audiosource { get => audiosource; }
        public AudioClip Clip { get => clip; }
        public float Volum { get => volum; }
        public bool Loop { get => loop; }

    }
}
