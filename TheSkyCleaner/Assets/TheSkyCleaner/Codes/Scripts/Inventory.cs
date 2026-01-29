using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    private Dictionary<MaterialType, int> m_materials 
        = new Dictionary<MaterialType, int>();

    public void Add(MaterialType type , int amount)
    {
        if(m_materials.ContainsKey(type))
        {
            m_materials[type] += amount;
        }
        else
        {
            m_materials.Add(type, amount);
        }
    }

    public int Get(MaterialType type)
    {
        return m_materials.TryGetValue(type, out var value) ? value : 0;
    }

    public IReadOnlyDictionary<MaterialType, int> GetAll()
    {
        return m_materials;
    }
}
