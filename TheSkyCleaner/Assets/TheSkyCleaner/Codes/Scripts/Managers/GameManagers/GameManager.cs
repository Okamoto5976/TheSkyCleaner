using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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
    [SerializeField] private ParticleSystem m_deathbossParticle;

    [SerializeField] private SaveManager m_saveManager;
    [SerializeField] private FadeManager m_fadeManager;
    [SerializeField] private GameMenuManager m_gameMenuManager;//result��menu�J���Ȃ��悤��
    [SerializeField] private SkillAdapt m_skilladapt;
    [SerializeField] private EnemyManager m_EM;
    [SerializeField] private TrashManager m_TM;
    [SerializeField] private LargeTrashManager m_LTM;

    [SerializeField] private GameObject m_resultScreen;
    [SerializeField] private ResultScreen m_result;
    [SerializeField] private TextMeshProUGUI m_phaseText;

    [SerializeField] private FloatContainer m_reticleControll;
    [SerializeField] private IntegerContainer m_bossPhase;
    [SerializeField] private BooleanContainer m_isDamageInvulnerable;

    [SerializeField] private GameObject m_boss;

    [SerializeField] private Slider m_slider;

    [SerializeField] private IntegerContainer m_scoreContainer;
    private int m_score;
    private float m_clearTime;

    private bool m_ischeck = false;


    [SerializeField] private AudioSource m_audioSource;
    [SerializeField] private AudioContainer m_buttonSound;
    private void Start()
    {
        m_slider.value = m_reticleControll.Value;
        m_isDamageInvulnerable.SetValue(false);

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

        m_phaseText.text = (m_sequenceIndex + 1).ToString();
        Debug.Log(m_sequenceIndex);
        m_bossPhase.SetValue(m_sequenceIndex);

        //�t�F�[�Y����
        foreach (var phase in m_sequences[m_sequenceIndex].Phase.m_phase)
        {
            var instance = Instantiate(phase);
            instance.Inject(this);
            m_phases.Add(instance);
        }

        NextPhase();

        //�����X�L��Load or �K��
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

        m_ischeck = false;
    }

    private void Update()
    {
        m_clearTime += Time.deltaTime;

        if (m_currentPhase == null) return;

        if(m_currentPhase.OnUpdate(Time.deltaTime))//true �ŏI��
        {
            m_currentPhase.OnExit();
            NextPhase();
        }

        GameOver();

        ////�f�o�b�O�̂���
        //if (Keyboard.current.tKey.wasPressedThisFrame)
        //{
        //    if(m_sequenceIndex < m_sequences.Count - 1)
        //    {
        //        m_sequenceIndex++;
        //    }
        //    m_saveManager.PhaseSave(m_sequenceIndex);

        //    SceneManager.LoadScene(1);
        //}

        ////�f�o�b�O�̂���
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
            //boss��hp < 0�@�̂Ƃ��@���U���g�\��etc...
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
        m_gameMenuManager.m_canCloseMenu = false;

        AddScore();
        m_saveManager.ScoreSave(m_score, m_clearTime);
        m_fadeManager.ChangeScene(m_enhanceScene.Value, false);
    }

    private void GameClear()
    {
        if (m_bossHealth.Value > 0) return;

        if (m_ischeck == true) return;
        m_ischeck = true;
        m_deathbossParticle.Play();

        m_resultScreen.SetActive(true);

        AddScore();
        m_gameMenuManager.m_canCloseMenu = false;
        m_result.Result(m_score,m_clearTime);
        Time.timeScale = 0f;//�~�߂�
    }


    private void GameOver()
    {
        if (m_playerHealth.Value > 0) return;
        m_gameMenuManager.m_canCloseMenu = false;

        AddScore();
        m_saveManager.ScoreSave(m_score, m_clearTime);

        m_fadeManager.ChangeScene(m_enhanceScene.Value, false);

    }

    public void MoveToTitleScene()//���炭���j���[����@timeScale��߂�
    {
        Time.timeScale = 1f;
        m_gameMenuManager.m_canCloseMenu = false;

        m_audioSource.PlayOneShot(m_buttonSound.AudioClip, m_buttonSound.Volume);

        m_fadeManager.ChangeScene(m_titleScene.Value, false);
    }

    public void StartEnemyPool() { m_EM.StartSpawn(); }
    public void StartTrashPool() { m_TM.StartSpawn(); }
    public void StartLargeTrashPool() { m_LTM.StartSpawn(); }
    public void StopEnemyPool() { m_EM.StopSpawn(); }
    public void StopTrashPool() { m_TM.StopSpawn(); }
    public void StopLargeTrashPool() { m_LTM.StopSpawn(); }

    /// <summary>
    /// ���x
    /// </summary>
    public void SetReticleControll(float value)
    {
        m_reticleControll.SetValue(value);

    }

    //--�{���Q�[���X�R�Ascript�ɕ��������--
    //--�ȗ��ȃX�R�A���o������--
    //--save�ł�SO�ł��ۑ����Ă��邽�ߋC�����̈������ƂɂȂ��Ă܂�
    public void AddScore()
    { 
        m_score = m_scoreContainer.Value;
    }
}
