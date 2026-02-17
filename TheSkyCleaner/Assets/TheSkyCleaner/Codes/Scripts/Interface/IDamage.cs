using UnityEngine;

public interface IDamage : ILockOnTarget
{
    void Damage(int damage);
}
