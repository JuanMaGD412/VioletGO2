using UnityEngine;

public class StatusPaneUIText : MonoBehaviour
{
    public GameObject panel;
    public ScrollCreditosUI scrollTextos;


    public void ClosePanel()
    {
        panel.SetActive(false);
    }
    
    public void OpenPanel()
    {
        panel.SetActive(true);

        if (scrollTextos != null)
        {
            scrollTextos.Reiniciar();
        }
    }
}