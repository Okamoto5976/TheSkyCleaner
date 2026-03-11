using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MovementHandler))]
public class BossController : MonoBehaviour, IDamage
{
    [Header("Logger")]
    [SerializeField] private Logger m_logger;

    [System.Serializable]
    private struct STATE
    {
        [SerializeField] private BossStateBase state;
        [SerializeField] private bool isOneTimeOnly;

        public readonly BossStateBase State => state;
        public readonly bool IsOneTimeOnly => isOneTimeOnly;
    }

    [System.Serializable]
    private struct Phase
    {
        [SerializeField] private BossStateBase entryState;
        [SerializeField] private List<STATE> states;
        [SerializeField] private bool isLooping;
        [SerializeField] private BossStateBase exitState;

        public readonly BossStateBase EntryState => entryState;
        public readonly List<STATE> States => states;
        public readonly bool IsLooping => isLooping;
        public readonly BossStateBase ExitState => exitState;
    }

    [SerializeField] private TriggerContainer m_activateStateTrigger;
    [SerializeField] private TriggerContainer m_deactivateStateTrigger;
    [SerializeField] private HealthContainer m_bossHealth;

    [SerializeField] private BooleanContainer m_isBossActive;
    [SerializeField] private IntegerContainer m_currentBossPhaseIndex;
    [SerializeField] private IntegerContainer m_currentBossStateIndex;
    [SerializeField] private Vector3 m_reticleOffset;

    public Vector3 ReticleOffset => m_reticleOffset;
    private int CurrentPhaseIndex
    {
        get { return m_currentBossPhaseIndex.Value; }
        set { m_currentBossPhaseIndex.SetValue(value); }
    }
    private int CurrentStateIndex
    {
        get { return m_currentBossStateIndex.Value; }
        set { m_currentBossStateIndex.SetValue(value); }
    }
    private Phase CurrentPhase => m_phases[CurrentPhaseIndex];
    private BossStateBase CurrentState => 
        m_isEntryState ? CurrentPhase.EntryState :
        m_isExitState ? CurrentPhase.ExitState :
        CurrentPhase.States[CurrentStateIndex].State;

    [SerializeField] private List<Phase> m_phases;

    [Header("Components")]
    [SerializeField] private Animator m_animator;
    private MovementHandler m_movementHandler;
    public MovementHandler MovementHandler => m_movementHandler;

    [Header("Player")]
    [SerializeField] private AxisVector3Container m_playerPosition;
    [SerializeField] private HealthContainer m_playerHealth;
    public Vector3 PlayerPosition => m_playerPosition.Value;
    public HealthContainer PlayerHealth => m_playerHealth;

    private Transform m_transform;
    public Transform Transform => m_transform;
    public GameObject GameObject => gameObject;

    private float m_stateTime = 0;
    public float StateTime => m_stateTime;

    private bool m_isEntryState = true;
    private bool m_isExitState = false;

    private void Awake()
    {
        m_transform = transform;
        m_movementHandler = GetComponent<MovementHandler>();
        m_bossHealth.ResetHealth();
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
        m_activateStateTrigger.OnTrigger += Activate;
        m_deactivateStateTrigger.OnTrigger += Deactivate;
    }

    private void OnDisable()
    {
        m_activateStateTrigger.OnTrigger -= Activate;
        m_deactivateStateTrigger.OnTrigger -= Deactivate;
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
        if (!m_isExitState) return;

        m_stateTime -= Time.deltaTime;
        if (CurrentState.IsStateEnd || m_stateTime <= 0)
        {
            m_isExitState = false;
        }

        CurrentState.DoAction(this);
    }

    private void OnActive()
    {
        // Tick state time
        m_stateTime -= Time.deltaTime;

        if (CurrentState.IsStateEnd)
        {
            if (m_isEntryState)
            {
                m_isEntryState = false;
            }
            else if (CurrentPhase.IsLooping)
            {
                CurrentStateIndex = (CurrentStateIndex + 1) % CurrentPhase.States.Count;
            }
            else
            {
                CurrentStateIndex = Mathf.Min(CurrentStateIndex + 1, CurrentPhase.States.Count - 1);
            }

            m_stateTime = CurrentState.EnterAction(this);
            m_logger.Log($"Next State, Next Action for {m_stateTime}", this);
        }

        if (m_stateTime <= 0)
        {
            CurrentState.AdvanceAction(this);
            m_stateTime = CurrentState.GetActionTime();
            m_logger.Log($"Next Action for {m_stateTime}", this);
        }

        CurrentState.DoAction(this);
    }

    public void PlayAnimation(string animationName)
    {
        m_animator.SetTrigger(animationName);
    }

    private void Activate()
    {
        m_isEntryState = true;
        m_isExitState = false;
        m_isBossActive.SetValue(true);
        CurrentStateIndex = 0;
        m_stateTime = CurrentState.EnterAction(this);
        m_logger.Log($"{m_stateTime}", this);
    }

    private void Deactivate()
    {
        m_isEntryState = false;
        m_isExitState = true;
        m_isBossActive.SetValue(false);
        CurrentStateIndex = 0;
        m_stateTime = CurrentState.EnterAction(this);
        m_logger.Log($"{m_stateTime}", this);
    }


    public bool TryCollect(int damage)
    {
        throw new System.NotImplementedException();
    }
}
