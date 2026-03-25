using UnityEngine;

public class ButtonPanel : MonoBehaviour
{
    public GameObject panelProgreso;

    public void AbrirPanel()
    {
        panelProgreso.SetActive(true);
    }

    public void CerrarPanel()
    {
        panelProgreso.SetActive(false);
    }
}
