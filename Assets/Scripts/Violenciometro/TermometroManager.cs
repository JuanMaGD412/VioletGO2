using System.Collections.Generic;
using UnityEngine;

public class TermometroManager : MonoBehaviour
{
    [Header("Etapas del termómetro")]
    public List<GameObject> etapa1;
    public List<GameObject> etapa2;
    public List<GameObject> etapa3;

    // 🔥 FUNCIÓN PRINCIPAL
    public void ActualizarTermometro(int etapa, int cantidad)
    {
        // Activar etapas anteriores completas
        if (etapa >= 1)
            ActivarListaCompleta(etapa1);

        if (etapa >= 2)
            ActivarListaCompleta(etapa2);

        // Activar parcialmente la actual
        if (etapa == 0)
            ActivarParcial(etapa1, cantidad);

        if (etapa == 1)
            ActivarParcial(etapa2, cantidad);

        if (etapa == 2)
            ActivarParcial(etapa3, cantidad);
    }

    void ActivarListaCompleta(List<GameObject> lista)
    {
        foreach (var obj in lista)
        {
            if (obj != null)
                obj.SetActive(true);
        }
    }

    void ActivarParcial(List<GameObject> lista, int cantidad)
    {
        for (int i = 0; i < lista.Count; i++)
        {
            if (lista[i] != null)
                lista[i].SetActive(i < cantidad);
        }
    }
}
