using UnityEngine;

public class TriggerImagen : MonoBehaviour
{
    public ImageManager manager;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            manager.ActivarImagenAleatoria();
        }
    }
}
