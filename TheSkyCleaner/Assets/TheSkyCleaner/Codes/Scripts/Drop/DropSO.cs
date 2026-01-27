using UnityEditor;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/DropSO")]
public class DropSO : ScriptableObject
{
    [SerializeField] private DropDataSO m_dropdata;

    [SerializeField] private string m_dropname;

    public string Dropname { get => m_dropname; }
    public DropDataSO DropData { get => m_dropdata; }

    [Header("Ž…"), SerializeField] private int m_material_1;
    [Header("•z"), SerializeField] private int m_material_2;
    [Header("”Â"), SerializeField] private int m_material_3;


    public int Material1 { get => m_material_1; }
    public int Material2 { get => m_material_2; }
    public int Material3 { get => m_material_3; }


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
