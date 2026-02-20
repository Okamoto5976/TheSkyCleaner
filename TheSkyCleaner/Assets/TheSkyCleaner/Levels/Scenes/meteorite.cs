using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Rendering;

public class meteorite : MonoBehaviour
{
    private float m_speed;
    private Vector3 m_SlideDiretion;

    public void Setup(float speed, Vector3 direction)
    {
        m_speed=speed;
       m_SlideDiretion =direction;
    }
    void Update()
    {
     transform.Translate(m_SlideDiretion*m_speed*Time.deltaTime);   

        if(Mathf.Abs(transform.position.x)>150f)
        {
            Destroy(gameObject);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent<fuel1>(out fuel1 player))
        {
            player.UseFuel(9999f);
            Debug.Log("ˆêŒ‚•KEŠZ‘³ˆêG—ô‹óŠW¢‹SšLšaša");
        }
    }
}
