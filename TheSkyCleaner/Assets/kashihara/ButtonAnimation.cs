using System.Collections;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class ButtonAnimation : MonoBehaviour
{
    [System.Serializable]
    struct AnimationTimer
    {
        [SerializeField] private int anim_push;
        [SerializeField] private int anim_pop;
        [SerializeField] private int anim_finish;

        public readonly int Anim_push => anim_push;
        public readonly int Anim_pop => anim_pop;

        public readonly int Anim_finish => anim_finish;
    }

    [System.Serializable]
    struct ChangeSize
    {
        [SerializeField] private float size_push;
        [SerializeField] private float size_pop;

        public readonly float Size_push => size_push;

        public float Size_pop => size_pop;
    }


    public bool canPress = true;    // 決定ボタンに反応するか
    public bool isAnimation;        // アニメーションが再生中かどうか
    public int m_animationType;     // アニメーションの種類

    [SerializeField] private ButtonConditionSO m_buttonCondition;
    [SerializeField] private AnimationTimer m_animationTimer;
    [SerializeField] private ChangeSize m_changeSize;
    [SerializeField] private Image m_icon;
    private Color m_changeColor;

    private int m_animationTime;
    private Transform m_transform;
    private float m_resizePerCall;      // 呼び出し毎のサイズ変更量
    private int m_conditionsForDecision; // 決定条件

    private Vector3 m_buttonSize;   // 元のサイズ
    private Color m_buttonColor;    // 元の色

    private void Start()
    {
        m_animationTime = 0;
        m_transform = gameObject.GetComponent<Transform>();
        m_buttonSize = m_transform.localScale;
        m_buttonColor = m_icon.color;
        m_conditionsForDecision = (int)m_buttonCondition.ConditionsForDecision;
    }

    private void Update()
    {
       
    }

    private void FixedUpdate()
    {
        if (isAnimation == true)
        {
            if (m_animationType == 1)
            {
                isAnimation = false;
                if (m_conditionsForDecision == 1)    // 決定条件が"PushToDecide"の場合
                {
                    canPress = false;
                }
                StartCoroutine("Animation_Push");
            }
            else if (m_animationType == 2)
            {
                isAnimation = false;
                canPress = false;
                StartCoroutine("Animation_Pop");
            }
        }
    }

    private IEnumerator Animation_Push()
    {
        //Debug.Log("Anim_Push");
        m_resizePerCall = (m_changeSize.Size_push - m_buttonSize.x) / m_animationTimer.Anim_push;
        m_animationTime = m_animationTimer.Anim_push;
        m_icon.color = m_changeColor;  // 色を変更
        for (int i = 1; i <= m_animationTime; i++)
        {
            m_transform.localScale = m_buttonSize * (1.0f + m_resizePerCall * i);
            yield return null;
        }
        if (m_conditionsForDecision != 2)    // 決定条件が"PopToDecide"でない場合
        {
            StartCoroutine("Animation_Pop");
        }
    }

    private IEnumerator Animation_Pop()
    {
        //Debug.Log("Anim_Pop");
        m_resizePerCall = (m_changeSize.Size_push - m_changeSize.Size_pop) / m_animationTimer.Anim_pop;
        m_animationTime = m_animationTimer.Anim_pop;
        m_icon.color = m_buttonColor;  // 色を戻す
        for (int i = 1; i <= m_animationTime; i++)
        {
            m_transform.localScale = m_buttonSize * (m_changeSize.Size_push - m_resizePerCall * i);
            yield return null;
        }
        StartCoroutine("Animation_Finish");
    }

    private IEnumerator Animation_Finish()  // ボタンを元の状態に戻すアニメーション
    {
        //Debug.Log("Anim_Finish");
        m_resizePerCall = (m_buttonSize.x - m_changeSize.Size_pop) / m_animationTimer.Anim_finish;
        m_animationTime = m_animationTimer.Anim_finish;
        for (int i = 1; i <= m_animationTime; i++)
        {
            m_transform.localScale = m_buttonSize * (m_changeSize.Size_pop + m_resizePerCall * i);
            yield return null;
        }
        m_transform.localScale = m_buttonSize;  // 誤差が絶対に出ないように大きさを戻す
        canPress = true;
    }

    // ボタンの状態を更新
    public void ButtonStateUpdate()
    {
        m_buttonColor = m_icon.color;
        m_changeColor = m_buttonColor - new Color32(20, 20, 20, 0);
    }
}
