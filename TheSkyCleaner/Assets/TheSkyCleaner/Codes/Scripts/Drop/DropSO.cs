using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public enum MaterialType
{
    Thread,
    Cloth,
    Wood
}

[Serializable]
public class DropMaterial
{
    public MaterialType type;
    public int amount;
}


[CreateAssetMenu(menuName = "Scriptable Objects/DropSO")]
public class DropSO : ScriptableObject
{
    [SerializeField] private DropDataSO m_dropdata;

    [SerializeField] private string m_dropname;

    [SerializeField] private List<DropMaterial> m_materials;

    public string Dropname { get => m_dropname; }
    public DropDataSO DropData { get => m_dropdata; }
    public IReadOnlyList<DropMaterial> Materials => m_materials;

#if UNITY_EDITOR
    public void Initialise(DropDataSO dropdata)
    {
        m_dropdata = dropdata;
    }
#endif

#if UNITY_EDITOR
    [ContextMenu("Rename to name")]
    private void Rename()
    {
        this.name = m_dropname;
        AssetDatabase.SaveAssets();
        EditorUtility.SetDirty(this);
    }
#endif

#if UNITY_EDITOR
    [ContextMenu("Delete this")]
    private void DeleteThis()
    {
        m_dropdata.DropSO.Remove(this);
        Undo.DestroyObjectImmediate(this);
        AssetDatabase.SaveAssets();
    }
#endif
}
