using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private SaveManager m_save;

    //BGMがフェードするのにかかる時間
    public const float m_bgmFadeSpeedHigh = 0.9f;
    public const float m_bgmFadeSpeedLow = 0.3f;
    private float m_bgmFadeSpeedRate = m_bgmFadeSpeedHigh;

    //次流すBGM名
    private AudioSO m_nextBGM;

    //BGMをフェードアウト中か
    private bool m_isFadeOut = false;

    //BGM用、SE用に分けてオーディオソースを持つ
    public AudioSource m_attachBGMSource, m_attachSESource;

    void Awake()
    {

    }

    private void Start()
    {
        var loadAudio = m_save.AudioLoad();
        m_attachBGMSource.volume = loadAudio.data.BGMVolume;
        m_attachSESource.volume = loadAudio.data.SEVolume;
    }

    //=================================================================================
    //SE
    //================================================= ================================

    /// <summary>
    /// 指定したファイル名のSEを流す。第二引数のdelayに指定した時間だけ再生までの間隔を空ける
    /// </summary>
    public void PlaySE(AudioSO SE)
    {
        m_attachSESource.PlayOneShot(SE.Clip,SE.Volum);
    }

    //=================================================================================
    //BGM
    //=================================================================================

    /// <summary>
    /// 指定したファイル名のBGMを流す。ただし既に流れている場合は前の曲をフェードアウトさせてから。
    /// 第二引数のfadeSpeedRateに指定した割合でフェードアウトするスピードが変わる
    /// </summary>
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
            m_attachSESource.volume = loadAudio.data.SEVolume;
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
    /// BGMのボリュームを変更&保存
    /// </summary>
    public void SetBGMVolume(float volume)
    {
        m_attachBGMSource.volume = volume;
        Debug.Log(volume);
        SaveVolume();
    }
    /// <summary>
    /// BGMのボリュームを変更&保存
    /// </summary>
    public void SetSEVolume(float volume)
    {
        m_attachSESource.volume = volume;

        SaveVolume();
    }

    public void SaveVolume()
    {
        Debug.Log("SaveVolume");
        m_save.AudioSave(m_attachBGMSource.volume, m_attachSESource.volume);
    }
}
