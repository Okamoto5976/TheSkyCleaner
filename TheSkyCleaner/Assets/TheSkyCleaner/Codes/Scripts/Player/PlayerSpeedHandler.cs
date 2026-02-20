using UnityEngine;

public class PlayerSpeedHandler : MonoBehaviour
{
    [Header("Input")]
    [SerializeField] private InputContainer m_inputContainer;

    [SerializeField] private PlayerStatus m_playerStatus;

    [SerializeField] private FloatContainer m_playerSpeedLerp;
    [SerializeField] private float m_lerpTime;

    private float m_value;
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
        m_playerSpeedLerp.SetValue(m_value);
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