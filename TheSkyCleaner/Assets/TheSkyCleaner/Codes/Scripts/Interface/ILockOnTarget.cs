using UnityEngine;

public interface ILockOnTarget
{
    Transform Transform { get; }

    GameObject GameObject { get; }

    Vector3 ReticleOffset { get; }

    Vector3 ReticlePosition => Transform.position + ReticleOffset;

    bool IsActive => GameObject.activeSelf;

    DropSO GetDropData();

    DropSO Collect();
}
