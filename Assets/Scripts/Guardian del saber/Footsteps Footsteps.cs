using UnityEngine;

public class Footsteps : MonoBehaviour
{
    [Header("Prefabs")]
    public GameObject huellaIzquierdaPrefab;
    public GameObject huellaDerechaPrefab;

    [Header("Configuración")]
    public float distanciaEntreHuellas = 1f;
    public float offsetLateral = 0.2f;
    public float tiempoVida = 10f;

    private Vector3 ultimaPosicion;
    private bool pieIzquierdo = true;

    private CharacterController controller;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        ultimaPosicion = transform.position;
    }

    void Update()
    {
        float distancia = Vector3.Distance(transform.position, ultimaPosicion);

        if (distancia >= distanciaEntreHuellas && controller.velocity.magnitude > 0.1f)
        {
            CrearHuella();
            ultimaPosicion = transform.position;

            pieIzquierdo = !pieIzquierdo;
        }
    }

    void CrearHuella()
    {
        RaycastHit hit;

        Vector3 origen = transform.position + Vector3.up * 0.5f;

        if (Physics.Raycast(origen, Vector3.down, out hit, 10f))
        {
            Vector3 direccion = transform.right;

            float lado = pieIzquierdo ? -1f : 1f;

            Vector3 posicionHuella =
                hit.point + direccion * offsetLateral * lado;

            float anguloPie = pieIzquierdo ? -15f : 15f;

            Quaternion rotacionFinal =
                Quaternion.LookRotation(transform.forward) *
                Quaternion.Euler(90f, anguloPie, 0f);

            GameObject prefabUsar =
                pieIzquierdo ? huellaIzquierdaPrefab : huellaDerechaPrefab;

            GameObject huella = Instantiate(
                prefabUsar,
                posicionHuella + Vector3.up * 0.02f,
                rotacionFinal
            );

            huella.transform.localScale =
                prefabUsar.transform.localScale = new Vector3(0.4385695f, 0.9951918f, 1f);
            Destroy(huella, tiempoVida);
        }
    }
}
