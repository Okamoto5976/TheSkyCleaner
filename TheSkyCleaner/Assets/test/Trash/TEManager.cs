using System;
using UnityEngine;

public class TEManager : MonoBehaviour
{
    ///“G‚©ƒSƒ~‚©”»•Ê

    public enum TEType
    {
        Trash,
        Enemy
    }

    public event Action<TEManager, TEType> OnInitialized;

    public TEType CurrentType { get; private set; }

    public void InitializeTE(bool isTrash)//, Vector3 pos, Quaternion rot)
    {
        //FX
        //transform.SetPositionAndRotation(pos, rot);

        //ƒSƒ~‚Ì‚Æ‚µ‚Ä‰Šú‰»
        if (isTrash)
        {
            //ƒSƒ~‚Æ‚¢‚¤î•ñ
            CurrentType = TEType.Trash;

        }
        //“G‚Æ‚µ‚Ä‰Šú‰»
        else if (!isTrash)
        {
            //“G‚Æ‚¢‚¤î•ñ
            CurrentType = TEType.Enemy;
        }

        // ‰Šú‰»Š®—¹‚ğ’Ê’m
        OnInitialized?.Invoke(this, CurrentType);

        //‹N“®‚·‚é
    }


}
