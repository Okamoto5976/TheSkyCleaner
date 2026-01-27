using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/AudioSO")]
public class AudioSO : ScriptableObject
{
    [SerializeField] private AudioDataSO m_audiodata;

    [SerializeField] private string m_audioname;
    [SerializeField] string audiosource;
    [SerializeField] AudioClip clip;
    [SerializeField] float volum;
    [SerializeField] bool loop;

    public string Audiosource { get => audiosource; }
    public AudioClip Clip { get => clip; }
    public float Volum { get => volum; }
    public bool Loop { get => loop; }
    public string Audioname { get => m_audioname; }
    public AudioDataSO AudioData { get => m_audiodata; }


#if UNITY_EDITOR
    public void Initialise(AudioDataSO audiodata)
    {
        m_audiodata = audiodata;
    }
#endif

#if UNITY_EDITOR
    [ContextMenu("Rename to name")]
    private void Rename()
    {
        this.name = m_audioname;
        AssetDatabase.SaveAssets();
        EditorUtility.SetDirty(this);
    }
#endif

#if UNITY_EDITOR
    [ContextMenu("Delete this")]
    private void DeleteThis()
    {
        m_audiodata.AudioSO.Remove(this);
        Undo.DestroyObjectImmediate(this);
        AssetDatabase.SaveAssets();
    }
#endif
}
