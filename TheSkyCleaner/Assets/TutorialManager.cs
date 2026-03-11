using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;


public class TutorialManager : MonoBehaviour
{
    public GameObject tutorialPanel; // チュートリアル画面全体
    public GameObject m_menuPanel;
    public Image displayImage;       // 画像を表示するコンポーネント
    public Sprite[] pages;           // 説明画像の配列（Inspectorで登録）
    [SerializeField] private GameMenuManager m_gameMenu;

    private int currentIndex = 0;    // 現在何ページ目か

    private bool m_canCloseHelp = false;

    void Start()
    {
        // 起動時はパネルを閉じておく
        tutorialPanel.SetActive(false);
    }

    private void Update()
    {
        if (!m_canCloseHelp) return;

        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            CloseTutorial();
        }


    }

    // HELPボタンを押した時
    public void OpenTutorial()
    {
        currentIndex = 0;
        UpdateUI();

        m_canCloseHelp = true;
        m_gameMenu.m_canCloseMenu = false;

        m_menuPanel.SetActive(false);
        tutorialPanel.SetActive(true);
    }

    // 右矢印ボタン
    public void NextPage()
    {
        if (currentIndex < pages.Length - 1)
        {
            currentIndex++;
            UpdateUI();
        }
    }

    // 左矢印ボタン
    public void PrevPage()
    {
        if (currentIndex > 0)
        {
            currentIndex--;
            UpdateUI();
        }
    }

    // 戻る（閉じる）ボタン
    public void CloseTutorial()
    {
        m_gameMenu.m_canCloseMenu = true;
        m_canCloseHelp = false;

        m_menuPanel.SetActive(true);
        tutorialPanel.SetActive(false);
    }

    // 表示を更新する
    void UpdateUI()
    {
        displayImage.sprite = pages[currentIndex];
    }
}