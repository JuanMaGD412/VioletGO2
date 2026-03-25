using UnityEngine;

public class BubbleWobble : MonoBehaviour
{
    public float velocidad = 2f;
    public float intensidad = 0.1f;

    private Vector3 escalaInicial;

    void Start()
    {
        escalaInicial = transform.localScale;
    }

    void Update()
    {
        float wobble = Mathf.Sin(Time.time * velocidad) * intensidad;

        float escalaX = escalaInicial.x + wobble;
        float escalaY = escalaInicial.y - wobble;

        transform.localScale = new Vector3(escalaX, escalaY, escalaInicial.z);

        // Movimiento flotante tipo burbuja
        float movimientoX = Mathf.Sin(Time.time) * 10f;
        float movimientoY = Mathf.Cos(Time.time * 0.5f) * 20f;

        transform.Translate(new Vector3(movimientoX, movimientoY, 0) * Time.deltaTime);
    }

}
