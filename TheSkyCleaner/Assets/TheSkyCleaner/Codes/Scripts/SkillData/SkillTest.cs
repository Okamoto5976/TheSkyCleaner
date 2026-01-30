using UnityEngine;
using UnityEngine.InputSystem;

public class SkillTest : MonoBehaviour
{
    [SerializeField] private InventorySO m_inventory;

    private void Update()
    {

        if (Keyboard.current.rKey.wasPressedThisFrame)
        {
            m_inventory.Add(MaterialType.Thread, 10);
            m_inventory.Add(MaterialType.Cloth, 10);
            m_inventory.Add(MaterialType.Wood, 10);


            foreach (var obj in m_inventory.GetAll())
            {
                Debug.Log($"{obj.Key}‚ğ{obj.Value}ŒÂŠ");
            }
        }
    }
}
