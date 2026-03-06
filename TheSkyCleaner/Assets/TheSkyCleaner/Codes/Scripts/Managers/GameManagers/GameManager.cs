using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private List<PhaseSequence> m_sequence;
    [SerializeField] private FloatContainer m_fuel;
    //[SerializeField] private InventorySO m_inventorySO;

    private List<GamePhase> m_phases = new();
    private int m_currentIndex;
    private GamePhase m_currentPhase;
    private int m_sequenceIndex;

    [SerializeField] private SaveManager m_saveManager;
    [SerializeField] private SkillAdapt m_skilladapt;
    [SerializeField] private EnemyManager m_EM;
    [SerializeField] private TrashManager m_TM;
    [SerializeField] private LargeTrashManager m_LTM;

    [SerializeField] private GameObject m_boss;
   

    private void Start()
    {
        //Load SequenseIndex
        var data = m_saveManager.PhaseLoad();
        if(data != null )
        {
            m_sequenceIndex = data.SequenceIndex;
        }

        Debug.Log(m_sequenceIndex);

        //フェーズ処理
        foreach (var phase in m_sequence[m_sequenceIndex].m_phase)
        {
            var instance = Instantiate(phase);
            instance.Inject(this);
            m_phases.Add(instance);
        }

        NextPhase();

        //強化スキルLoad or 適応
        m_skilladapt.LoadSkillType();
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

        if (Keyboard.current.tKey.wasPressedThisFrame)
        {
            if(m_sequenceIndex < m_sequence.Count - 1)
            {
                m_sequenceIndex++;
            }
            m_saveManager.PhaseSave(m_sequenceIndex);

            SceneManager.LoadScene(1);
        }

        //デバッグのため
        if (Keyboard.current.yKey.wasPressedThisFrame)
        {
            m_sequenceIndex = 0;
            m_saveManager.PhaseSave(m_sequenceIndex);
            Debug.Log(m_sequenceIndex);
        }

        //bossのhp < 0　のとき　リザルト表示etc...
        PhaseClear();

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

    private void PhaseClear()
    {
        //SequenseIndex save
    }


    private void GameOver()
    {
        if (m_fuel.Value > 0) return;
        //inventory save;
        //m_saveManager.InventorySave(m_inventorySO.Material);

        //gameSceen = powerSceen;
    }

    public void StartEnemyPool() { m_EM.StartSpawn(); }
    public void StartTrashPool() { m_TM.StartSpawn(); }
    public void StartLargeTrashPool() { m_LTM.StartSpawn(); }
    public void StopEnemyPool() { m_EM.StopSpawn(); }
    public void StopTrashPool() { m_TM.StopSpawn(); }
    public void StopLargeTrashPool() { m_LTM.StopSpawn(); }

  
}
