using UnityEngine;

public class T_Spawn : MonoBehaviour
{
    public ComponentPoolHandler<T_Collect> collectpool;
    [SerializeField] private Transform _transform;

    public void SpawnEnemy()
    {
        Debug.Log("spawn");
        GameObject obj = collectpool.GetObjectFromPool();

        Vector3 pos = obj.transform.position;
        pos.x += Random.Range(-3, 3);
        pos.y += Random.Range(-3, 3);
        obj.transform.position = pos;
    }
}
