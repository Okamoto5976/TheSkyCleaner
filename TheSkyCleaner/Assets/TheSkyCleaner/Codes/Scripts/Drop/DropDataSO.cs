using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/DropDataSO")]
public class DropDataSO : ScriptableObject
{
    [SerializeField] private List<DropSO> m_dropSO = new List<DropSO>();

    public List<DropSO> DropSO { get => m_dropSO; set => m_dropSO = value; }

#if UNITY_EDITOR
    [ContextMenu("Make New")]
    private void MakeNewDamageType()
    {
        DropSO dropSO = ScriptableObject.CreateInstance<DropSO>();
        dropSO.name = "New Skill Type";
        dropSO.Initialise(this);

        m_dropSO.Add(dropSO);

        AssetDatabase.AddObjectToAsset(dropSO, this);
        AssetDatabase.SaveAssets();

        EditorUtility.SetDirty(this);
        EditorUtility.SetDirty(dropSO);
    }
#endif
}
