using UnityEngine;

public class T_Enemy : MonoBehaviour, ILockOnTarget
{
    public int objectId;

    public int ObjectID => objectId;
    public Transform Transform => transform;
    public GameObject GameObject => gameObject;

    private void OnEnable()
    {

    }
}
