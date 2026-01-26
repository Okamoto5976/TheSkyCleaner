using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/AudioDataSO")]
public class AudioDataSO : ScriptableObject
{
    [SerializeField] private List<AudioSO> m_audioSO = new List<AudioSO>();

    public List<AudioSO> AudioSO { get => m_audioSO; set => m_audioSO = value; }

#if UNITY_EDITOR
    [ContextMenu("Make New")]
    private void MakeNewDamageType()
    {
        AudioSO audioSO = ScriptableObject.CreateInstance<AudioSO>();
        audioSO.name = "New Skill Type";
        audioSO.Initialise(this);

        m_audioSO.Add(audioSO);

        AssetDatabase.AddObjectToAsset(audioSO, this);
        AssetDatabase.SaveAssets();

        EditorUtility.SetDirty(this);
        EditorUtility.SetDirty(audioSO);
    }
#endif
}
