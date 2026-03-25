using UnityEngine;

public class StatusPaneUI : MonoBehaviour
{
    public GameObject panel;

    public void ClosePanel()
    {
        panel.SetActive(false);
    }
     public void OpenPanel()
    {
        panel.SetActive(true);
    }
}