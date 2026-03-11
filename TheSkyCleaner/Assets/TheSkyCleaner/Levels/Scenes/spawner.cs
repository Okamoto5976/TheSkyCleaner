using UnityEngine;

public class spawner : MonoBehaviour
{
    [Header("設定")]
    [SerializeField] private GameObject m_meteoritePrefab;
    [SerializeField] private Transform m_playerTransflom;

    [Header("モード切替")]
    [Tooltip("付ける奥と外すと横")]
    public bool m_mode = false;

    [Header("確率設定(%)")]
    [Range(0, 100)] public float m_meteoProbability = 1.68f;//今の出現確率(1フェイズ200秒なら3回に一回ぐらい出る) フェイズ切り替えの機能や仕組みが分からないからこのスクリプト複製でここの確率弄ればいいか？
    [SerializeField] private float m_gambling = 10f;//抽選

    [Header("隕石の性能")]
    [SerializeField] private float m_meteoritespeed = 10f;
    [SerializeField] private float m_BesideDistance = 15f;//横
    [SerializeField] private float m_BackDistance = 60f;//奥



    void Start()
    {
        InvokeRepeating(nameof(RollOfTheDice), 2f, m_gambling);
    }

    void RollOfTheDice()
    {
        float roll = Random.Range(0f, 100f);
        if (roll <= m_meteoProbability)
        {
            Spawnmeteorit();
        }
        else
        {
            //Debug.Log("出ないハズレか...");
        }
    }
    void Spawnmeteorit()
    {
        if (m_playerTransflom == null) return;

        Vector3 spawnPos;
        Vector3 moveDir;

        if (m_mode)
        {
            bool isLeftCorner = Random.value > 0.5f;

            //奥
            spawnPos = m_playerTransflom.position + m_playerTransflom.forward * m_BackDistance;
            //上に散らす
            spawnPos.y = 50f;
            if (isLeftCorner)
            {
                spawnPos.x = Random.Range(-35f,-60f);
            }
            else
            {
                spawnPos.x = Random.Range(35f, 60f);
            }
            //プレイヤー狙う
            moveDir = (m_playerTransflom.position - spawnPos).normalized;
        }
        else
        {
            //左右どちらか+ue
            bool Left = Random.value > 0.5f;
            float xPos = Left ? -m_BesideDistance : m_BesideDistance;
            float yPos = m_playerTransflom.position.y + 10f;//斜め
            //float yPos=m_playerTransflom.position.y;//真横
            //配置
            spawnPos = new Vector3(xPos, yPos, m_playerTransflom.position.z);
            Vector3 targetPos = m_playerTransflom.position;//斜め
            moveDir = (targetPos - spawnPos).normalized;//斜め
            //moveDir = Left ? Vector3.right : Vector3.left;//真横
        }
        GameObject meteorObj = Instantiate(m_meteoritePrefab, spawnPos, Quaternion.identity);

         //if(m_mode) meteorObj.transform.localScale = Vector3.one*20f;

        meteorObj.GetComponent<meteorite>().Setup(m_meteoritespeed, moveDir);
        //Debug.Log("当たった確変や");
    }
}
