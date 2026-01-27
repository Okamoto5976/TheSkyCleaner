using UnityEngine;

public class T_Collect : MonoBehaviour, ILockOnTarget
{
    [SerializeField] private CollectSO m_collectSO;
    public Transform Transform => transform;
    public GameObject GameObject => gameObject;
    public DropSO GetDropData() => m_collectSO.Drop;

    private void OnEnable()
    {

    }
}
