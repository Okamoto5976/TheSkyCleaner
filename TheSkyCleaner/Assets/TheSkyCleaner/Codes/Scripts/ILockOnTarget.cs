using UnityEngine;

public interface ILockOnTarget
{
    int ObjectID { get; }

    Transform Transform { get; }

    GameObject GameObject { get; }
}
