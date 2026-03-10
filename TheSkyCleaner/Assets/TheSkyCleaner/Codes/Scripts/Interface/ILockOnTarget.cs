using UnityEngine;

public interface ILockOnTarget
{
    Transform Transform { get; }

    GameObject GameObject { get; }

    Vector3 ReticleOffset { get; }

    bool IsActive => GameObject.activeSelf;

    DropSO GetDropData();

    DropSO Collect();
}
