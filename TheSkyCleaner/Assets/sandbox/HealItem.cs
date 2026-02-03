using UnityEngine;

public class HealItem : ItemBase
{
    [SerializeField] private int _healAmount;
    public override void UseItem()
    {
        Debug.Log($"{name}, Heal at {_healAmount}!");
    }
}
