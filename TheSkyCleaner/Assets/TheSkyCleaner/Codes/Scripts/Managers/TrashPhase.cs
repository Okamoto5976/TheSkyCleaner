using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/TrashPhase")]
public class TrashPhase : GamePhase
{
    [SerializeField] private float duration = 10f;
    private float timer;

    public override void OnEnter()
    {
        timer = 0f;
        //m_gm.SpawnTrash();
    }

    public override bool OnUpdate(float deltaTime)
    {
        timer += deltaTime;
        return timer >= duration;
    }
}
