using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GameMenuManager : MonoBehaviour
{
    [SerializeField] private GameObject m_poseUI;
    [SerializeField] private GameObject m_optionUI;
    [SerializeField] private GameObject m_poseButtonFrame;
    [SerializeField] private GameObject m_optionButtonFrame;

    [SerializeField] private Slider m_bgmSlider;
    [SerializeField] private Slider m_seSlider;

    [SerializeField] private SaveManager m_save;

    private InputAction m_Pause;
    private void Start()
    {
        //全部閉じる時間も進める
        m_poseUI.SetActive(false); 
        m_optionUI.SetActive(false);
        Time.timeScale = 1.0f;
        m_Pause = InputSystem.actions.FindAction("Pause");
    }
    void Update()
    {
        //ESCキーが押された時
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
            ESCpussyu();
        // if (m_Pause.WasPressedThisFrame())
        // {

        // }
        if (m_poseUI.activeSelf)
        {
            //マウスが動いたら選択枠消す
            Vector2 mouseDelta1 = Mouse.current.delta.ReadValue();
            if (mouseDelta1.sqrMagnitude > 0.1f)
            {
                if (EventSystem.current.currentSelectedGameObject != null)
                {
                    EventSystem.current.SetSelectedGameObject(null);
                }
            }
            //選択せれていない時にキーを押したら最初のボタンを選択する
            if (EventSystem.current.currentSelectedGameObject == null)
            {
                if (Keyboard.current.anyKey.wasPressedThisFrame)
                {
                    EventSystem.current.SetSelectedGameObject(m_poseButtonFrame);
                }
            }
        }
        if (m_optionUI.activeSelf)
        {
            //マウスが動いたら選択枠消す
            Vector2 mouseDelta2 = Mouse.current.delta.ReadValue();
            if (mouseDelta2.sqrMagnitude > 0.1f)
            {
                if (EventSystem.current.currentSelectedGameObject != null)
                {
                    EventSystem.current.SetSelectedGameObject(null);
                }
            }
            //選択せれていない時にキーを押したら最初のボタンを選択する
            if (EventSystem.current.currentSelectedGameObject == null)
            {
                if (Keyboard.current.anyKey.wasPressedThisFrame)
                {
                    EventSystem.current.SetSelectedGameObject(m_optionButtonFrame);
                }
            }
        }
    }
    private void ESCpussyu()
    {
        //オプションが開いてるならオプションだけ消してポーズに戻る
        if(m_optionUI.activeSelf)
        {
            
            OptionsClose();
        }
        //ポーズが開いていたらポーズ消してゲームに戻る
        else if (m_poseUI.activeSelf)
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
        m_poseUI.SetActive(true);//ポーズ開く
        Time.timeScale = 0f;//止める
        EventSystem.current.SetSelectedGameObject(m_poseButtonFrame);
    }
    public void ReturnGame()
    {
        Debug.Log("ゲームに戻る");
        m_poseUI.SetActive(false);//ポーズ消す
        m_optionUI.SetActive(false);//一応念のため
        Time.timeScale = 1f;//時は動き出す
    }
    public void OpenOptions()
    {
        Debug.Log("オプション");
        m_poseUI.SetActive(false);//ポーズ画面消す
        m_optionUI.SetActive(true);//オプション出す
        SetSliderVolume();
        EventSystem.current.SetSelectedGameObject(m_optionButtonFrame);

    }
    public void OptionsClose()
    {
        Debug.Log("戻る");
        m_optionUI.SetActive(false);//消す
        m_poseUI.SetActive(true);//ポーズに戻す
        EventSystem.current.SetSelectedGameObject(m_poseButtonFrame);

    }
    public void botan1()
    {
        Debug.Log("スタート!");
    }

    private void SetSliderVolume()
    {
        Debug.Log("SetSlider");
        var loadAudio = m_save.AudioLoad();
        m_bgmSlider.value = loadAudio.data.BGMVolume;
        m_seSlider.value = loadAudio.data.SEVolume;
    }

    public void Back()
    {
        ReturnGame();
    }

    public void Quit()
    {
        
    }
}