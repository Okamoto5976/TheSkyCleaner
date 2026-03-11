using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private HealthContainer m_playerHealth;
    [SerializeField] private HealthContainer m_bossHealth;

    [SerializeField] private StringContainer m_titleScene;
    [SerializeField] private StringContainer m_enhanceScene;

    private List<GamePhase> m_phases = new();
    private int m_currentIndex;
    private GamePhase m_currentPhase;
    private int m_sequenceIndex;

    [System.Serializable]
    public struct SequenceLoop
    { 
        [SerializeField] private PhaseSequence m_phase;
        [SerializeField] private bool m_isLoopCheck;
        [SerializeField] private int m_bossRunHealth;
        [SerializeField] private bool m_isBossDownCheck;
        public PhaseSequence Phase { get => m_phase; } 
        public bool IsLoopCheck { get => m_isLoopCheck; }
        public int BossRunHealth { get => m_bossRunHealth; }
        public bool IsBossDownCheck { get => m_isBossDownCheck; }
    }

    [SerializeField] private List<SequenceLoop> m_sequences;

    [SerializeField] private SaveManager m_saveManager;
    [SerializeField] private SkillAdapt m_skilladapt;
    [SerializeField] private EnemyManager m_EM;
    [SerializeField] private TrashManager m_TM;
    [SerializeField] private LargeTrashManager m_LTM;
    [SerializeField] private GameObject m_resultScreen;
    [SerializeField] private ResultScreen m_result;

    [SerializeField] private GameObject m_boss;

    private int m_score;
    private float m_clearTime;

    public int Score { get => m_score; }
    public float ClearTime { get => m_clearTime; }
   

    private void Start()
    {
        //Load SequenseIndex
        var data = m_saveManager.PhaseLoad();
        if(data != null )
        {
            m_sequenceIndex = data.SequenceIndex;
        }
        else
        {
            m_sequenceIndex = 0;
        }

        Debug.Log(m_sequenceIndex);

        //フェーズ処理
        foreach (var phase in m_sequences[m_sequenceIndex].Phase.m_phase)
        {
            var instance = Instantiate(phase);
            instance.Inject(this);
            m_phases.Add(instance);
        }

        NextPhase();

        //強化スキルLoad or 適応
        m_skilladapt.LoadSkillType();

        var gamedata = m_saveManager.CurrentDataLoad();
        if(gamedata != null )
        {
            m_score = gamedata.m_scoredata.m_score;
            m_clearTime = gamedata.m_scoredata.m_clearTime;
        }
        else
        {
            m_score = 0;
            m_clearTime = 0;
        }
    }

    private void Update()
    {
        m_clearTime += Time.deltaTime;

        if (m_currentPhase == null) return;

        if(m_currentPhase.OnUpdate(Time.deltaTime))//true で終了
        {
            m_currentPhase.OnExit();
            NextPhase();
        }

        GameOver();

        ////デバッグのため
        //if (Keyboard.current.tKey.wasPressedThisFrame)
        //{
        //    if(m_sequenceIndex < m_sequences.Count - 1)
        //    {
        //        m_sequenceIndex++;
        //    }
        //    m_saveManager.PhaseSave(m_sequenceIndex);

        //    SceneManager.LoadScene(1);
        //}

        ////デバッグのため
        //if (Keyboard.current.yKey.wasPressedThisFrame)
        //{
        //    m_sequenceIndex = 0;
        //    m_saveManager.PhaseSave(m_sequenceIndex);
        //    Debug.Log(m_sequenceIndex);
        //}

        if (!m_sequences[m_sequenceIndex].IsBossDownCheck)
        {
            PhaseClear();
        }
        else
        {
            //bossのhp < 0　のとき　リザルト表示etc...
            GameClear();
        }
    }

    private void NextPhase()
    {
        if(m_currentIndex >= m_phases.Count)
        {
            if (m_sequences[m_sequenceIndex].IsLoopCheck)
            {
                m_currentIndex = 0;
            }
            else
            {
                return;
            }
        }

        m_currentPhase = m_phases[m_currentIndex];
        m_currentIndex++;
        m_currentPhase.OnEnter();
    }

    private void PhaseClear()
    {
        //SequenseIndex save
        if (m_bossHealth.Value > m_sequences[m_sequenceIndex].BossRunHealth) return;



        if (m_sequenceIndex < m_sequences.Count - 1)
        {
            m_sequenceIndex++;
        }
        m_saveManager.PhaseSave(m_sequenceIndex);

        SceneManager.LoadScene(m_enhanceScene.Value);
    }

    private void GameClear()
    {
        if (m_bossHealth.Value > 0) return;

        m_resultScreen.SetActive(true);
        m_result.Result();
    }


    private void GameOver()
    {
        if (m_playerHealth.Value > 0) return;

        SceneManager.LoadScene(m_enhanceScene.Value);
    }

    public void MoveToTitleScene()
    {
        SceneManager.LoadScene(m_titleScene.Value);
    }

    public void StartEnemyPool() { m_EM.StartSpawn(); }
    public void StartTrashPool() { m_TM.StartSpawn(); }
    public void StartLargeTrashPool() { m_LTM.StartSpawn(); }
    public void StopEnemyPool() { m_EM.StopSpawn(); }
    public void StopTrashPool() { m_TM.StopSpawn(); }
    public void StopLargeTrashPool() { m_LTM.StopSpawn(); }

  
}
