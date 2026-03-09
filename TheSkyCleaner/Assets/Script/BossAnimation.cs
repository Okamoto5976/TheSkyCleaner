using UnityEngine;
using UnityEngine.SceneManagement;

public class BossAnimation : MonoBehaviour
{
    [SerializeField] private Animator m_seni;
    [SerializeField] private SaveManager m_save;
    //[SerializeField] private GameObject m_boss;

    [Header("ƒ{ƒ^ƒ“‰Ÿ‚µ‚Ä‰æ–Ê‘JˆÚ‚·‚éŽžŠÔ")]
    [SerializeField] private float m_seniInterval;
    [SerializeField] private string m_tranName;
   

    private void Awake()
    {
        m_seni.SetBool(m_tranName, false);
        m_save.StartResetData();
    }

    public void ChangeScene()
    {


        Invoke("change_button", m_seniInterval);
      
    }

    public void change_button()
    {
        SceneManager.LoadScene("MainScene");
    }

   public void changeAnimation()
    {
        m_seni.SetBool(m_tranName, true);
    }

   



}