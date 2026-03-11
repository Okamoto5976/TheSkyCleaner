using UnityEditor;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Data/EnemySO")]
public class EnemySO : ScriptableObject
{
    [SerializeField] private EnemyDataSO m_enemydata;
    [SerializeField] private DropSO m_dropSO;

    [SerializeField] private string m_enemyname;
    [SerializeField] private int m_attack;
    [SerializeField] private int m_hp;

    public EnemyDataSO EnemyData { get => m_enemydata; }
    public DropSO Drop { get => m_dropSO; }
    public string Enemyname { get => m_enemyname; }
    public int Attack { get => m_attack; }
    public int HP { get => m_hp; }

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