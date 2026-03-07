using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/InventorySO")]
public class InventorySO : ScriptableObject
{
    private Dictionary<MaterialType, int> m_materials
        = new Dictionary<MaterialType, int>();
    public Dictionary<MaterialType, int> Material { get => m_materials; }

    [SerializeField] private int m_maxAmount = 50;


    public void Add(MaterialType type, int amount)
    {
        if (m_materials.ContainsKey(type))
        {
            m_materials[type] = Mathf.Min(
                m_materials[type] + amount, m_maxAmount);
        }
        else
        {
            m_materials[type] = Mathf.Min(amount, m_maxAmount);
        }
    }

Å@Å@public void Remove(MaterialType type,int amount)
    {
        if (!m_materials.ContainsKey(type)) return;

        m_materials[type] = Mathf.Max(
          m_materials[type] - amount, 0);


        //if (m_materials[type] <= 0)
        //{
        //    m_materials.Remove(type);
        //}
    }

    public int Get(MaterialType type)
    {
        return m_materials.TryGetValue(type, out var value) ? value : 0;
    }

    public IReadOnlyDictionary<MaterialType, int> GetAll()
    {
        return m_materials;
    }

    public void AddMultiple(DropSO drops)
    {
        foreach (var mat in drops.Materials)
        {
            if (mat.amount <= 0) continue;

            Add(mat.type, mat.amount);
        }
    }
}
