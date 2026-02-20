using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private PhaseSequence m_sequence;
    [SerializeField] private FloatContainer m_fuel;
    [SerializeField] private InventorySO m_inventorySO;

    private List<GamePhase> m_phases = new();
    private int m_currentIndex;
    private GamePhase m_currentPhase;

    [SerializeField] private SaveManager m_saveManager;
    [SerializeField] private EnemyManager m_EM;
    [SerializeField] private TrashManager m_TM;
    [SerializeField] private LargeTrashManager m_LTM;

    [SerializeField] private GameObject m_boss;
   

    private void Start()
    {
        foreach(var phase in m_sequence.m_phase)
        {
            var instance = Instantiate(phase);
            instance.Inject(this);
            m_phases.Add(instance);
        }

        NextPhase();
    }

    private void Update()
    {
        if (m_currentPhase == null) return;

        if(m_currentPhase.OnUpdate(Time.deltaTime))//true で終了
        {
            m_currentPhase.OnExit();
            NextPhase();
        }

        GameOver();
    }

    private void NextPhase()
    {
        if(m_currentIndex >= m_phases.Count)
        {
            return;
        }

        m_currentPhase = m_phases[m_currentIndex];
        m_currentIndex++;
        m_currentPhase.OnEnter();
    }

    public void StartEnemyPool() { m_EM.StartSpawn(); }
    public void StartTrashPool() { m_TM.StartSpawn(); }
    public void StartLargeTrashPool() { m_LTM.StartSpawn(); }
    public void StopEnemyPool() { m_EM.StopSpawn(); }
    public void StopTrashPool() { m_TM.StopSpawn(); }
    public void StopLargeTrashPool() { m_LTM.StopSpawn(); }

    //bossのhp < 0　のとき　リザルト表示etc...
    //

    private void GameOver()
    {
        if (m_fuel.Value > 0) return;
        //inventory save;
        m_saveManager.InventorySave(m_inventorySO.Material);

        //gameSceen = powerSceen;
    }
}
