using UnityEngine;

public class RangoManager : MonoBehaviour
{
    public static RangoManager Instance;

    private bool[] maximoPorLaberinto = new bool[5];

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // 👈 CLAVE
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public int CalcularRango(int laberintoIndex, int aciertos)
    {
        // 🥇 Rango máximo
        if (aciertos == 7)
        {
            if (laberintoIndex == 0 || maximoPorLaberinto[laberintoIndex - 1])
            {
                maximoPorLaberinto[laberintoIndex] = true;
                return 3;
            }
        }

        // 🥈 Rango medio
        if (aciertos >= 6)
        {
            maximoPorLaberinto[laberintoIndex] = false;
            return 2;
        }

        // 🥉 Rango básico
        if (aciertos >= 3)
        {
            maximoPorLaberinto[laberintoIndex] = false;
            return 1;
        }

        // ❌ Sin rango
        maximoPorLaberinto[laberintoIndex] = false;
        return 0;
    }

    public bool TieneMaximoAnterior(int laberintoIndex)
    {
        if (laberintoIndex == 0) return true;
        return maximoPorLaberinto[laberintoIndex - 1];
    }
}
