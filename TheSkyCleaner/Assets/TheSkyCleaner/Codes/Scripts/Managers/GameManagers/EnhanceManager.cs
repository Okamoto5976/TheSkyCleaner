using UnityEngine;

public class EnhanceManager : MonoBehaviour
{
    [SerializeField] private SaveManager m_savemanager;
    [SerializeField] private FadeManager m_fadeManager;
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
        m_skillget.SaveSkillType();
        m_fadeManager.ChangeScene(m_mainScene.Value, false);

    }
}
