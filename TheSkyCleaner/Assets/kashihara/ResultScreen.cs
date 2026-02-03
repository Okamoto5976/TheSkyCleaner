using System.Security;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class ResultScreen : MonoBehaviour
{
    private int timer;  // 仮
    private int score;  // 仮
    public int[] PartLevel = new int[3];    // 仮

    [SerializeField] private GameObject[] m_button; // 画面遷移用ボタン
    [SerializeField] private GameObject m_cursor;   // カーソルのオブジェクト
    [SerializeField] private TextMeshProUGUI[] m_resultText;        // ゲームの結果の文字
    [SerializeField] private TextMeshProUGUI[] m_resultCountText;   // ゲームの結果の数字
    [SerializeField] private TextMeshProUGUI[] m_buttonText;        // ボタンに表示する文字
    private Vector2[] m_buttonPos = new Vector2[2];
    private int m_screenWidth;  // 取得した画面の横幅を格納する変数
    private int m_screenHeight; // 取得した画面の縦幅を格納する変数
    private float m_cursorPosX; // カーソルのx座標
    private float m_cursorPosY; // カーソルのy座標
    private bool pressDecide;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        timer = 99;
        score = 8765432;
        PartLevel[0] = 111;
        PartLevel[1] = 234;
        PartLevel[2] = 567;

        m_screenWidth = Screen.width;   // 画面の横幅を取得する
        m_screenHeight = Screen.height; // 画面の縦幅を取得する
        m_cursorPosX = m_screenWidth / 2;   // カーソルのx座標
        m_cursorPosY = m_screenHeight / 2;  // カーソルのy座標
        m_resultText[0].text = "Score";
        m_resultText[1].text = "time";
        m_resultText[2].text = "Fuel";
        m_resultText[3].text = "Armor";
        m_resultText[4].text = "EneBul";

        m_resultCountText[0].text = score.ToString("N0") + " pt.";
        m_resultCountText[1].text = timer.ToString("N0");
        m_resultCountText[2].text = "Lv." + PartLevel[0].ToString("N0");
        m_resultCountText[3].text = "Lv." + PartLevel[1].ToString("N0");
        m_resultCountText[4].text = "Lv." + PartLevel[2].ToString("N0");

        m_buttonText[0].text = "InGame";
        m_buttonText[1].text = "Upgrade";
        m_buttonPos[0] = m_button[0].transform.position;    // ボタンの位置を取得
        m_buttonPos[1] = m_button[1].transform.position;    // ボタンの位置を取得
    }

    // Update is called once per frame
    void Update()
    {
        if (PressDecide() == 1) // 決定ボタンが押されたとき
        {
            pressDecide = true;
        }
    }

    private void FixedUpdate()
    {
        CursorControl();    // カーソルの操作

        if (pressDecide == true) // 決定ボタンが押されたとき
        {
            pressDecide = false;

            if (PressButton(m_buttonPos[0], 75, 40) == true)
            {
                Debug.Log("インゲームへ");
                // ロードシーン（ingame）
            }
            if (PressButton(m_buttonPos[1], 75, 40) == true)
            {
                Debug.Log("アップグレード画面へ");
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
            m_cursorPosY += 4f;
        }
        if (downArrowKey.isPressed)
        {
            m_cursorPosY -= 4f;
        }
        if (leftArrowKey.isPressed)
        {
            m_cursorPosX -= 4f;
        }
        if (rightArrowKey.isPressed)
        {
            m_cursorPosX += 4f;
        }
        // カーソルを画面内に移動する
        if (m_cursorPosX < 0)
        {
            m_cursorPosX = 0;
        }
        if (m_cursorPosX > m_screenWidth)
        {
            m_cursorPosX = m_screenWidth;
        }
        if (m_cursorPosY > m_screenHeight)
        {
            m_cursorPosY = m_screenHeight;
        }
        if (m_cursorPosY < 0)
        {
            m_cursorPosY = 0;
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
}
