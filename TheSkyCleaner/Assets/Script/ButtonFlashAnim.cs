using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonFlashAnim : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Animator anim;

    private static readonly int HashFlash = Animator.StringToHash("Flash");
    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("HashFlash");
        anim.SetBool(HashFlash, true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        anim.SetBool(HashFlash, false);
    }
}
