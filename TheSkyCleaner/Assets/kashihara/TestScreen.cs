using TMPro;
using System.Collections;
using UnityEngine;
using UnityEditor.Timeline;
using UnityEngine.InputSystem;
// 覇空清掃機
public static class CountRecord
{
    public static int PlayCount;            // プレイ回数
    public static int[] PlayerLevel = new int[3];        // プレイヤーの各部位の強化状況
    public static int BestRecordOfCatch;    // 一度で捕ったゴミの数の最高記録
}

// 背景
public enum BACKGROUND
{
    RESULT,     // リザルト画面
    UPGRADE1,   // アップグレード画面１
    UPGRADE2,   // アップグレード画面２
}

public class TestScreen : MonoBehaviour
{
    public int CurrentScreen; // 現在表示中の画面
    // 0    : タイトル画面
    // 100  : チュートリアル
    // 200  : インゲーム
    // 300  : ポーズ画面
    // 400  : リザルト画面
    // 500  : アップグレード画面

    public int[] MaterialAmount; // 集めた各ごみの量（重さ（g））
    // 0    : 布
    // 1    : 金属
    // 2    : 木材

    public int Score = 999444;  // (仮)スコア
    public int Time = 555;      // (仮)時間

    [SerializeField] private InputAction _action;
    private int m_materialVariety;  // 再利用できるごみの種類
    private int m_maxDisplayMaterialAmount; // 表示するゴミの最大所持量
    [SerializeField] private GameObject m_resultScreen;     // リザルト画面
    [SerializeField] private GameObject m_upgrade1Screen;   // アップグレード画面１（アップグレードをする場所）
    [SerializeField] private GameObject m_upgrade2Screen;   // アップグレード画面２（集めたゴミが置いてある場所）
    [SerializeField] private TextMeshPro[] m_weightText;    // 計りの数字
    [SerializeField] private TextMeshPro[] m_resultText;    // リザルト画面の文字
    [SerializeField] private Sprite[] m_background;         // 各画面の背景
    [SerializeField] private Sprite[] m_bagSprite;          // ゴミ袋のスプライト
    [SerializeField] private Sprite[] m_WeightMeasureSprite;        // 計りのスプライト
    [SerializeField] private GameObject m_displayBackground;        // 表示する背景
    [SerializeField] private GameObject[] m_displayStand;           // 表示する台
    [SerializeField] private GameObject[] m_displayBag;             // 表示するゴミ袋
    [SerializeField] private GameObject[] m_displayWeightMeasure;   // 表示する計り
    [SerializeField] private GameObject m_playerSprite; // 機体のスプライト
    //[SerializeField] private GameObject[] m_playerPart; // 強化する各部位のスプライト
    // 0    : 胴体（耐久）
    // 1    : アーム
    // 2    : ネット
    // 3    : 燃料（補給）
    [SerializeField] private GameObject[] m_button; // 強化、別の画面への遷移等のボタン
    [SerializeField] private GameObject m_cursor;   // カーソルのオブジェクト
    private Vector2[] m_buttonPos = new Vector2[5]; // ボタンの位置
    private int m_screenWidth;  // 取得した画面の横幅を格納する変数
    private int m_screenHeight; // 取得した画面の縦幅を格納する変数
    private Vector2 m_worldSize; // 取得した画面の縦幅を格納する変数
    private float m_cursorPosX; // カーソルのx座標
    private float m_cursorPosY; // カーソルのy座標
    private bool pressDecide;
    private int m_buttonAnimationTime;  // ボタンを押した時の、ボタンのアニメーション時間

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        m_materialVariety = 3;
        m_maxDisplayMaterialAmount = 999999;

        MaterialAmount[0] = 0;
        MaterialAmount[1] = 9997;
        MaterialAmount[2] = 1000000;
        CountRecord.PlayerLevel[0] = 2; // (仮)燃料のレベル
        CountRecord.PlayerLevel[1] = 11; // (仮)アーマーのレベル
        CountRecord.PlayerLevel[2] = 789; // (仮)エネルギー弾のレベル

        m_screenWidth = Screen.width;   // 画面の横幅を取得する
        m_screenHeight = Screen.height; // 画面の縦幅を取得する
        m_worldSize = Camera.main.ScreenToWorldPoint(new Vector2(m_screenWidth, m_screenHeight));
        pressDecide = false;
        m_buttonAnimationTime = 30;
        
        Debug.Log($"Width : {m_screenWidth}, Height : {m_screenHeight}, Size : {m_worldSize}");

        // タイトル画面の表示
        //CurrentScreen = 0;

        // リザルト画面の表示（仮）
        Screen400();

