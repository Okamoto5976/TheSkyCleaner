using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public enum ResultButtonType
{
    TransitionTitle,
    TransitionIngame,
}

public class ResultScreen : MonoBehaviour
{
    [System.Serializable]
    struct ButtonElement
    {
        [SerializeField] private GameObject gameObject;
        [SerializeField] private GameObject icon;

        public readonly GameObject GameObject => gameObject;
        public readonly GameObject Icon => icon;
    };

    [System.Serializable]
    struct CursorElement
    {
        [SerializeField] private GameObject gameObject;
        [SerializeField] private float moveSpeed;

        public readonly GameObject GameObject => gameObject;
        public readonly float MoveSpeed => moveSpeed;
    }

    [System.Serializable]
    struct TextElement
    {
        [SerializeField] private TextMeshProUGUI resultString;
        [SerializeField] private string text;

        public readonly TextMeshProUGUI ResultString => resultString;
        public readonly string Text => text;
    }

    [System.Serializable]
    struct AircraftElement
    {
        [SerializeField] private GameObject gameObject;
        [SerializeField] private bool isRotate;
        [SerializeField] private float rotateSpeed;

        public readonly GameObject GameObject => gameObject;
        public readonly bool IsRotate => isRotate;
        public readonly float RotateSpeed => rotateSpeed;
    }

    [SerializeField] private ButtonConditionSO m_buttonCondition;

    private int m_conditionsForDecision;

    [SerializeField] private List<ButtonElement> m_buttonElements;
    [SerializeField] private CursorElement m_cursorElement;
    [SerializeField] private List<TextElement> m_textElements;
    [SerializeField] private AircraftElement m_aircraftElement;

    private int timer;  // 仮
    private int score;  // 仮
    public int[] PartLevel = new int[3];    // 仮

    [SerializeField] private List<Vector2> m_buttonPositions;
    [SerializeField] private List<Vector3> m_buttonSizes;
    [SerializeField] private List<ButtonAnimation> m_buttonAnimations;

    [SerializeField] private Vector3 m_axis = new Vector3(0.0f, 1.0f, 0.0f);
    private Transform m_aircraftTransform;

    private int m_screenWidth;  // 取得した画面の横幅を格納する変数
    private int m_screenHeight; // 取得した画面の縦幅を格納する変数
    [SerializeField] private Vector2 m_cursorPos; // カーソルの座標
    [SerializeField] private List<TextMeshProUGUI> m_resultCountText;
    private int m_pressButtonType;  // 押されたボタンの種類
    private int m_pressDecide;      // ボタンが押されたか離れたか
    private bool pressedButton;     // 以前にボタンが押されたか




    //InputSystemでマウスをカーソルが追従
    //コントローラーでもカーソルがそう動くようにする
    //決定キーでボタンを押せるようにする（unityでそうなっているなら必要ない)
    //コメントアウトしておくだけにすること


    private void Awake()
    {
        //m_aircraftTransform = m_aircraftElement.GameObject.transform;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        //timer = 99;
        //score = 8765432;
        //PartLevel[0] = 111;
        //PartLevel[1] = 234;
        //PartLevel[2] = 567;

        //m_screenWidth = Screen.width;   // 画面の横幅を取得する
        //m_screenHeight = Screen.height; // 画面の縦幅を取得する
        m_screenWidth = 845;    // UIを他のシーンより手前に表示しているから少し大きい
        m_screenHeight = 475;
        m_cursorPos = Vector2.zero;     // カーソルの座標
        m_conditionsForDecision = (int)m_buttonCondition.ConditionsForDecision;

        for (int i = 0; i < m_textElements.Count; i++)
        {
            m_textElements[i].ResultString.text = m_textElements[i].Text;
        }

        m_resultCountText[0].text = score.ToString("N0") + " pt.";
        m_resultCountText[1].text = timer.ToString("N0");
        m_resultCountText[2].text = "Lv." + PartLevel[0].ToString("N0");
        m_resultCountText[3].text = "Lv." + PartLevel[1].ToString("N0");
        m_resultCountText[4].text = "Lv." + PartLevel[2].ToString("N0");

        for (int i = 0; i < m_buttonElements.Count; i++)
        {
            m_buttonPositions.Add(m_buttonElements[i].GameObject.transform.localPosition);          // ボタンの位置を取得
            m_buttonSizes.Add(m_buttonElements[i].Icon.GetComponent<RectTransform>().rect.size);    // ボタンの大きさを取得
            m_buttonSizes[i] /= 2;                                                                  // ボタンの取得した大きさを半分にする
            m_buttonAnimations.Add(m_buttonElements[i].GameObject.GetComponent<ButtonAnimation>());
        }
    }

