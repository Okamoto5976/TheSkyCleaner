using UnityEngine;

public abstract class ItemBase : MonoBehaviour
{
    [SerializeField] private string _name;

    public abstract void UseItem();
}
