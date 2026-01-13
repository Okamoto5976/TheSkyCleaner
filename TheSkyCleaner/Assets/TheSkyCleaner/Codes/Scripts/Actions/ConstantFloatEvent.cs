using UnityEngine;
using UnityEngine.Events;

public class BulletController : MonoBehaviour
{
    [SerializeField] private UnityEvent<float> m_onMove;
    private void Update()
    {
        m_onMove.Invoke(1);
    }
}
