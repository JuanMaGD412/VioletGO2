using UnityEngine;

public class PanelAyudaUI : MonoBehaviour
{
    public GameObject panelAyuda;

    // 🔘 ABRIR PANEL
    public void AbrirAyuda()
    {
        panelAyuda.SetActive(true);
    }

    // ❌ CERRAR PANEL
    public void CerrarAyuda()
    {
        panelAyuda.SetActive(false);
    }
}
