using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Phase/WaitPhase")]
public class WaitPhase : GamePhase
{
    [SerializeField] private float duration = 10f;
    private float timer;

    public override void OnEnter()
    {
        timer = 0; 
    }

    public override bool OnUpdate(float deltaTime)
    {

        timer += deltaTime;
        return timer >= duration;
    }

    public override void OnExit() { }
}
