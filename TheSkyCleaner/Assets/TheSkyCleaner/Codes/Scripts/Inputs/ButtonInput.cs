using UnityEngine;


[System.Serializable]
public struct ButtonInput
{
    [SerializeField] private TriggerContainer tap;
    [SerializeField] private BooleanContainer holdState;
    [SerializeField] private TriggerContainer onPress;

    public readonly TriggerContainer Tap => tap;
    public readonly BooleanContainer HoldState => holdState;
    public readonly TriggerContainer OnPress => onPress;
}