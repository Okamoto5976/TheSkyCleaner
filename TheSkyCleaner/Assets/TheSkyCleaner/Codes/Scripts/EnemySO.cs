using UnityEditor;
using UnityEngine;

public enum EnemyType
{
    a,
    b,
    c
}

[CreateAssetMenu(fileName = "EnemySO", menuName = "Scriptable Objects/EnemySO")]
public class EnemySO : ScriptableObject
{
    [SerializeField] private EnemyDataSO m_enemydata;
    [SerializeField] private EnemyType m_enemytype;

    [SerializeField] private string m_enemyname;
    [SerializeField] private Material m_material;

    public string Enemyname { get => m_enemyname; }
    public EnemyDataSO EnemyData { get => m_enemydata; }

    [System.Serializable]
    public class Material
    {
        [Header("Ž…"), SerializeField] private int m_material_1;
        [Header("•z"), SerializeField] private int m_material_2;
        [Header("”Â"), SerializeField] private int m_material_3;


        public int Mterial1 { get => m_material_1; }
        public int Mterial2 { get => m_material_2; }
        public int Mterial3 { get => m_material_3; }

    }

#if UNITY_EDITOR
    public void Initialise(EnemyDataSO enemydata)
    {
        m_enemydata = enemydata;
    }
#endif

#if UNITY_EDITOR
    [ContextMenu("Rename to name")]
    private void Rename()
    {
        this.name = m_enemyname;
        AssetDatabase.SaveAssets();
        EditorUtility.SetDirty(this);
    }
#endif

#if UNITY_EDITOR
    [ContextMenu("Delete this")]
    private void DeleteThis()
    {
        m_enemydata.EnemySO.Remove(this);
        Undo.DestroyObjectImmediate(this);
        AssetDatabase.SaveAssets();
    }
#endif
}
