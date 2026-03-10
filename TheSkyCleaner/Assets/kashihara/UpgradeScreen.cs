using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public enum UpgradeButtonType
{
    TransitionIngame,
    ArmPowerUp1,
    ArmPowerUp2,
    SpeedUp1,
    SpeedUp2,
    NetUp1,
    ButtonAmount
}

public class UpgradeScreen : MonoBehaviour
{
    [System.Serializable]
    struct ButtonElement
    {
        [SerializeField] private GameObject gameObject;
        [SerializeField] private GameObject icon;
        [SerializeField] private Sprite unlockSprite;       // スキルを取得した時のスプライト
        [SerializeField] private Sprite canUnlockSprite;    // スキルを取得可能の時のスプライト
        [SerializeField] private Color lockColor;           // スキルが取得不可能の時の色

        public readonly GameObject GameObject => gameObject;
        public readonly GameObject Icon => icon;
        public readonly Sprite UnlockSprite => unlockSprite;
        public readonly Sprite CanUnlockSprite => canUnlockSprite;
        public readonly Color LockColor => lockColor;
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
    [SerializeField] private Skillget m_skillget;
    [SerializeField] private ButtonConditionSO m_buttonCondition;

    private int m_conditionsForDecision;

    [SerializeField] private List<ButtonElement> m_buttonElements;
    [SerializeField] private CursorElement m_cursorElement;

    [SerializeField] private List<Vector2> m_buttonPositions;
    [SerializeField] private List<Vector2> m_buttonSizes;
    [SerializeField] private List<ButtonAnimation> m_buttonAnimations;
    public List<SkillSO> m_skills;
    [SerializeField] private List<Image> m_buttonImages;

    private int m_screenWidth;  // 取得した画面の横幅を格納する変数
    private int m_screenHeight; // 取得した画面の縦幅を格納する変数
    [SerializeField] private Vector2 m_cursorPos; // カーソルの座標
    private int m_pressButtonType;  // 押されたボタンの種類
    private int m_pressDecide;      // ボタンが押されたか離れたか
    private bool pressedButton;     // 以前にボタンが押されたか

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //m_screenWidth = Screen.width;   // 画面の横幅を取得する
        //m_screenHeight = Screen.height; // 画面の縦幅を取得する
        m_screenWidth = 800;
        m_screenHeight = 450;
        m_cursorPos = Vector2.zero;   // カーソルの座標
        m_conditionsForDecision = (int)m_buttonCondition.ConditionsForDecision;
        for (int i = 0; i < m_buttonElements.Count; i++)
        {
            m_buttonPositions.Add(m_buttonElements[i].GameObject.transform.localPosition);          // ボタンの位置を取得
            m_buttonSizes.Add(m_buttonElements[i].Icon.GetComponent<RectTransform>().rect.size);    // ボタンの大きさを取得
            m_buttonSizes[i] /= 2;                                                                  // ボタンの取得した大きさを半分にする
            m_buttonImages.Add(m_buttonElements[i].Icon.GetComponent<Image>());
            if (i > 0)
            {
                m_skills.Add(m_buttonElements[i].GameObject.GetComponent<SkillButton>().m_skill);
                //CheckUnlock(i, m_skills[i - 1]);
            }
            m_buttonAnimations[i].ButtonStateUpdate();
        }
    }

    // Update is called once per frame
    void Update()
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

