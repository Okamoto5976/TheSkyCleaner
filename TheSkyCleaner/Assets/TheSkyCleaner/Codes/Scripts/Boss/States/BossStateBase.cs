using UnityEngine;

public abstract class BossStateBase : ScriptableObject
{
    protected bool m_isStateEnd = false;
    public bool IsStateEnd => m_isStateEnd;
    protected int m_actionIndex = 0;
    public void AdvanceAction(BossController bossController)
    {
        m_actionIndex++;
        DoActionOneShot(bossController);
    }

    public abstract float GetActionTime();
    public abstract float EnterAction(BossController controller);
    public abstract bool DoAction(BossController bossController);
    protected abstract bool DoActionOneShot(BossController bossController);
}
