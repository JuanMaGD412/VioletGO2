using UnityEngine;

public class TriggerImagen : MonoBehaviour
{
    public ImageManager manager;
    private Collider col;

    private void Start()
    {
        col = GetComponent<Collider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            manager.ActivarImagenAleatoria();


            col.enabled = false;
        }
    }
}
