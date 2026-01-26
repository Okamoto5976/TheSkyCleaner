using UnityEngine;

public class PlayerShotHandler : MonoBehaviour
{
    [SerializeField] private ActivateWithTimer m_activator;
    public void Shoot()
    {
        m_activator.StartTimer();
    }
}
