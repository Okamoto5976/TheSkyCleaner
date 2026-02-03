using UnityEngine;

public class Caller : MonoBehaviour
{
    [SerializeField] private ItemBase _item;

    [ContextMenu("Use Item")]
    public void UseItem()
    {
        _item.UseItem();
    }

    public void GiveItem(ItemBase item)
    {
        _item = item;
    }
}
