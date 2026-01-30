using UnityEngine;
using System.Collections.Generic;
using UnityEditor;

[CreateAssetMenu(menuName = "Scriptable Objects/EnemyDataSO")]
public class EnemyDataSO : ScriptableObject
{
    [SerializeField] private List<EnemySO> _enemySO = new List<EnemySO>();

    public List<EnemySO> EnemySO { get => _enemySO; set => _enemySO = value; }

#if UNITY_EDITOR
    [ContextMenu("Make New")]
    private void MakeNewDamageType()
    {
        EnemySO enemySO = ScriptableObject.CreateInstance<EnemySO>();
        enemySO.name = "New Skill Type";
        enemySO.Initialise(this);

        _enemySO.Add(enemySO);

        AssetDatabase.AddObjectToAsset(enemySO, this);
        AssetDatabase.SaveAssets();

        EditorUtility.SetDirty(this);
        EditorUtility.SetDirty(enemySO);
    }
#endif
}
