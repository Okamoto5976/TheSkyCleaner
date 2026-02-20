using UnityEngine;

public class spawner : MonoBehaviour
{
    [Header("設定")]
    [SerializeField] private GameObject m_meteoritePrefab;
    [SerializeField] private Transform m_playerTransflom;

    [Header("確率設定(%)")]
    [Range(0, 100)] public float m_meteoProbability = 1.68f;//今の出現確率(1フェイズ200秒なら3回に一回ぐらい出る) フェイズ切り替えの機能や仕組みが分からないからこのスクリプト複製でここの確率弄ればいいか？
    [SerializeField] private float m_gambling = 10f;//抽選

    [Header("隕石の性能")]
    [SerializeField] private float m_meteoritespeed = 10f;
    [SerializeField] private float m_distance = 10f;
    void Start()
    {
       InvokeRepeating(nameof(RollOfTheDice), 2f, m_gambling);
    }

    void RollOfTheDice()
    {
        float roll = Random.Range(0f, 100f);
        if(roll<=m_meteoProbability)
        {
            Spawnmeteorit();
        }
    }
    void Spawnmeteorit()
    {
        if (m_playerTransflom == null) return;

        //左右どちらか
        bool Left = Random.value > 0.5f;
        float xPos = Left ? -m_distance : m_distance;

        //配置
        Vector3 spawnPos = new Vector3(xPos, m_playerTransflom.position.y, m_playerTransflom.position.z);

        //生成してセット
        GameObject meteorObj = Instantiate(m_meteoritePrefab, spawnPos, Quaternion.identity);
        Vector3 moveDir =Left?Vector3.right:Vector3.left;

        meteorObj.GetComponent<meteorite>().Setup(m_meteoritespeed, moveDir);
    }
}
