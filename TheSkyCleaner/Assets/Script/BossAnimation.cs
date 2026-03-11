using UnityEngine;
using UnityEngine.SceneManagement;

public class BossAnimation : MonoBehaviour
{
    [SerializeField] private Animator m_seni;
    [SerializeField] private SaveManager m_save;
    [SerializeField] private FadeManager m_fadeManager;
    [SerializeField] private InventorySO m_inventory;
    //[SerializeField] private GameObject m_boss;

    [Header("ƒ{ƒ^ƒ“‰Ÿ‚µ‚Ä‰æ–Ê‘JˆÚ‚·‚éŽžŠÔ")]
    [SerializeField] private float m_seniInterval;
    [SerializeField] private string m_tranName;

    [SerializeField] private StringContainer m_mainScene;

    private void Awake()
    {
        m_seni.SetBool(m_tranName, false);
        m_save.StartResetData();
        m_inventory.Reset();
    }

    public void ChangeScene()
    {
        Debug.Log("Start");

        Invoke("change_button", m_seniInterval);
      
    }

    public void change_button()
    {
        m_fadeManager.ChangeScene(m_mainScene.Value, true);
    }

    public void changeAnimation()
    {
        m_seni.SetBool(m_tranName, true);
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}