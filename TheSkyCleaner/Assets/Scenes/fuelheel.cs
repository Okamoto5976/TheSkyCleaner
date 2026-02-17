using UnityEngine;

public class Fuelheel : MonoBehaviour
{
    [Header("回復量")]
    [SerializeField] private float m_FuelRecovery = 20f;

    [Header("エフェクト")]
    [SerializeField] private GameObject m_RecoveryEffect;

    private void OnTriggerEnter(Collider collision)
    {
        //fuel持ってるか確認
        if (collision.TryGetComponent<fuel1>(out fuel1 playerFuel))
        {
            //回復
            playerFuel.UseFuel(-m_FuelRecovery);
            Debug.Log($"燃料を{m_FuelRecovery}回復しました");
            //エフェクト
            if (m_RecoveryEffect != null) Instantiate(m_RecoveryEffect, transform.position, Quaternion.identity);
            //消す
            Destroy(gameObject);
        }
    }

}