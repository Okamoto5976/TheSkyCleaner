using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

public class Test_Okamoto_001 : MonoBehaviour
{
    [SerializeField] private SkillDataSO skilldata;
    [SerializeField] private UnityEvent<AudioSO> onPlaySE;
    [SerializeField] private AudioSO SE;
    [SerializeField] private Inventory m_inventory;

    private void Start()
    {
        //float value = skilldata.SkillSO[0].UpdataValue;
    }

    private void Update()
    {
        if (Keyboard.current.tKey.wasPressedThisFrame)
        {
            onPlaySE.Invoke(SE);
        }

        if (Keyboard.current.rKey.wasPressedThisFrame)
        {
            foreach(var obj in m_inventory.GetAll())
            {
                Debug.Log($"{obj.Key}Ç{obj.Value}å¬èäéù");
            }
        }
    }
}
