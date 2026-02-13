using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/WaitPhase")]
public class WaitPhase : GamePhase
{
    public override bool OnUpdate(float deltaTime)
    {
        return true;
    }
}
