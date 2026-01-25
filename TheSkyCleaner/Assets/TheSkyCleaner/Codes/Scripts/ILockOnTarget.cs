using UnityEngine;

public interface ILockOnTarget
{
    Transform Transform { get; }

    GameObject GameObject { get; }
}
