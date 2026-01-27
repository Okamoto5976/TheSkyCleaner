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
    [SerializeField] private DropSO m_dropSO;

    [SerializeField] private CollectType m_collecttype;//•K—v‚©•s–¾
    [SerializeField] private string m_collectname;

    public CollectType CollectType { get => m_collecttype; }
    public string Collectname { get => m_collectname; }
    public DropSO Drop { get => m_dropSO; }


    public CollectDataSO CollectData { get => m_collectdata; }


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
