using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class fuel : MonoBehaviour
{
    [Header("燃料設定")]
    [SerializeField] private float m_maxFuel = 100f;//満タン
    [SerializeField] private float m_currentFuel;//今の燃料
    [SerializeField] private float m_consumptionRate = 1f;//継続消費
    [SerializeField] private float m_boostFuelCost = 10f;//ブーストとかの奴(一回)

    [Header("移動設定")]
    [SerializeField] private float m_moveSpeed = 5f;//通常速度
    [SerializeField] private float m_boostMultiplier = 2f;//ブースト倍率
    [SerializeField] private float m_boostTimer = 1f;//終わり

    [Header("UI設定")]
    [SerializeField] private Slider m_fuelSliber;

    private bool m_Boosting = false;
    void Start()
    {
        m_currentFuel = m_maxFuel;
        if (m_fuelSliber != null)
        {
            m_fuelSliber.maxValue = m_maxFuel;
            m_fuelSliber.value = m_currentFuel;
        }
    }

    void Update()
    {
        //燃料残ってる
        if (m_currentFuel > 0)
        {
            //時間経過で減る
            m_currentFuel -= m_consumptionRate * Time.deltaTime;

            // if (Keyboard.current.spaceKey.wasPressedThisFrame)
            // {
            //     Debug.Log("押されたよ"+m_currentFuel);
            // }

            //ブースト
            if (Keyboard.current.spaceKey.wasPressedThisFrame && m_currentFuel >= m_boostFuelCost && !m_Boosting)
            {
                StartCoroutine(ActivateBoost());
            }

        }
        else if (m_currentFuel <=0)
        {
            m_currentFuel = 0;
            m_Boosting = false;//おっと燃料無いやん考えて消費しろよ
            if (Keyboard.current.spaceKey.wasPressedThisFrame)
            {
                Debug.Log("燃料無いぞ　残り燃料" + m_currentFuel + "%です　　ほらもうブーストに回せる燃料無いんや");
            }
            StopAllCoroutines();//止まっちゃうよー
        }
        //移動
        Move();

        //UI更新
        if (m_fuelSliber != null)
        {
            m_fuelSliber.value = m_currentFuel;
        }
    }
    private void Move()
    {
        //燃料無いよー動けないよー
        if (m_currentFuel <= 0) return;
        float currentSpeed = m_Boosting ? m_moveSpeed * m_boostMultiplier : m_moveSpeed;
        transform.Translate(Vector3.forward * currentSpeed * Time.deltaTime);
    }

    public void UseFuel(float amount)
    {
        m_currentFuel -= amount;
        m_currentFuel=Mathf.Clamp(m_currentFuel,0,m_maxFuel);
        if(m_currentFuel <= 0)
        {
            m_Boosting = false;
        }
   
    }
   
    IEnumerator ActivateBoost()
    {
        m_Boosting = true;//起動
        Debug.Log("ブーーーーストー! 　残り燃料" + (m_currentFuel-10) + "%です");
        float timer=0f;//燃料消費
        float fueltime=m_boostFuelCost/m_boostTimer;
        while (timer < m_boostTimer)
        { 
            if (m_currentFuel <= 0) break;
       
            UseFuel(fueltime * Time.deltaTime);
       
            timer += Time.deltaTime;
            yield return null; // 1フレーム待機
        }
       
        yield return new WaitForSeconds(m_boostTimer);//ブースト中

        m_Boosting = false;//おわた
        Debug.Log("ブースト終わり");
    }
}
