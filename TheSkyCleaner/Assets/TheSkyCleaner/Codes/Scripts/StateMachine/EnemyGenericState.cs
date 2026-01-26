using UnityEngine;

public abstract class EnemyGenericState : ScriptableObject
{

    protected Vector3 m_direction;
    protected abstract void UpdateState();
}
