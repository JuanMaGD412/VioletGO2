using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class BubbleUI : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    public TextMeshProUGUI texto;
    public AudioSource audioSource;
    public string contenido;

    public BubbleManager manager;

    private RectTransform rt;
    private Canvas canvas;

    private Vector2 offset;
    private bool seEstaArrastrando = false;

    void Start()
    {
        rt = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();

        texto.text = contenido;
    }

    // 👇 Cuando toca
    public void OnPointerDown(PointerEventData eventData)
    {
        seEstaArrastrando = false;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            rt,
            eventData.position,
            eventData.pressEventCamera,
            out offset
        );
    }

    // 👇 Mientras arrastra
    public void OnDrag(PointerEventData eventData)
    {
        seEstaArrastrando = true;

        Vector2 pos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas.transform as RectTransform,
            eventData.position,
            eventData.pressEventCamera,
            out pos
        );

        rt.anchoredPosition = pos;
    }

    // 👇 Cuando suelta
    public void OnPointerUp(PointerEventData eventData)
    {
        if (!seEstaArrastrando)
        {
            Reventar();
        }
    }

    void Reventar()
    {
        if (audioSource != null)
            audioSource.Play();

        StartCoroutine(AnimacionReventar());
    }

    System.Collections.IEnumerator AnimacionReventar()
    {
        yield return new WaitForSeconds(0.1f);

        manager.BurbujaReventada(gameObject);
        Destroy(gameObject);
    }
}
