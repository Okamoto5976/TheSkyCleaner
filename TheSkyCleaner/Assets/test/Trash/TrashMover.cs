using UnityEditor.Rendering;
using UnityEngine;

public class TrashMover : MonoBehaviour
{
    [SerializeField] public Vector3 m_trashMoveSpeed; //生成したオブジェクトの移動速度
    [SerializeField] public int m_DestroyPosZ; //debug用

    void Update()
    {
        //transform.position += new Vector3(
        //    Time.deltaTime * m_trashMoveSpeed.x, 
        //    Time.deltaTime * m_trashMoveSpeed.y, 
        //    Time.deltaTime * m_trashMoveSpeed.z
        //    );

        transform.position += m_trashMoveSpeed * Time.deltaTime;

        //if (transform.position.z >= m_DestroyPosZ)
        //    ///＊Debug用に一時的にDestroyにしている
        //    Destroy(this.gameObject);

        float s = Mathf.Sign((float)m_DestroyPosZ + Mathf.Epsilon);
        // s が +1 なら (z <= m_DestroyPosZ)、s が -1 なら (z >= m_DestroyPosZ) と同等になる
        if (s * transform.position.z >= s * m_DestroyPosZ)
            ///＊Debug用に一時的にDestroyにしている
            Destroy(gameObject); //poolのやり方わかんない
    }

}
