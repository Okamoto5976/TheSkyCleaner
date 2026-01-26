using UnityEngine;
using UnityEngine.InputSystem;

public class T_Spawn : MonoBehaviour
{
    [SerializeField] private EnemyPoolManager enemypool;
    [SerializeField] private Transform _transform;

    private void Update()
    {
        if (Keyboard.current.eKey.wasPressedThisFrame)
        {
            Debug.Log("spawn");
            GameObject obj = enemypool.GetObjectFromPool().gameObject;
            obj.SetActive(true);//ƒZƒbƒg

            Vector3 pos = obj.transform.position;
            pos.x += Random.Range(-3, 3);
            pos.y += Random.Range(-3, 3);
            pos.z += Random.Range(-3, 3);
            obj.transform.position = pos;
        }
    }
}
