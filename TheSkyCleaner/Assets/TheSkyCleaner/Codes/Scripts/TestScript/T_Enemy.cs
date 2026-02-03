using UnityEngine;

public class T_Enemy : MonoBehaviour, ILockOnTarget, IDamage
{
    [SerializeField] private EnemySO m_enemySO;
    public Transform Transform => transform;
    public GameObject GameObject => gameObject;
    public DropSO GetDropData() => m_enemySO.Drop;

    public int m_hp = 10;

    private void OnEnable()
    {

    }

    public void Damage(int damage)
    {
        m_hp -= damage;
        //Debug.Log(m_hp);
    }
}
