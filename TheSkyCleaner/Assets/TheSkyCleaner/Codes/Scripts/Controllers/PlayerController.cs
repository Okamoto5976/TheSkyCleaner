using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(MovementHandler))]
public class PlayerController : MonoBehaviour
{
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
            Debug.Log("Hold Started");
            m_onMainActionHoldStarted.Invoke();
        }
        else
        {
            Debug.Log("Hold Cancelled");
            m_onMainActionHoldCancelled.Invoke();
        }
    }
}
