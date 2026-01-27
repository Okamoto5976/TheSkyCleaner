using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/CollectDataSO")]
public class CollectDataSO : ScriptableObject
{
    [SerializeField] private List<CollectSO> m_collectSO = new List<CollectSO>();

    public List<CollectSO> CollectSO { get => m_collectSO; set => m_collectSO = value; }

#if UNITY_EDITOR
    [ContextMenu("Make New")]
    private void MakeNewDamageType()
    {
        CollectSO collectSO = ScriptableObject.CreateInstance<CollectSO>();
        collectSO.name = "New Skill Type";
        collectSO.Initialise(this);

        m_collectSO.Add(collectSO);

        AssetDatabase.AddObjectToAsset(collectSO, this);
        AssetDatabase.SaveAssets();

        EditorUtility.SetDirty(this);
        EditorUtility.SetDirty(collectSO);
    }
#endif
}
