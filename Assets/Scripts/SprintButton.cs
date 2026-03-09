using UnityEngine;
using UnityEngine.EventSystems;

public class SprintButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private Vector3 escalaOriginal;
    private PlayerMovement player;

    void Start()
    {
        escalaOriginal = transform.localScale;
        player = FindObjectOfType<PlayerMovement>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (player != null)
        {
            player.ToggleSprint();

            
            if (player.mobileSprint)
            {
                transform.localScale = escalaOriginal * 0.85f; 
            }
            else
            {
                transform.localScale = escalaOriginal; 
            }
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {

    }
}
