using UnityEngine;

public class RangoManager : MonoBehaviour
{
    public static RangoManager Instance;

    private bool[] maximoPorLaberinto = new bool[5];
    private bool[,] rangosObtenidos = new bool[5, 3]; // [laberinto, rango]

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            CargarProgreso();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public int CalcularRango(int laberintoIndex, int aciertos)
    {
        int rango = 0;

        // 🥇 Rango máximo con condición
        if (aciertos == 7)
        {
            if (laberintoIndex == 0 || maximoPorLaberinto[laberintoIndex - 1])
            {
                maximoPorLaberinto[laberintoIndex] = true;
                rango = 3;
            }
        }

        // 🥈 Rango medio
        if (rango == 0 && aciertos >= 6)
        {
            maximoPorLaberinto[laberintoIndex] = false;
            rango = 2;
        }
        // 🥉 Rango básico
        else if (rango == 0 && aciertos >= 3)
        {
            maximoPorLaberinto[laberintoIndex] = false;
            rango = 1;
        }
        else if (rango == 0)
        {
            maximoPorLaberinto[laberintoIndex] = false;
        }

        // Guardar logro
        if (rango > 0)
        {
            rangosObtenidos[laberintoIndex, rango - 1] = true;
            GuardarProgreso();
        }

        return rango;
    }

    public bool TieneRango(int laberinto, int rango)
    {
        return rangosObtenidos[laberinto, rango];
    }

    void GuardarProgreso()
    {
        for (int i = 0; i < 5; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                string key = "L" + i + "R" + j;
                PlayerPrefs.SetInt(key, rangosObtenidos[i, j] ? 1 : 0);
            }

            PlayerPrefs.SetInt("MaxL" + i, maximoPorLaberinto[i] ? 1 : 0);
        }

        PlayerPrefs.Save();
    }

    void CargarProgreso()
    {
        for (int i = 0; i < 5; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                string key = "L" + i + "R" + j;
                rangosObtenidos[i, j] = PlayerPrefs.GetInt(key, 0) == 1;
            }

            maximoPorLaberinto[i] = PlayerPrefs.GetInt("MaxL" + i, 0) == 1;
        }
    }
}
