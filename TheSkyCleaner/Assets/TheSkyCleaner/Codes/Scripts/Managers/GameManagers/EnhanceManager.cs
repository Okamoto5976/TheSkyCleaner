using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class EnhanceManager : MonoBehaviour
{
    [SerializeField] private SaveManager m_savemanager;
    [SerializeField] private Skillget m_skillget;

    [SerializeField] private StringContainer m_mainScene;

    private void Start()
    {
        m_skillget.LoadSkillType();
    }

    private void Update()
    {
        //if (Keyboard.current.tKey.wasPressedThisFrame)
        //{
        //    m_skillget.SaveSkillType();
        //    SceneManager.LoadScene(0);
        //}
    }

    public void MoveToMainScene()
    {
        SceneManager.LoadScene(m_mainScene.Value);

    }
}
