using UnityEngine;

public class DashItem : ItemBase
{
    [SerializeField] private int _dashSpeed;
    public override void UseItem()
    {
        Debug.Log($"{name}, Dash at {_dashSpeed}!");
    }
}