            if (PressButton(m_buttonPositions[(int)UpgradeButtonType.TransitionIngame], m_buttonSizes[(int)UpgradeButtonType.TransitionIngame]) == true)
            {   
                // インゲーム
                pressedButton = true;
                m_pressButtonType = (int)UpgradeButtonType.TransitionIngame;
                m_buttonAnimations[(int)UpgradeButtonType.TransitionIngame].m_animationType = 1;
                m_buttonAnimations[(int)UpgradeButtonType.TransitionIngame].isAnimation = true;
            }
            if (PressButton(m_buttonPositions[(int)UpgradeButtonType.ArmPowerUp1], m_buttonSizes[(int)UpgradeButtonType.ArmPowerUp1]) == true)
            {   
                // アームパワーアップ１
                pressedButton = true;
                m_pressButtonType = (int)UpgradeButtonType.ArmPowerUp1;
                m_buttonAnimations[(int)UpgradeButtonType.ArmPowerUp1].m_animationType = 1;
                m_buttonAnimations[(int)UpgradeButtonType.ArmPowerUp1].isAnimation = true;
            }
            if (PressButton(m_buttonPositions[(int)UpgradeButtonType.ArmPowerUp2], m_buttonSizes[(int)UpgradeButtonType.ArmPowerUp2]) == true)
            {
                // アームパワーアップ２
                pressedButton = true;
                m_pressButtonType = (int)UpgradeButtonType.ArmPowerUp2;
                m_buttonAnimations[(int)UpgradeButtonType.ArmPowerUp2].m_animationType = 1;
                m_buttonAnimations[(int)UpgradeButtonType.ArmPowerUp2].isAnimation = true;
            }
            if (PressButton(m_buttonPositions[(int)UpgradeButtonType.SpeedUp1], m_buttonSizes[(int)UpgradeButtonType.SpeedUp1]) == true)
            {
                // スピードアップ１
                pressedButton = true;
                m_pressButtonType = (int)UpgradeButtonType.SpeedUp1;
                m_buttonAnimations[(int)UpgradeButtonType.SpeedUp1].m_animationType = 1;
                m_buttonAnimations[(int)UpgradeButtonType.SpeedUp1].isAnimation = true;
            }
            if (PressButton(m_buttonPositions[(int)UpgradeButtonType.SpeedUp2], m_buttonSizes[(int)UpgradeButtonType.SpeedUp2]) == true)
            {
                // スピードアップ２
                pressedButton = true;
                m_pressButtonType = (int)UpgradeButtonType.SpeedUp2;
                m_buttonAnimations[(int)UpgradeButtonType.SpeedUp2].m_animationType = 1;
                m_buttonAnimations[(int)UpgradeButtonType.SpeedUp2].isAnimation = true;
            }
            if (PressButton(m_buttonPositions[(int)UpgradeButtonType.NetUp1], m_buttonSizes[(int)UpgradeButtonType.NetUp1]) == true)
            {
                // ネットアップ１
                pressedButton = true;
                m_pressButtonType = (int)UpgradeButtonType.NetUp1;
                m_buttonAnimations[(int)UpgradeButtonType.NetUp1].m_animationType = 1;
                m_buttonAnimations[(int)UpgradeButtonType.NetUp1].isAnimation = true;
            }

            if (m_conditionsForDecision == 1)
            {
                pressedButton = false;
                //SceneLoader();
            }
        }
        else if (m_pressDecide == 2 && pressedButton == true
            &&m_conditionsForDecision == (int)ConditionsForDecision.PopToDecide)    // 決定ボタンが離された & 決定条件が"PopToDecide"
        {
            m_pressDecide = 0;

            m_buttonAnimations[m_pressButtonType].m_animationType = 2;
            m_buttonAnimations[m_pressButtonType].isAnimation = true;
            pressedButton = false;

            if (PressButton(m_buttonPositions[m_pressButtonType], m_buttonSizes[m_pressButtonType]) == true)
            {
                //SceneLoader();
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

    //public void CheckUnlock(int buttonNum, SkillSO skillData)
    //{
    //    Debug.Log($"buttonNum: {buttonNum}");
    //    if (m_skillget.unlockSkills.Contains(skillData))
    //    {
    //        Debug.Log("取得済み");
    //        m_buttonImages[buttonNum].sprite = m_buttonElements[buttonNum].UnlockSprite;
    //        return;
    //    }

    //    foreach (var need in skillData.NeedSkill)//必要なスキルを取得済みかどうか
    //    {
    //        if (!m_skillget.unlockSkills.Contains(need))
    //        {
    //            Debug.Log("未解放");
    //            m_buttonImages[buttonNum].color = m_buttonElements[buttonNum].LockColor;
    //            return;
    //        }
    //    }

    //    if (!m_skillget.HasMaterials(skillData))
    //    {
    //        Debug.Log("ポイントが不足");
    //        m_buttonImages[buttonNum].color = m_buttonElements[buttonNum].LockColor;
    //        return;
    //    }

    //    Debug.Log("取得可能");
    //    m_buttonImages[buttonNum].sprite = m_buttonElements[buttonNum].CanUnlockSprite;
    //}

    // シーンをロード
    //private void SceneLoader()
    //{
    //    switch (m_pressButtonType)
    //    {
    //        case (int)UpgradeButtonType.TransitionIngame:
    //            Debug.Log("インゲームへ");
    //            // ロードシーン（ingame）
    //            break;

    //        case (int)UpgradeButtonType.ArmPowerUp1:
    //            Debug.Log("アームパワーアップ１");
    //            m_skillButton.OnClick();
    //            break;

    //        case (int)UpgradeButtonType.ArmPowerUp2:
    //            Debug.Log("アームパワーアップ２");
    //            m_skillButton.OnClick();
    //            break;

    //        case (int)UpgradeButtonType.SpeedUp1:
    //            Debug.Log("スピードアップ１");
    //            m_skillButton.OnClick();
    //            break;

    //        case (int)UpgradeButtonType.SpeedUp2:
    //            Debug.Log("スピードアップ2");
    //            m_skillButton.OnClick();
    //            break;

    //        case (int)UpgradeButtonType.NetUp1:
    //            Debug.Log("ネットアップ１");
    //            m_skillButton.OnClick();
    //            break;
    //    }
    //}
}
