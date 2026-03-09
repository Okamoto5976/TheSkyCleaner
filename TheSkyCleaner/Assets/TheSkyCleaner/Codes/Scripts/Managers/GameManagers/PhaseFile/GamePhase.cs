using UnityEngine;

public abstract class GamePhase : ScriptableObject
{
    public enum PhaseType
    {
        None,
        Start,
        Stop
    }


    protected GameManager m_gm;

    public void Inject(GameManager gm)
    {
        m_gm = gm;
    }

    public virtual void OnEnter() { }

    public abstract bool OnUpdate(float deltaTime);

    public virtual void OnExit() { }
}
