using UnityEngine;

public class BossController : MonoBehaviour, IDamage
{
    [SerializeField] private TriggerContainer m_activeStateTrigger;
    [SerializeField] private HealthContainer m_bossHealth;

    [SerializeField] private BooleanContainer m_isBossActive;
    [SerializeField] private IntegerContainer m_currentBossPhase;
    [SerializeField] private IntegerContainer m_currentBossState;

    private Transform m_transform;


    public Transform Transform => m_transform;
    public GameObject GameObject => gameObject;

    private void Awake()
    {
        m_transform = transform;
    }
    public DropSO Collect()
    {
        throw new System.NotImplementedException();
    }

    public void Damage(int damage)
    {
        m_bossHealth.Damage(damage);
    }

    public DropSO GetDropData()
    {
        throw new System.NotImplementedException();
    }


    private void OnEnable()
    {
        m_activeStateTrigger.OnTrigger += Activate;
    }

    private void OnDisable()
    {
        m_activeStateTrigger.OnTrigger -= Activate;
    }

    private void Update()
    {
        if (!m_isBossActive.Value)
        {
            OnInactive();
            return;
        }
        else
        {
            OnActive();
            return;
        }
    }

    private void OnInactive()
    {

    }

    private void OnActive()
    {
        
    }

    private void Activate()
    {
        m_isBossActive.SetValue(true);
        m_currentBossState.SetValue(0);
    }


    public bool TryCollect(int damage)
    {
        throw new System.NotImplementedException();
    }
}
