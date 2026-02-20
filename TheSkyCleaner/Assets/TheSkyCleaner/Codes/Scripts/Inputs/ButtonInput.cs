using UnityEngine;


[System.Serializable]
public struct ButtonInput
{
    [SerializeField] private TriggerContainer tap;
    [SerializeField] private BooleanContainer holdState;

    public readonly TriggerContainer Tap => tap;
    public readonly BooleanContainer HoldState => holdState;
}