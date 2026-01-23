using UnityEngine;

public class PlayerAttackController : MonoBehaviour
{
    [SerializeField] private PlayerShotHandler m_playerShotHandler;
    [SerializeField] private ArmController m_armController;

    [SerializeField] private InputContainer m_inputContainer;

    private void OnEnable()
    {
        m_inputContainer.MainAction.Tap.OnTrigger += Shoot;
        m_inputContainer.SubAction.Tap.OnTrigger += ShootArm;
    }

    private void OnDisable()
    {
        m_inputContainer.MainAction.Tap.OnTrigger -= Shoot;
        m_inputContainer.SubAction.Tap.OnTrigger -= ShootArm;
    }

    private void Shoot()
    {
        m_playerShotHandler.Shoot();
    }

    private void ShootArm()
    {
        m_armController.ArmShot();
    }
}
