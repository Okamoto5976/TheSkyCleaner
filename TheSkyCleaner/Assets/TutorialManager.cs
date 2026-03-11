using UnityEngine;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
    public GameObject tutorialPanel; // チュートリアル画面全体
    public Image displayImage;       // 画像を表示するコンポーネント
    public Sprite[] pages;           // 説明画像の配列（Inspectorで登録）

    private int currentIndex = 0;    // 現在何ページ目か

    void Start()
    {
        // 起動時はパネルを閉じておく
        tutorialPanel.SetActive(false);
    }

    // HELPボタンを押した時
    public void OpenTutorial()
    {
        currentIndex = 0;
        UpdateUI();
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
        tutorialPanel.SetActive(false);
    }

    // 表示を更新する
    void UpdateUI()
    {
        displayImage.sprite = pages[currentIndex];
    }
}