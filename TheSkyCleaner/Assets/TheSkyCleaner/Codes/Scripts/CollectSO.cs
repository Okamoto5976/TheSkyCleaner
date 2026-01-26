using UnityEditor;
using UnityEngine;

public enum CollectType
{
    a,
    b,
    c
}


[CreateAssetMenu(menuName = "Scriptable Objects/CollectSO")]
public class CollectSO : ScriptableObject
{
    [SerializeField] private CollectDataSO m_collectdata;

    [SerializeField] private CollectType m_collecttype;//必要か不明
    [SerializeField] private string m_collectname;
    [SerializeField] private int m_attack;

    [SerializeField] private Material m_material;

    public CollectType CollectType { get => m_collecttype; }
    public string Collectname { get => m_collectname; }
    public int Attack { get => m_attack; }

    public CollectDataSO CollectData { get => m_collectdata; }

    [System.Serializable]
    public class Material
    {
        [Header("糸"), SerializeField] private int m_material_1;
        [Header("布"), SerializeField] private int m_material_2;
        [Header("板"), SerializeField] private int m_material_3;


        public int Material1 { get => m_material_1; }
        public int Material2 { get => m_material_2; }
        public int Material3 { get => m_material_3; }

    }

#if UNITY_EDITOR
    public void Initialise(CollectDataSO collectdata)
    {
        m_collectdata = collectdata;
    }
#endif

#if UNITY_EDITOR
    [ContextMenu("Rename to name")]
    private void Rename()
    {
        this.name = m_collectname;
        AssetDatabase.SaveAssets();
        EditorUtility.SetDirty(this);
    }
#endif

#if UNITY_EDITOR
    [ContextMenu("Delete this")]
    private void DeleteThis()
    {
        m_collectdata.CollectSO.Remove(this);
        Undo.DestroyObjectImmediate(this);
        AssetDatabase.SaveAssets();
    }
#endif
}