    // Update is called once per frame
    private void Update()
    {
        if (PressDecide() == 1) // 決定ボタンが押されたとき
        {
            m_pressDecide = 1;
        }
        else if (PressDecide() == 2)    // 決定ボタンが離されたとき
        {
            m_pressDecide = 2;
        }
    }

    private void FixedUpdate()
    {
        CursorControl();    // カーソルの操作

        if (m_pressDecide == 1) // 決定ボタンが押されたとき
        {
            m_pressDecide = 0;

            if (PressButton(m_buttonPositions[(int)ResultButtonType.TransitionTitle], m_buttonSizes[(int)ResultButtonType.TransitionTitle]) == true)
            {
                // タイトル画面
                pressedButton = true;
                m_pressButtonType = (int)ResultButtonType.TransitionTitle;
                m_buttonAnimations[(int)ResultButtonType.TransitionTitle].m_animationType = 1;
                m_buttonAnimations[(int)ResultButtonType.TransitionTitle].isAnimation = true;
            }
            if (PressButton(m_buttonPositions[(int)ResultButtonType.TransitionIngame], m_buttonSizes[(int)ResultButtonType.TransitionIngame]) == true)
            {
                // インゲーム
                pressedButton = true;
                m_pressButtonType = (int)ResultButtonType.TransitionIngame;
                m_buttonAnimations[(int)ResultButtonType.TransitionIngame].m_animationType = 1;
                m_buttonAnimations[(int)ResultButtonType.TransitionIngame].isAnimation = true;
            }

            if (m_conditionsForDecision == 1)
            {
                pressedButton = false;
                SceneLoader();
            }
        }
        else if (m_pressDecide == 2 && pressedButton == true
            && m_conditionsForDecision == (int)ConditionsForDecision.PopToDecide)    // 決定ボタンが離された & 決定条件が"PopToDecide"
        {
            m_pressDecide = 0;

            m_buttonAnimations[m_pressButtonType].m_animationType = 2;
            m_buttonAnimations[m_pressButtonType].isAnimation = true;
            pressedButton = false;

            if (PressButton(m_buttonPositions[m_pressButtonType], m_buttonSizes[m_pressButtonType]) == true)
            {
                SceneLoader();
            }
        }

        if (m_aircraftElement.IsRotate == true)
        {
            //AircraftRotator();
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
            m_cursorPos.y += m_cursorElement.MoveSpeed;
        }
        if (downArrowKey.isPressed)
        {
            m_cursorPos.y -= m_cursorElement.MoveSpeed;
        }
        if (leftArrowKey.isPressed)
        {
            m_cursorPos.x -= m_cursorElement.MoveSpeed;
        }
        if (rightArrowKey.isPressed)
        {
            m_cursorPos.x += m_cursorElement.MoveSpeed;
        }
        // カーソルを画面内に移動する
        if (m_cursorPos.x < -m_screenWidth / 2)
        {
            m_cursorPos.x = -m_screenWidth / 2;
        }
        if (m_cursorPos.x > m_screenWidth / 2)
        {
            m_cursorPos.x = m_screenWidth / 2;
        }
        if (m_cursorPos.y > m_screenHeight / 2)
        {
            m_cursorPos.y = m_screenHeight / 2;
        }
        if (m_cursorPos.y < -m_screenHeight / 2)
        {
            m_cursorPos.y = -m_screenHeight / 2;
        }
        // カーソルを移動させる
        m_cursorElement.GameObject.transform.localPosition = m_cursorPos;
    }

    // 決定ボタンが押されたかの検知
    // & 離されたかの検知
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
        else if (enterKey.wasReleasedThisFrame)
        {
            return 2;
        }
        else
        {
            return 0;
        }
    }

    // ボタンが押されたかの検知
    private bool PressButton(Vector2 pos, Vector2 hSize)
    {
        if (m_cursorPos.x > pos.x - hSize.x && m_cursorPos.x < pos.x + hSize.x &&
            m_cursorPos.y > pos.y - hSize.y && m_cursorPos.y < pos.y + hSize.y)
        {
            return true;
        }
        return false;
    }

    // 機体の回転処理
    private void AircraftRotator()
    {
        // １フレームで回転する角度を角速度から計算
        var angle = m_aircraftElement.RotateSpeed * Time.deltaTime;

        // 既存のrotationに軸回転のクォータニオンを掛ける
        m_aircraftTransform.rotation = Quaternion.AngleAxis(angle, m_axis) * m_aircraftTransform.rotation;
    }

    // シーンをロード
    private void SceneLoader()
    {
        switch (m_pressButtonType)
        {
            case (int)ResultButtonType.TransitionTitle:
                Debug.Log("タイトル画面へ");
                // ロードシーン（title）
                break;

            case (int)ResultButtonType.TransitionIngame:
                Debug.Log("インゲームへ");
                // ロードシーン（ingame）
                break;
        }
    }
}
