using UnityEngine;

public class PlayerSpeedHandler : MonoBehaviour
{
    [Header("Input")]
    [SerializeField] private InputContainer m_inputContainer;

    [SerializeField] private PlayerStatus m_playerStatus;
    [SerializeField] private TransformValue m_environmentTransformValue;

    private float m_value;
    [SerializeField] private float m_lerpTime;

    [SerializeField] private Vector3 m_transformingValues;
    [SerializeField] private MovementHandler m_playerMoveHandler;


    private int m_strongHoldValue;
    private int m_weakHoldValue;

    private void Awake()
    {
        m_value = 0;
        m_strongHoldValue = 0;
        m_weakHoldValue = 0;
    }

    private void OnEnable()
    {
        m_inputContainer.StrongAction.HoldState.OnValueChanged += OnStrongHold;
        m_inputContainer.WeakAction.HoldState.OnValueChanged += OnWeakHold;
    }

    private void OnDisable()
    {
        m_inputContainer.StrongAction.HoldState.OnValueChanged -= OnStrongHold;
        m_inputContainer.WeakAction.HoldState.OnValueChanged -= OnWeakHold;
    }

    private void Update()
    {
        ChangeSpeed(m_strongHoldValue + m_weakHoldValue);
    }

    public void LerpSpeedDir(int dir)
    {
        m_value = Mathf.Lerp(m_value, dir, Time.deltaTime * m_lerpTime);

        Transform(m_value);
        m_environmentTransformValue.Transform(m_value);
    }

    private void Transform(float val)
    {
        float newVal;
        if (val > 0)
        {
            newVal = (float)(val * (m_transformingValues.z - m_transformingValues.y) + m_transformingValues.y);
        }
        else if (val < 0)
        {
            newVal = (float)(val * (m_transformingValues.y - m_transformingValues.x) + m_transformingValues.y);
        }
        else
        {
            newVal = m_transformingValues.y;
        }
        m_playerStatus.UpdateSpeed(newVal);
        m_playerMoveHandler.SetSpeed(newVal);
    }

    public void OnStrongHold(bool state)
    {
        m_strongHoldValue = state ? 1 : 0;
    }
    public void OnWeakHold(bool state)
    {
        m_weakHoldValue = state ? -1 : 0;
    }
    public void ChangeSpeed(int dir)
    {
        LerpSpeedDir(dir);
        m_playerStatus.UpdateSpeedAxis(dir);
    }

}