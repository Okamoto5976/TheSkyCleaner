using UnityEngine;

public class Damage : MonoBehaviour
{
    [Header("ダメージ量")]
    [SerializeField] private float m_FuelRecovery = 10f;

    [Header("ダメージエフェクト")]
    [SerializeField] private GameObject m_DamageEffect;

    private void OnTriggerEnter(Collider collision)
    {
        //fuel持ってるか確認
        if (collision.TryGetComponent<fuel1>(out fuel1 playerFuel))
        {
            //回復
            playerFuel.UseFuel(m_FuelRecovery);
            Debug.Log($"燃料が{m_FuelRecovery}洩れました");
            //エフェクト
            if (m_DamageEffect != null) Instantiate(m_DamageEffect, transform.position, Quaternion.identity);
            //消す
            Destroy(gameObject);
        }
    }
}
