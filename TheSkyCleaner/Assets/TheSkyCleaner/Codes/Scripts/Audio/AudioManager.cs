using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private SaveManager m_save;

    //ボリューム保存用のkeyとデフォルト値
    //private const string BGM_VOLUME_KEY = "BGM_VOLUME_KEY";
    //private const string SE_VOLUME_KEY = "SE_VOLUME_KEY";
    //private const float BGM_VOLUME_DEFULT = 1.0f;
    //private const float SE_VOLUME_DEFULT = 1.0f;

    //BGMがフェードするのにかかる時間
    public const float m_bgmFadeSpeedHigh = 0.9f;
    public const float m_bgmFadeSpeedLow = 0.3f;
    private float m_bgmFadeSpeedRate = m_bgmFadeSpeedHigh;

    //次流すBGM名、SE名
    //private string _nextBGMName;
    //private string _nextSEName;
    private AudioSO m_nextBGM;

    //BGMをフェードアウト中か
    private bool m_isFadeOut = false;

    //BGM用、SE用に分けてオーディオソースを持つ
    public AudioSource m_attachBGMSource, m_attachSESource;

    //全Audioを保持
    //private Dictionary<string, AudioClip> _bgmDic, _seDic;

    void Awake()
    {
        //if (Instance == null)
        //{
        //    Instance = this;   //一応
        //    DontDestroyOnLoad(gameObject);
        //}
        //else
        //{
        //    Destroy(gameObject);
        //}

        // リソースフォルダから全SE & BGMのファイルを読み込みセット

        //_bgmDic = new Dictionary<string, AudioClip>();
        //_seDic = new Dictionary<string, AudioClip>();

        //foreach (var data in _audiosourceSO.BGMList)
        //    _bgmDic[data.Audiosource] = data.Clip;

        //foreach (var data in _audiosourceSO.SEList)
        //    _seDic[data.Audiosource] = data.Clip;
    }

    private void Start()
    {
        var loadAudio = m_save.AudioLoad();
        m_attachBGMSource.volume = loadAudio.data.BGMVolume;
        m_attachSESource.volume = loadAudio.data.BGMVolume;
    }

    //=================================================================================
    //SE
    //================================================= ================================

    /// <summary>
    /// 指定したファイル名のSEを流す。第二引数のdelayに指定した時間だけ再生までの間隔を空ける
    /// </summary>
    //public void PlaySE(string seName, float delay = 0.0f)
    //{
    //    if (!_seDic.ContainsKey(seName))
    //    {
    //        Debug.Log(seName + "という名前のSEがありません");
    //        return;
    //    }

    //    _nextSEName = seName;
    //    Invoke("DelayPlaySE", delay);
    //}

    //private void DelayPlaySE()
    //{
    //    AttachSESource.PlayOneShot(_seDic[_nextSEName]);
    //}

    public void PlaySE(AudioSO SE)
    {
        m_attachSESource.PlayOneShot(SE.Clip);
    }

    //=================================================================================
    //BGM
    //=================================================================================

    /// <summary>
    /// 指定したファイル名のBGMを流す。ただし既に流れている場合は前の曲をフェードアウトさせてから。
    /// 第二引数のfadeSpeedRateに指定した割合でフェードアウトするスピードが変わる
    /// </summary>
    //public void PlayBGM(string bgmName, float fadeSpeedRate = BGM_FADE_SPEED_RATE_HIGH)
    //{
    //    if (!_bgmDic.ContainsKey(bgmName))
    //    {
    //        Debug.Log(bgmName + "という名前のBGMがありません");
    //        return;
    //    }

    //    //現在BGMが流れていない時はそのまま流す
    //    if (!AttachBGMSource.isPlaying)
    //    {
    //        _nextBGMName = "";
    //        AttachBGMSource.clip = _bgmDic[bgmName];
    //        AttachBGMSource.Play();
    //    }
    //    //違うBGMが流れている時は、流れているBGMをフェードアウトさせてから次を流す。同じBGMが流れている時はスルー
    //    else if (AttachBGMSource.clip.name != bgmName)
    //    {
    //        _nextBGMName = bgmName;
    //        FadeOutBGM(fadeSpeedRate);
    //    }

    //}

    public void PlayBGM(AudioSO BGM,float fadeSpeedRate = m_bgmFadeSpeedHigh)
    {
        if (m_attachBGMSource.isPlaying && m_attachBGMSource.clip == BGM.Clip)
            return;

        if(!m_attachBGMSource.isPlaying)
        {
            ApplyBGM(BGM);
        }
        else if(m_attachBGMSource.clip != BGM.Clip)
        {
            m_nextBGM = BGM;
            FadeOutBGM(fadeSpeedRate);
        }
    }

    private void ApplyBGM(AudioSO BGM)
    {
        m_attachBGMSource.clip = BGM.Clip;
        m_attachBGMSource.volume = BGM.Volum;
        m_attachBGMSource.loop = BGM.Loop;
        m_attachBGMSource.Play();
    }
    /// <summary>
    /// 現在流れている曲をフェードアウトさせる
    /// fadeSpeedRateに指定した割合でフェードアウトするスピードが変わる
    /// </summary>
    public void FadeOutBGM(float fadeSpeedRate = m_bgmFadeSpeedLow)
    {
        m_bgmFadeSpeedRate = fadeSpeedRate;
        m_isFadeOut = true;
    }

    private void Update()
    {
        if (!m_isFadeOut) return;


        //徐々にボリュームを下げていき、ボリュームが0になったらボリュームを戻し次の曲を流す
        m_attachBGMSource.volume -= Time.deltaTime * m_bgmFadeSpeedRate;
        if (m_attachBGMSource.volume <= 0)
        {
            m_attachBGMSource.Stop();
            var loadAudio = m_save.AudioLoad();
            m_attachBGMSource.volume = loadAudio.data.BGMVolume;
            m_attachSESource.volume = loadAudio.data.BGMVolume;
            m_isFadeOut = false;

            if (m_nextBGM != null)
            {
                ApplyBGM(m_nextBGM);
                m_nextBGM = null;
            }
        }

    }

    //=================================================================================
    //音量変更
    //=================================================================================

    /// <summary>
    /// BGMとSEのボリュームを別々に変更&保存
    /// </summary>
    public void ChangeVolume(float BGMVolume, float SEVolume)
    {
        m_attachBGMSource.volume = BGMVolume;
        m_attachSESource.volume = SEVolume;

        // JSONに保存
        m_save.AudioSave(BGMVolume, SEVolume);
    }
}
