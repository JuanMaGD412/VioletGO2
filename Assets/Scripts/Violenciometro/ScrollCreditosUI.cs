using UnityEngine;

public class ScrollCreditosUI : MonoBehaviour
{
    [Header("Referencia")]
    public RectTransform contenido; // el texto que se mueve

    [Header("Velocidad")]
    public float velocidadAuto = 30f;
    public float velocidadManual = 80f;

    [Header("Límites")]
    public float limiteArriba = 1000f;
    public float limiteAbajo = 0f;

    [Header("Cierre")]
    public GameObject panel;
    public float tiempoAntesDeCerrar = 3f;

    private bool terminado = false;
    private float timer = 0f;

    void Update()
    {
        if (!terminado)
        {
            ScrollAutomatico();
            ControlManual();
            VerificarFin();
        }
        else
        {
            timer += Time.deltaTime;

            if (timer >= tiempoAntesDeCerrar)
            {
                panel.SetActive(false);
            }
        }
    }

    void ScrollAutomatico()
    {
        contenido.anchoredPosition += Vector2.up * velocidadAuto * Time.deltaTime;
    }

    void ControlManual()
    {
        float input = Input.GetAxis("Vertical"); // teclado (W/S o flechas)

        if (input != 0)
        {
            contenido.anchoredPosition += Vector2.up * input * velocidadManual * Time.deltaTime;
        }

        // 🖱️ Scroll del mouse (PC)
        float scroll = Input.mouseScrollDelta.y;

        if (scroll != 0)
        {
            contenido.anchoredPosition += Vector2.up * scroll * velocidadManual * 0.5f;
        }

        // 📱 Touch (mobile)
        if (Input.touchCount == 1)
        {
            Touch t = Input.GetTouch(0);

            if (t.phase == TouchPhase.Moved)
            {
                contenido.anchoredPosition += new Vector2(0, t.deltaPosition.y);
            }
        }
    }

    void VerificarFin()
    {
        if (contenido.anchoredPosition.y >= limiteArriba)
        {
            terminado = true;
        }
    }

    // 🔄 Para reiniciar si abres el panel otra vez
    public void Reiniciar()
    {
        contenido.anchoredPosition = new Vector2(
            contenido.anchoredPosition.x,
            limiteAbajo
        );

        terminado = false;
        timer = 0f;
    }
}
