using UnityEngine;

public static class RangosData
{
    public static string[,] rangos = new string[5, 3]
    {
        // LABERINTO 1
        {
            "Observador/a",
            "Identificador/a de violencias",
            "Reconocedor/a crítico/a"
        },

        // LABERINTO 2
        {
            "Aprendiz de género",
            "Analista de situaciones",
            "Comprensor/a crítico/a"
        },

        // LABERINTO 3
        {
            "Cuestionador/a inicial",
            "Desmontador/a de estereotipos",
            "Pensador/a crítico/a"
        },

        // LABERINTO 4
        {
            "Agente consciente",
            "Interventor/a responsable",
            "Defensor/a activo/a"
        },

        // LABERINTO 5
        {
            "Promotor/a de respeto",
            "Constructor/a de cambio",
            "Agente de transformación social"
        }
    };

    public static string ObtenerNombreRango(int laberintoIndex, int rango)
    {
        if (rango <= 0) return "Sin rango";

        return rangos[laberintoIndex, rango - 1];
    }
}
