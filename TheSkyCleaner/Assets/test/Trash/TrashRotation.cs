using System.Runtime.CompilerServices;
using UnityEngine;

public class TrashRotation : MonoBehaviour
{

    private Quaternion m_rotation;
    private void Update()
    {
        m_rotation = Random.rotation;
        gameObject.transform.rotation = m_rotation;
    }
}
