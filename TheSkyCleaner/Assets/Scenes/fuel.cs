using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class fuel : MonoBehaviour
{
    [Header("Debug Fuel Amount")]
    [SerializeField] private float m_kakuninnyouFuel;

    [Header("SO")]
    [SerializeField] private FloatContainer m_fuelContainer;

    [Header("Fuel Settings")]
    [SerializeField] private float m_consumptionRate = 1f;
    [SerializeField] private float m_boostFuelCost = 10f;

    [Header("Move Settings")]
    [SerializeField] private float m_moveSpeed = 5f;
    [SerializeField] private float m_boostMultiplier = 2f;
    [SerializeField] private float m_boostTimer = 1f;

    [Header("UI Settings")]
    [SerializeField] private Slider m_fuelSliber;

    private bool m_Boosting = false;
    private float m_CurrentFuel => m_fuelContainer.Value;
    void Start()
    {
        if (m_fuelSliber != null)
        {
            // Set ui elements
            m_fuelSliber.maxValue = m_fuelContainer.InitialValue;
            m_fuelSliber.value = m_CurrentFuel;
        }
    }

    void Update()
    {
        //燃料残ってる
        if (m_CurrentFuel > 0)
        {
            //時間経過で減る
            UseFuel(m_consumptionRate * Time.deltaTime);

            // if (Keyboard.current.spaceKey.wasPressedThisFrame)
            // {
            //     Debug.Log("押されたよ"+m_currentFuel);
            // }

            //ブースト
            if (Keyboard.current.spaceKey.wasPressedThisFrame && m_CurrentFuel >= m_boostFuelCost && !m_Boosting)
            {
                StartCoroutine(ActivateBoost());
            }

        }
        else //if (m_CurrentFuel <=0)
        {
            if (Keyboard.current.spaceKey.wasPressedThisFrame)
            {
                Debug.Log("燃料無いぞ　残り燃料" + (m_CurrentFuel-10) + "%です　　ほらもうブーストに回せる燃料無いんや");
            }
            m_Boosting = false;
            StopAllCoroutines();//止まっちゃうよー
        }
        //移動
        Move();

        //UI更新
        if (m_fuelSliber != null)
        {
            m_fuelSliber.value = m_CurrentFuel;
        }
        m_kakuninnyouFuel = m_CurrentFuel;
    }
    private void Move()
    {
        //燃料無いよー動けないよー
        if (m_CurrentFuel <= 0) return;
        float currentSpeed = m_Boosting ? m_moveSpeed * m_boostMultiplier : m_moveSpeed;
        transform.Translate(Vector3.forward * currentSpeed * Time.deltaTime);
    }

    public void UseFuel(float amount)
    {
        if (m_fuelContainer == null) return;
        float newValue = Mathf.Clamp(m_CurrentFuel - amount, 0, m_fuelContainer.InitialValue);
        m_fuelContainer.SetValue(newValue);

        if (newValue <= 0)
        {
            m_Boosting = false;
        }
    }

    IEnumerator ActivateBoost()
    {
        m_Boosting = true;//起動
        float predictedFuel = m_CurrentFuel - m_boostFuelCost;
        Debug.Log("ブーーーーストー! 　残り燃料" + Mathf.Max(0,predictedFuel) + "%です");

        float timer = 0f;//燃料消費
        float fueltime = m_boostFuelCost / m_boostTimer;
        while (timer < m_boostTimer)
        {
            if (m_CurrentFuel <= 0) break;

            UseFuel(fueltime * Time.deltaTime);

            timer += Time.deltaTime;
            yield return null; // 1フレーム待機
        }

        m_Boosting = false;//おわた
        Debug.Log("ブースト終わり");
    }
}