using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(MovementHandler))]
public class PlayerController : MonoBehaviour
{
    [Header("Logger")]
    [SerializeField] private Logger m_logger;

    [Header("Events")]
    [SerializeField] private UnityEvent<Vector2> m_onMoveAll;
    [SerializeField] private UnityEvent<float> m_onMoveHorizontal;
    [SerializeField] private UnityEvent<float> m_onMoveVertical;
    [SerializeField] private UnityEvent m_onMainActionTap;
    [SerializeField] private UnityEvent m_onMainActionHoldStarted;
    [SerializeField] private UnityEvent m_onMainActionHoldCancelled;
    [SerializeField] private UnityEvent<float> m_onChangeSpeed;

    private MovementHandler m_movementHandler;

    private void Awake()
    {
        m_movementHandler = GetComponent<MovementHandler>();
    }

    public void MoveHorizontal(float dir)
    {
        m_onMoveHorizontal.Invoke(dir);
        m_movementHandler.MoveHorizontal(dir);
    }

    public void MoveVertical(float dir)
    {
        m_onMoveVertical.Invoke(dir);
        m_movementHandler.MoveVertical(dir);
    }

    public void MoveAll(Vector2 dir)
    {
        m_onMoveAll.Invoke(dir);
        m_onMoveHorizontal.Invoke(dir.x);
        m_onMoveVertical.Invoke(dir.y);
        m_movementHandler.MoveOnZ(dir);
    }

    public void ChangeSpeed(float dir)
    {
        m_onChangeSpeed.Invoke(dir);
    }

    public void MainActionTap()
    {
        m_onMainActionTap.Invoke();
    }

    public void MainActionHoldSetState(bool state)
    {
        if (state)
        {
            m_logger.Log("Hold Started", this);
            m_onMainActionHoldStarted.Invoke();
        }
        else
        {
            m_logger.Log("Hold Cancelled", this);
            m_onMainActionHoldCancelled.Invoke();
        }
    }
}
