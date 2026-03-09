using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Phase/BossPhase")]
public class BossPhase : GamePhase
{
    [SerializeField] private TriggerContainer m_bossActive;

    public override void OnEnter()
    {
        m_bossActive.Trigger();
    }

    public override bool OnUpdate(float deltaTime)
    {
        return true;
    }

    public override void OnExit()
    {
        
    }
}