        // アップグレード画面の表示（仮）
        //Screen500();
    }

    private void Update()
    {
        if (PressDecide() == 1) // 決定ボタンが押されたとき
        {
            pressDecide = true;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // タイトル画面
        if (CurrentScreen == 0)
        {

        }
        // リザルト画面
        else if (CurrentScreen == 400)
        {
            ControlScreen400();
        }
        // アップグレード画面１
        else if (CurrentScreen == 500)
        {
            controlScreen500();
        }
        // アップグレード画面２
        else if (CurrentScreen == 501)
        {
            controlScreen501();
        }
    }

    // チュートリアル画面へ遷移
    private void Screen100()
    {
        CurrentScreen = 100;
    }

    // リザルト画面への遷移（仮）
    private void Screen400()
    {
        CurrentScreen = 400;
        m_displayBackground.GetComponent<SpriteRenderer>().sprite = m_background[(int)BACKGROUND.RESULT];
        m_resultScreen.SetActive(true);
        // 文字を表示
        m_resultText[0].text = "Score  ";
        m_resultText[1].text = "Time  ";
        m_resultText[2].text = "Aircraft Level";
        m_resultText[3].text = "Fuel";
        m_resultText[4].text = "Armor";
        m_resultText[5].text = "EneBul";
        m_resultText[6].text = Score.ToString("N0");
        m_resultText[7].text = Time.ToString("N0");
        m_resultText[8].text = "Lv." + CountRecord.PlayerLevel[0].ToString("N0");
        m_resultText[9].text = "Lv." + CountRecord.PlayerLevel[1].ToString("N0");
        m_resultText[10].text = "Lv." + CountRecord.PlayerLevel[2].ToString("N0");
        m_resultText[11].text = "Title";
        m_resultText[12].text = "Upgrade";
        m_resultText[13].text = "Replay";
        // 前面に表示
        m_resultText[11].GetComponent<MeshRenderer>().sortingOrder = 2;
        m_resultText[12].GetComponent<MeshRenderer>().sortingOrder = 2;
        m_resultText[13].GetComponent<MeshRenderer>().sortingOrder = 2;
        // ボタンの位置を取得
        m_buttonPos[2] = m_button[2].transform.position;
        m_buttonPos[3] = m_button[3].transform.position;
        m_buttonPos[4] = m_button[4].transform.position;
        // カーソルのオブジェクトを中央に配置
        m_cursorPosX = 0;
        m_cursorPosY = 0;
    }

    // リザルト画面での操作
    private void ControlScreen400()
    {
        CursorControl();    // カーソルの操作

        if (pressDecide == true) // 決定ボタンが押されたとき
        {
            pressDecide = false;
            if (PressButton(m_buttonPos[2], 1, 0.5f) == true)
            {
                Debug.Log("タイトルへ");
            }
            if (PressButton(m_buttonPos[3], 1, 0.5f) == true)
            {
                Debug.Log("アップグレード画面へ");
                Screen500();    // アップグレード画面１へ遷移
                m_resultScreen.SetActive(false);
            }
            if (PressButton(m_buttonPos[4], 1, 0.5f) == true)
            {
                Debug.Log("リプレイへ");
            }
        }
    }

    // アップグレード画面１へ遷移
    private void Screen500()
    {
        CurrentScreen = 500; 
        m_displayBackground.GetComponent<SpriteRenderer>().sprite = m_background[(int)BACKGROUND.UPGRADE1];
        m_upgrade1Screen.SetActive(true);
        // ボタンの位置を取得
        m_buttonPos[0] = m_button[0].GetComponent<Transform>().position;
        // カーソルのオブジェクトを中央に配置
        m_cursorPosX = 0;
        m_cursorPosY = 0;
    }

    // アップグレード画面１での操作
    private void controlScreen500()
    {

        CursorControl();    // カーソルの操作

        if (pressDecide == true) // 決定ボタンが押されたとき
        {
            pressDecide = false;
            if (PressButton(m_buttonPos[0], 0.5f, 0.5f) == true)
            {
                Screen501();    // アップグレード画面２へ遷移
                m_upgrade1Screen.SetActive(false);
            }
        }
    }

    // アップグレード画面２へ遷移
    private void Screen501()
    {
        CurrentScreen = 501;
        m_displayBackground.GetComponent<SpriteRenderer>().sprite = m_background[(int)BACKGROUND.UPGRADE2];
        m_upgrade2Screen.SetActive(true);
        m_weightText[0].text = MaterialAmount[0].ToString("N0") + "g"; // 布の所持量を表示する
        m_weightText[1].text = MaterialAmount[1].ToString("N0") + "g"; // 金属の所持量を表示する
        m_weightText[2].text = MaterialAmount[2].ToString("N0") + "g"; // 木材の所持量を表示する
        m_weightText[0].GetComponent<MeshRenderer>().sortingOrder = 2; // 前面に表示
        m_weightText[1].GetComponent<MeshRenderer>().sortingOrder = 2;
        m_weightText[2].GetComponent<MeshRenderer>().sortingOrder = 2;

        for (int i = 0; i < m_materialVariety; i++)
        {
            m_displayStand[i].transform.position = new Vector3(-6 + 3 * i, -4.5f, 0);   // 台の位置
            m_displayWeightMeasure[i].transform.position = new Vector3(m_displayStand[i].transform.position.x, m_displayStand[i].transform.position.y + 1.5f, 0);   // 計りの位置
            m_weightText[i].transform.position = new Vector3(m_displayWeightMeasure[i].transform.position.x - 0.4f, m_displayWeightMeasure[i].transform.position.y - 0.25f, 0); // 文字の位置
            m_displayBag[i].transform.position = new Vector3(-6 + 3 * i, m_displayWeightMeasure[i].transform.position.y + 1.15f, 0); // 袋の位置
            if (MaterialAmount[i] > 100) // 100gを超えた場合
            {
                m_displayBag[i].GetComponent<SpriteRenderer>().sprite = m_bagSprite[1];
                m_displayBag[i].transform.position = new Vector3(-6 + 3 * i, m_displayWeightMeasure[i].transform.position.y + 1.95f, 0); // 袋の位置
            }
            if (MaterialAmount[i] > m_maxDisplayMaterialAmount) // 999,999gを超えた場合
            {
                m_weightText[i].text = m_maxDisplayMaterialAmount.ToString("N0") + "g"; // 最大表示量を表示する
                m_displayWeightMeasure[i].GetComponent<SpriteRenderer>().sprite = m_WeightMeasureSprite[1];
                m_displayWeightMeasure[i].transform.position = new Vector3(m_displayStand[i].transform.position.x, m_displayStand[i].transform.position.y + 1.35f, 0);   // 計りの位置
                m_weightText[i].transform.position = new Vector3(m_displayWeightMeasure[i].transform.position.x - 0.4f, m_displayWeightMeasure[i].transform.position.y - 0.3f, 0); // 文字の位置
                m_displayBag[i].transform.position = new Vector3(-6 + 3 * i, m_displayWeightMeasure[i].transform.position.y + 1.65f, 0);    // 袋の位置
            }
            Debug.Log($"position[{i}].x : {m_displayWeightMeasure[i].transform.position.x}");
        }
        // ボタンの位置を取得
        m_buttonPos[1] = m_button[1].GetComponent<Transform>().position;
        // カーソルのオブジェクトを中央に配置
        m_cursorPosX = 0;
        m_cursorPosY = 0;
    }

    // アップグレード画面２での操作
    private void controlScreen501()
    {
        CursorControl();    // カーソルの操作

        if (pressDecide == true) // 決定ボタンが押されたとき
        {
            pressDecide = false;
            if (PressButton(m_buttonPos[1], 0.5f, 0.5f) == true)
            {
                Screen500();    // アップグレード画面１へ遷移
                m_upgrade2Screen.SetActive(false);
            }
        }
    }

    // カーソルの操作
    private void CursorControl()
    {
        // 現在のキーボード情報
        var current = Keyboard.current;
        // キーボード接続チェック
        if (current == null)
        {
            // キーボードが未接続
            return;
        }
        var aKey = current.aKey;
        var upArrowKey = current.upArrowKey;
        var downArrowKey = current.downArrowKey;
        var leftArrowKey = current.leftArrowKey;
        var rightArrowKey = current.rightArrowKey;
        // キーが押されているかどうか
        if (upArrowKey.isPressed)
        {
            m_cursorPosY += 0.1f;
        }
        if (downArrowKey.isPressed)
        {
            m_cursorPosY -= 0.1f;
        }
        if (leftArrowKey.isPressed)
        {
            m_cursorPosX -= 0.1f;
        }
        if (rightArrowKey.isPressed)
        {
            m_cursorPosX += 0.1f;
        }
        // カーソルを画面内に移動する
        if (m_cursorPosX < -m_worldSize.x)
        {
            m_cursorPosX = -m_worldSize.x;
        }
        if (m_cursorPosX > m_worldSize.x)
        {
            m_cursorPosX = m_worldSize.x;
        }
        if (m_cursorPosY > m_worldSize.y)
        {
            m_cursorPosY = m_worldSize.y;
        }
        if (m_cursorPosY < -m_worldSize.y)
        {
            m_cursorPosY = -m_worldSize.y;
        }
        // カーソルを移動させる
        m_cursor.transform.position = new Vector2(m_cursorPosX, m_cursorPosY);
    }

    // 決定ボタンが押されたかの検知
    private int PressDecide()
    {
        // 現在のキーボード情報
        var current = Keyboard.current;
        // キーボード接続チェック
        if (current == null)
        {
            // キーボードが未接続
            return -1;
        }
        var enterKey = current.enterKey;
        if (enterKey.wasPressedThisFrame)
        {
            return 1;
        }
        else
        {
            return 0;
        }
    }

    // ボタンが押されたかの検知
    private bool PressButton(Vector2 pos, float rx, float ry)
    {
        Debug.Log("press");
        if (m_cursorPosX > pos.x - rx && m_cursorPosX < pos.x + rx &&
            m_cursorPosY > pos.y - ry && m_cursorPosY < pos.y + ry)
        {
            return true;
        }
        return false;
    }

    // ボタンを押したときのアニメーション
    private void ButtonAnimation()
    {

    }
}