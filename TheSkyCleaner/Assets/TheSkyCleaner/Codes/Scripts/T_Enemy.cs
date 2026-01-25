using UnityEngine;

public class T_Enemy : MonoBehaviour, ILockOnTarget
{
    public int enemyId;

    public Transform Transform => transform;
    public GameObject GameObject => gameObject;

    private void OnEnable()
    {

    }
}
