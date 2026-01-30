using UnityEngine;

//•ûŒü‚ð—^‚¦‚é
[CreateAssetMenu(fileName = "ES_MoveStraight", menuName = "Enemy/States/Move Straight")]
public class ES_MoveStraight : EnemyState
{
    [SerializeField] private Vector3 moveDirection = Vector3.forward;
    [Tooltip("ˆÚ“®ŽžŠÔ")]

    EnemySequence.StateMachineState es = new EnemySequence.StateMachineState();
    private float _timer;

    public override void OnEnter(EnemyStateMachine est)
    {
        _timer = 0f;
    }

    public override StateStatus OnUpdate(EnemyStateMachine est, float deltaTime)
    {
        if (es.m_time <= 0f)
        {
           // Move(est, moveDirection.normalized);
            return StateStatus.Success;
        }

        _timer += deltaTime;

        // is•ûŒü‚ð—^‚¦‚éiMoveHandler ˜AŒg or Transform ˆÚ“®j
        //Move(est, moveDirection.normalized);

        return _timer >= es.m_time ? StateStatus.Success : StateStatus.Running;
    }
}
