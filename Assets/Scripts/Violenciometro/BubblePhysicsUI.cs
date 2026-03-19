using UnityEngine;
using System.Collections;

public class BubblePhysicsUI : MonoBehaviour
{
    [Header("Movimiento")]
    public Vector2 velocidad;
    public float velocidadInicialMin = 50f;
    public float velocidadInicialMax = 120f;

    [Header("Propiedades")]
    public float radio;

    private RectTransform rt;
    private RectTransform contenedor;

    private Vector3 escalaOriginal;

    void Start()
    {
        rt = GetComponent<RectTransform>();
        contenedor = rt.parent as RectTransform;

        // 🔥 Calcular radio REAL según tamaño
        radio = (rt.rect.width * rt.localScale.x) / 2f;

        escalaOriginal = transform.localScale;

        // 🎯 Velocidad aleatoria inicial
        velocidad = new Vector2(
            Random.Range(-velocidadInicialMax, velocidadInicialMax),
            Random.Range(-velocidadInicialMax, velocidadInicialMax)
        );

        // Evitar velocidades muy bajas
        if (velocidad.magnitude < velocidadInicialMin)
        {
            velocidad = velocidad.normalized * velocidadInicialMin;
        }
    }

    void Update()
    {
        Mover();
        LimitarBordes();
    }

    void Mover()
    {
        rt.anchoredPosition += velocidad * Time.deltaTime;
    }

    void LimitarBordes()
    {
        float halfWidth = contenedor.rect.width / 2;
        float halfHeight = contenedor.rect.height / 2;

        Vector2 pos = rt.anchoredPosition;

        // Rebote horizontal
        if (pos.x > halfWidth - radio)
        {
            pos.x = halfWidth - radio;
            velocidad.x *= -1;
            Impacto();
        }
        else if (pos.x < -halfWidth + radio)
        {
            pos.x = -halfWidth + radio;
            velocidad.x *= -1;
            Impacto();
        }

        // Rebote vertical
        if (pos.y > halfHeight - radio)
        {
            pos.y = halfHeight - radio;
            velocidad.y *= -1;
            Impacto();
        }
        else if (pos.y < -halfHeight + radio)
        {
            pos.y = -halfHeight + radio;
            velocidad.y *= -1;
            Impacto();
        }

        rt.anchoredPosition = pos;
    }

    // 💥 EFECTO GEL / ONDULACIÓN
    public void Impacto()
    {
        StopAllCoroutines();
        StartCoroutine(Wobble());
    }

    IEnumerator Wobble()
    {
        float tiempo = 0f;
        float duracion = 0.2f;

        while (tiempo < duracion)
        {
            tiempo += Time.deltaTime;

            float factor = Mathf.Sin(tiempo * 30f) * 0.2f;

            float scaleX = escalaOriginal.x + factor;
            float scaleY = escalaOriginal.y - factor;

            transform.localScale = new Vector3(scaleX, scaleY, 1);

            yield return null;
        }

        transform.localScale = escalaOriginal;
    }
}
