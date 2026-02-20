using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class EnhanceManager : MonoBehaviour
{
    [SerializeField] private SaveManager m_savemanager;
    [SerializeField] private Skillget m_skillget;

    private void Start()
    {
        m_skillget.LoadSkillType();
    }

    private void Update()
    {
        if (Keyboard.current.tKey.wasPressedThisFrame)
        {
            m_skillget.SaveSkillType();
            SceneManager.LoadScene(0);
        }
    }
}
