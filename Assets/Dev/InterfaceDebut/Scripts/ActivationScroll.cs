using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ActivationScroll : MonoBehaviour,
    IPointerEnterHandler,
    IPointerExitHandler
{
    public ScrollRect scrollRect;

    public void OnPointerEnter(PointerEventData eventData)
    {
        scrollRect.vertical = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        scrollRect.vertical = false;
    }
}