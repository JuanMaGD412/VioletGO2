using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonPressEffect : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private Vector3 escalaOriginal;

    void Start()
    {
        escalaOriginal = transform.localScale;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        transform.localScale = escalaOriginal * 0.9f;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        transform.localScale = escalaOriginal;
    }
}
