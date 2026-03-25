using UnityEngine;
using UnityEngine.SceneManagement;

public class CambiarEscena : MonoBehaviour
{
    public void CargarEscena(int indice)
    {
        SceneManager.LoadScene(indice);
    }
}