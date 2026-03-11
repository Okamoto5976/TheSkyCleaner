using UnityEngine;

public interface IDamage : ILockOnTarget
{
    void Damage(int damage);

    bool TryCollect(int damage);
}
