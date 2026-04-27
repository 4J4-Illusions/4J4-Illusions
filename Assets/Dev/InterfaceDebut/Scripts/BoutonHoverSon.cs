using UnityEngine;
using UnityEngine.EventSystems;

public class BoutonHoverSon : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public AudioSource audioSource;
    public AudioClip sonHover;

    private bool estSurvole = false;

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!estSurvole)
        {
            audioSource.PlayOneShot(sonHover);
            estSurvole = true;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        estSurvole = false;
    }
}