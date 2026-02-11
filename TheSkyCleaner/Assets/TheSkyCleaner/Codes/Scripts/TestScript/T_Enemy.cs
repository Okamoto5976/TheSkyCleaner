using UnityEngine;

public class T_Enemy : MonoBehaviour, ILockOnTarget
{
    [SerializeField] private EnemyStateMachine m_enemyStateMachine;

    public int objectId;

    public EnemyStateMachine EnemyStateMachine => m_enemyStateMachine;

    public int ObjectID => objectId;


    public Transform Transform => transform;
    public GameObject GameObject => gameObject;

    private void OnEnable()
    {

    }
}
