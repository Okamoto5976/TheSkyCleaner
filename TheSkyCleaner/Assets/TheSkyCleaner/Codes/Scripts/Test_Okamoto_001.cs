using UnityEngine;

public class Test_Okamoto_001 : MonoBehaviour
{
    [SerializeField] private SkillDataSO skilldata;

    private void Start()
    {
        float value = skilldata.SkillSO[0].UpdataValue;
    }
}
