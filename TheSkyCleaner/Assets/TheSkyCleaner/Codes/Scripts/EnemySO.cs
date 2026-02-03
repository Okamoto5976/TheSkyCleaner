using UnityEditor;
using UnityEngine;

public enum EnemyType
{
    a,
    b,
    c
}

[CreateAssetMenu(menuName = "Scriptable Objects/EnemySO")]
public class EnemySO : ScriptableObject
{
    [SerializeField] private EnemyDataSO m_enemydata;
    [SerializeField] private DropSO m_dropSO;

    [SerializeField] private EnemyType m_enemytype;//•K—v‚©•s–¾
    [SerializeField] private string m_enemyname;
    [SerializeField] private int m_attack;

    public EnemyDataSO EnemyData { get => m_enemydata; }
    public DropSO Drop { get => m_dropSO; }
    public EnemyType EnemyType { get => m_enemytype; }
    public string Enemyname { get => m_enemyname; }
    public int Attack { get => m_attack; }

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