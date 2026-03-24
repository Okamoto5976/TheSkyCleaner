using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class GameOption : MonoBehaviour
{
    [SerializeField] private GameObject m_optionUI;

    [SerializeField] private Slider m_bgmSlider;
    [SerializeField] private Slider m_seSlider;
    [SerializeField] private SaveManager m_save;

    [SerializeField] private AudioSource m_audioSource;
    [SerializeField] private AudioContainer m_buttonSound;

    public bool m_canCloseMenu = true;

    void Start()
    {
        m_optionUI.SetActive(false);
        Time.timeScale = 1.0f;

        m_canCloseMenu = true;
    }

    void Update()
    {
        if (m_canCloseMenu == false) return;

        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            ESCpussyu();

        }
    }

    private void ESCpussyu()
    {

        //ポーズが開いていたらポーズ消してゲームに戻る
        if (m_optionUI.activeSelf)
        {
            ReturnGame();
        }
        //何も開いていなかったらポーズ画面を開く
        else
        {
            OpenPose();
        }
    }

    public void OpenPose()
    {
        m_audioSource.PlayOneShot(m_buttonSound.AudioClip, m_buttonSound.Volume);


        m_optionUI.SetActive(true);//ポーズ開く
        SetSliderVolume();


        Time.timeScale = 0f;//止める
    }

    public void ReturnGame()
    {
        m_audioSource.PlayOneShot(m_buttonSound.AudioClip, m_buttonSound.Volume);


        Debug.Log("ゲームに戻る");
        m_optionUI.SetActive(false);//一応念のため


        Time.timeScale = 1f;//時は動き出す
    }

    private void SetSliderVolume()
    {
        Debug.Log("SetSlider");
        var loadAudio = m_save.AudioLoad();
        m_bgmSlider.value = loadAudio.data.BGMVolume;
        m_seSlider.value = loadAudio.data.SEVolume;
    }
}
