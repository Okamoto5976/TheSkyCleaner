using UnityEngine;
using UnityEngine.InputSystem;

public class T_Spawn : MonoBehaviour
{
    [SerializeField] private CollectPoolManager collectpool;
    [SerializeField] private Transform _transform;

    private void Update()
    {
        if (Keyboard.current.eKey.wasPressedThisFrame)
        {
            Debug.Log("spawn");
            GameObject obj = collectpool.GetObjectFromPool().gameObject;
            obj.SetActive(true);//ƒZƒbƒg

            Vector3 pos = obj.transform.position;
            pos.x += Random.Range(0, 3);
            pos.y += Random.Range(0, 3);
            pos.z += Random.Range(0, 3);
            obj.transform.position = pos;
        }
    }
}
