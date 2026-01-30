using UnityEngine;

public class PlayerAttackController : MonoBehaviour
{
    [SerializeField] private InputContainer m_inputContainer;
    [SerializeField] private PlayerShotHandler m_playerShotHandler;
    [SerializeField] private ArmController m_armController;
    [SerializeField] private PlayerChargeAttackHandler m_chargeAttackHandler;

    private void OnEnable()
    {
        m_inputContainer.MainAction.Tap.OnTrigger += Shoot;
        m_inputContainer.SubAction.Tap.OnTrigger += ShootArm;
        m_inputContainer.MainAction.HoldState.OnValueChanged += MainChargeShot;
    }

    private void OnDisable()
    {
        m_inputContainer.MainAction.Tap.OnTrigger -= Shoot;
        m_inputContainer.SubAction.Tap.OnTrigger -= ShootArm;
        m_inputContainer.MainAction.HoldState.OnValueChanged -= MainChargeShot;
    }

    private void MainChargeShot(bool state)
    {
        if (state)
        {
            m_chargeAttackHandler.StartCharge();
        }
        else
        {
            m_chargeAttackHandler.ReleaseCharge();
        }
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
