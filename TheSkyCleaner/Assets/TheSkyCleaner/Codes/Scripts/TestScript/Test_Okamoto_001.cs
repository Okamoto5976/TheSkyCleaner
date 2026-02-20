using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

public class Test_Okamoto_001 : MonoBehaviour
{
    [SerializeField] private SkillDataSO skilldata;
    [SerializeField] private UnityEvent<AudioSO> onPlaySE;
    [SerializeField] private AudioSO SE;
    [SerializeField] private InventorySO m_inventory;

    private void Start()
    {
        //float value = skilldata.SkillSO[0].UpdataValue;
    }

    private void Update()
    {
        //if (Keyboard.current.tKey.wasPressedThisFrame)
        //{
        //    onPlaySE.Invoke(SE);
        //}

        if (Keyboard.current.rKey.wasPressedThisFrame)
        {
            m_inventory.Add(MaterialType.Thread, 10);
            m_inventory.Add(MaterialType.Cloth, 10);
            m_inventory.Add(MaterialType.Wood, 10);


            foreach (var obj in m_inventory.GetAll())
            {
                Debug.Log($"{obj.Key}Ç{obj.Value}å¬èäéù");
            }
        }
    }
}
