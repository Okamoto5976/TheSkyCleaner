using UnityEngine;

public class T_Enemy : MonoBehaviour, ILockOnTarget, IDamage
{
    [SerializeField] private EnemySO m_enemySO;
    public Transform Transform => transform;
    public GameObject GameObject => gameObject;
    public DropSO GetDropData() => m_enemySO.Drop;


    private int m_attack;
    private int m_hp;

    private void OnEnable()
    {
        m_attack = m_enemySO.Attack;
        m_hp = m_enemySO.HP;
    }

    public void Damage(int damage)
    {
        m_hp -= damage;
        //Debug.Log(m_hp);
    }


}
