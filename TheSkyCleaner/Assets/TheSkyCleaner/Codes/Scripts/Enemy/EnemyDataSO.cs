using UnityEngine;
using System.Collections.Generic;
using UnityEditor;

[CreateAssetMenu(menuName = "Scriptable Objects/EnemyDataSO")]
public class EnemyDataSO : ScriptableObject
{
    [SerializeField] private List<EnemySO> m_enemySO = new List<EnemySO>();

    public List<EnemySO> EnemySO { get => m_enemySO; set => m_enemySO = value; }

#if UNITY_EDITOR
    [ContextMenu("Make New")]
    private void MakeNewDamageType()
    {
        EnemySO enemySO = ScriptableObject.CreateInstance<EnemySO>();
        enemySO.name = "New Skill Type";
        enemySO.Initialise(this);

        m_enemySO.Add(enemySO);

        AssetDatabase.AddObjectToAsset(enemySO, this);
        AssetDatabase.SaveAssets();

        EditorUtility.SetDirty(this);
        EditorUtility.SetDirty(enemySO);
    }
#endif
}
