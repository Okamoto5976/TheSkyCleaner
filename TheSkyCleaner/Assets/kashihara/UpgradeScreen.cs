using NUnit.Framework;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public enum ButtonType
{
    TransitionIngame,
    ArmPowerUp1,
    ArmPowerUp2,
    SpeedUp1,
    SpeedUp2,
    NetUp1
}

public class UpgradeScreen : MonoBehaviour
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

    [SerializeField] private SkillButton m_skillButton;

    [SerializeField] private List<ButtonElement> m_buttonElements;
    [SerializeField] private CursorElement m_cursorElement;

    [SerializeField] private List<Vector2> m_buttonPositions;
    [SerializeField] private List<Vector2> m_buttonSizes;
    private List<ButtonAnimation> m_buttonAnimations;

    private int m_screenWidth;  // 取得した画面の横幅を格納する変数
    private int m_screenHeight; // 取得した画面の縦幅を格納する変数
    [SerializeField] private Vector2 m_cursorPos; // カーソルの座標
    private bool pressDecide;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //m_screenWidth = Screen.width;   // 画面の横幅を取得する
        //m_screenHeight = Screen.height; // 画面の縦幅を取得する
        m_screenWidth = 800;
        m_screenHeight = 450;
        m_cursorPos = Vector2.zero;   // カーソルの座標

        for (int i = 0; i < m_buttonElements.Count; i++)
        {
            Debug.Log(i);
            Debug.Log($"buttonElements{i}.GO : {m_buttonElements[i].GameObject}");
            m_buttonPositions.Add(m_buttonElements[i].GameObject.transform.localPosition);          // ボタンの位置を取得
            m_buttonSizes.Add(m_buttonElements[i].Icon.GetComponent<RectTransform>().rect.size);    // ボタンの大きさを取得
            m_buttonSizes[i] /= 2;                                                                  // ボタンの取得した大きさを半分にする
            m_buttonAnimations.Add(m_buttonElements[i].GameObject.GetComponent<ButtonAnimation>());
        }
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

            if (PressButton(m_buttonPositions[(int)ButtonType.TransitionIngame], m_buttonSizes[(int)ButtonType.TransitionIngame]) == true)
            {
                Debug.Log("インゲームへ");
                m_buttonAnimations[(int)ButtonType.TransitionIngame].m_animationType = 1;
                m_buttonAnimations[(int)ButtonType.TransitionIngame].isAnimation = true;
                // ロードシーン（ingame）
            }
            if (PressButton(m_buttonPositions[(int)ButtonType.ArmPowerUp1], m_buttonSizes[(int)ButtonType.ArmPowerUp1]) == true)
            {
                Debug.Log("アームパワーアップ1");
                m_buttonAnimations[(int)ButtonType.ArmPowerUp1].m_animationType = 1;
                m_buttonAnimations[(int)ButtonType.ArmPowerUp1].isAnimation = true;
                m_skillButton.OnClick();
            }
            if (PressButton(m_buttonPositions[(int)ButtonType.ArmPowerUp2], m_buttonSizes[(int)ButtonType.ArmPowerUp2]) == true)
            {
                Debug.Log("アームパワーアップ2");
                m_buttonAnimations[(int)ButtonType.ArmPowerUp2].m_animationType = 1;
                m_buttonAnimations[(int)ButtonType.ArmPowerUp2].isAnimation = true;
                m_skillButton.OnClick();
            }
            if (PressButton(m_buttonPositions[(int)ButtonType.SpeedUp1], m_buttonSizes[(int)ButtonType.SpeedUp1]) == true)
            {
                Debug.Log("スピードアップ1");
                m_buttonAnimations[(int)ButtonType.SpeedUp1].m_animationType = 1;
                m_buttonAnimations[(int)ButtonType.SpeedUp1].isAnimation = true;
                m_skillButton.OnClick();
            }
            if (PressButton(m_buttonPositions[(int)ButtonType.SpeedUp2], m_buttonSizes[(int)ButtonType.SpeedUp2]) == true)
            {
                Debug.Log("スピードアップ2");
                m_buttonAnimations[(int)ButtonType.SpeedUp2].m_animationType = 1;
                m_buttonAnimations[(int)ButtonType.SpeedUp2].isAnimation = true;
                m_skillButton.OnClick();
            }
            if (PressButton(m_buttonPositions[(int)ButtonType.NetUp1], m_buttonSizes[(int)ButtonType.NetUp1]) == true)
            {
                Debug.Log("ネットアップ1");
                m_buttonAnimations[(int)ButtonType.NetUp1].m_animationType = 1;
                m_buttonAnimations[(int)ButtonType.NetUp1].isAnimation = true;
                m_skillButton.OnClick();
            }
            Debug.Log("check");
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
    private bool PressButton(Vector2 pos, Vector2 rSize)
    {
        if (m_cursorPos.x > pos.x - rSize.x && m_cursorPos.x < pos.x + rSize.x &&
            m_cursorPos.y > pos.y - rSize.y && m_cursorPos.y < pos.y + rSize.y)
        {
            return true;
        }
        return false;
    }
}
