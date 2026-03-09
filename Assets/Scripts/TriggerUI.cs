using UnityEngine;

public class TriggerUI : MonoBehaviour
{
    public GameObject uiPanel;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            uiPanel.SetActive(true);
        }
    }
}