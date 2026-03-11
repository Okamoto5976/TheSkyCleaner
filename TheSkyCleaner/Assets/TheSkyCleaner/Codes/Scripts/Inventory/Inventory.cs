using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class Inventory : MonoBehaviour
{
    [System.Serializable]
    public struct MaterialText
    {
        [SerializeField] private TextMeshProUGUI m_text;
        [SerializeField] private MaterialType m_materialType;

        public TextMeshProUGUI Text { get => m_text; }
        public MaterialType Material {  get => m_materialType; }
    }

    [SerializeField] private List<MaterialText> m_inventoryText;
    
    [SerializeField] private InventorySO m_inventorySO;

    private void Update()
    {
        foreach(var value in m_inventoryText)
        {
            if(m_inventorySO.Material.TryGetValue(value.Material, out int amount))
            {
                value.Text.text = amount.ToString();
            }
            else
            {
                value.Text.text = "0";
            }
        }
    }
}
