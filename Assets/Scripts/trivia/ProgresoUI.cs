using UnityEngine;
using TMPro;

public class ProgresoUI : MonoBehaviour
{
    public TMP_Text[] textos; // 15 textos (5 laberintos x 3 rangos)

    void OnEnable()
    {
        MostrarProgreso();
    }

    void MostrarProgreso()
    {
        int index = 0;

        for (int i = 0; i < 5; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                bool obtenido = RangoManager.Instance.TieneRango(i, j);
                string nombre = RangosData.rangos[i, j];

                textos[index].text = nombre + ": " + (obtenido ? "Obtenido" : "No obtenido");

                index++;
            }
        }
    }
}
