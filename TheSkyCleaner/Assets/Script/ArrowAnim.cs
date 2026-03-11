using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;

public class ArrowAnim: MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    [SerializeField] private GameObject _arrow;
    public float ARROW_OFFSET = 170;
    private Vector3 _arrowPos;
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        _arrow.SetActive(true);
        _arrowPos = gameObject.transform.position;
        _arrowPos.x = transform.position.x - ARROW_OFFSET;
        _arrow.transform.position = _arrowPos;
    }
    
    public void OnPointerExit(PointerEventData eventData)
    {
        _arrow.SetActive(false);
    }
 
}
