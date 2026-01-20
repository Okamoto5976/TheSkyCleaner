using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(MovementHandler))]
public class PlayerController : MonoBehaviour
{
    [System.Serializable]
    struct ButtonEvent
    {
        [SerializeField] UnityEvent m_tap;
        [SerializeField] UnityEvent m_holdStarted;
        [SerializeField] UnityEvent m_holdCancelled;
        public readonly UnityEvent Tap => m_tap;
        public readonly UnityEvent HoldStarted => m_holdStarted;
        public readonly UnityEvent HoldCancelled => m_holdCancelled;
    }


    [Header("Logger")]
    [SerializeField] private Logger m_logger;

    [Header("Events")]
    [SerializeField] private UnityEvent<Vector2> m_onMoveAll;
    [SerializeField] private UnityEvent<float> m_onMoveHorizontal;
    [SerializeField] private UnityEvent<float> m_onMoveVertical;
    [SerializeField] private ButtonEvent m_onMainAction;
    [SerializeField] private UnityEvent<float> m_onChangeSpeed;
    [SerializeField] private UnityEvent<bool> m_onDodge;

    private MovementHandler m_movementHandler;
    private float m_strongHoldValue;
    private float m_weakHoldValue;
    private Vector2 m_movementAxis;

    private void Awake()
    {
        m_movementHandler = GetComponent<MovementHandler>();
        m_strongHoldValue = 0;
        m_weakHoldValue = 0;
        m_movementAxis = Vector2.zero;
    }

    private void Update()
    {
        ChangeSpeed(m_strongHoldValue + m_weakHoldValue);
    }

    public void MoveHorizontal(float dir)
    {
        m_onMoveHorizontal.Invoke(dir);
        m_movementHandler.MoveHorizontal(dir);
        m_movementAxis.x = dir;
    }

    public void MoveVertical(float dir)
    {
        m_onMoveVertical.Invoke(dir);
        m_movementHandler.MoveVertical(dir);
        m_movementAxis.y = dir;
    }

    public void MoveAll(Vector2 dir)
    {
        m_movementAxis = dir;
        m_onMoveAll.Invoke(dir);
        m_onMoveHorizontal.Invoke(dir.x);
        m_onMoveVertical.Invoke(dir.y);
        m_movementHandler.MoveOnZ(dir);
    }

    public void OnStrongHold(bool state)
    {
        m_strongHoldValue = state ? 1 : 0;
    }
    public void OnStrongAction(bool state)
    {
        m_onDodge.Invoke(state);
    }
    public void OnWeakHold(bool state)
    {
        m_weakHoldValue = state ? -1 : 0;
    }
    public void ChangeSpeed(float dir)
    {
        m_onChangeSpeed.Invoke(dir);
    }

    public void MainActionTap()
    {
        m_onMainAction.Tap.Invoke();
    }

    public void MainActionHoldSetState(bool state)
    {
        if (state)
        {
            m_logger.Log("Hold Started", this);
            m_onMainAction.HoldStarted.Invoke();
        }
        else
        {
            m_logger.Log("Hold Cancelled", this);
            m_onMainAction.HoldCancelled.Invoke();
        }
    }
}
