using UnityEngine;

public class MicroMovimientoCamara : MonoBehaviour
{
    [Header("Referencias")]
    public Transform player;
    public Transform salida;
    public float maxDistance = 60f;

    [Header("Movimiento")]
    public float intensidadMaxPos = 0.03f;
    public float intensidadMaxRot = 0.4f;
    public float velocidad = 0.8f;

    Vector3 posicionInicial;
    Quaternion rotacionInicial;

    void Start()
    {
        posicionInicial = transform.localPosition;
        rotacionInicial = transform.localRotation;
    }

    void LateUpdate()
    {
        float distancia = Vector3.Distance(player.position, salida.position);

        float progreso = Mathf.SmoothStep(0, 1,
            Mathf.Clamp01(1 - (distancia / maxDistance)));

        float respiracion = Mathf.Sin(Time.time * velocidad);

        // Intensidad mayor cuando está lejos
        float intensidadActualPos =
            Mathf.Lerp(intensidadMaxPos, 0.002f, progreso);

        float intensidadActualRot =
            Mathf.Lerp(intensidadMaxRot, 0.05f, progreso);

        // Movimiento posición
        Vector3 offset = new Vector3(
            respiracion * intensidadActualPos,
            respiracion * intensidadActualPos * 0.5f,
            0
        );

        transform.localPosition = posicionInicial + offset;

        // Micro rotación lateral
        float rotZ = respiracion * intensidadActualRot;
        transform.localRotation =
            rotacionInicial * Quaternion.Euler(0, 0, rotZ);
    }
}
