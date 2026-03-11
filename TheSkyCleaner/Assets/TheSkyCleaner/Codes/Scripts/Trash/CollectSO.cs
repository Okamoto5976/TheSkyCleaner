using UnityEditor;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Data/CollectSO")]
public class CollectSO : ScriptableObject
{
    [SerializeField] private CollectDataSO m_collectdata;
    [SerializeField] private DropSO m_dropSO;

    [SerializeField] private string m_collectname;

    [SerializeField] private int m_attack;
    [SerializeField] private int m_hp;

    public string Collectname { get => m_collectname; }
    public DropSO Drop { get => m_dropSO; }
    public int Attack { get => m_attack; }
    public int HP { get => m_hp; }

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
