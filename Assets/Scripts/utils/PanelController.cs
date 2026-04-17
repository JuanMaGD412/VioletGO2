using UnityEngine;

public class PanelController : MonoBehaviour
{
    [Header("Panel principal")]
    public GameObject panel;

    [Header("Textos")]
    public GameObject[] textos; // Arrastra aquí los 5 textos

    // Mostrar texto según botón
    public void MostrarTexto(int index)
    {
        panel.SetActive(true);

        // Oculta todos primero
        for (int i = 0; i < textos.Length; i++)
        {
            textos[i].SetActive(false);
        }

        // Activa solo el correspondiente
        if (index >= 0 && index < textos.Length)
        {
            textos[index].SetActive(true);
        }
    }

    // Botón cerrar
    public void CerrarPanel()
    {
        panel.SetActive(false);

        // Opcional: ocultar todos los textos también
        for (int i = 0; i < textos.Length; i++)
        {
            textos[i].SetActive(false);
        }
    }
}