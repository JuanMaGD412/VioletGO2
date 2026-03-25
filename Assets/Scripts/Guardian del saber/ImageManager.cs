using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageManager : MonoBehaviour
{
    public List<GameObject> imagenes = new List<GameObject>();

    public void ActivarImagenAleatoria()
    {
        if (imagenes.Count == 0)
        {
            Debug.Log("No quedan imágenes disponibles");
            return;
        }

        int indice = Random.Range(0, imagenes.Count);

        GameObject imagen = imagenes[indice];

        imagen.SetActive(true);

        imagenes.RemoveAt(indice);
    }
}
