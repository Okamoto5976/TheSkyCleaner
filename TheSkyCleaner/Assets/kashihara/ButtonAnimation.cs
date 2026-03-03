using System.Collections;
using System.Threading;
using UnityEngine;

public class ButtonAnimation : MonoBehaviour
{
    [System.Serializable]
    struct AnimationTimer
    {
        [SerializeField] private int anim_push;
        [SerializeField] private int anim_pop;

        public readonly int Anim_push => anim_push;
        public readonly int Anim_pop => anim_pop;
    }

    public bool isAnimation;
    public int m_animationType;
    [SerializeField] private AnimationTimer m_animationTimer;
    [SerializeField] private GameObject m_icon;
    private int m_animationTime;
    private Transform m_transform;

    private Vector2 m_buttonSize;
    private Color m_buttonColor;

    private Vector2 m_saveSize;
    private Color m_saveColor;

    private void Start()
    {
        m_animationTime = 0;
        m_transform = gameObject.GetComponent<Transform>();
        m_buttonSize = m_transform.localScale;
        //m_buttonColor = m_icon.GetComponent<Image>().color;
        m_saveSize = m_buttonSize;
        //m_saveColor = m_buttonColor;
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
                StartCoroutine("Animation_Push");
            }
        }
    }

    private IEnumerator Animation_Push()
    {
        for (int i = 0; i < m_animationTime; i++)
        {
            m_buttonSize = m_saveSize * (0.1f * i);
            yield return null;
        }
        isAnimation = false;
    }
}
