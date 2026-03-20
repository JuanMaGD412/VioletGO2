using System.Collections.Generic;
using UnityEngine;

public class BubbleManager : MonoBehaviour
{
    public GameObject prefabBurbuja;
    public Transform contenedor;

    public int maxEnPantalla = 5;

    private JuegoData data;
    private int etapaActual = 0;
    private int indiceBurbuja = 0;
    [Header("Sprites por etapa")]
    public Sprite[] spritesPorEtapa;

    private List<GameObject> burbujasActivas = new List<GameObject>();

    void Start()
    {
        CargarJSON();
        CargarSiguienteGrupo();
    }
    
    void Update()
    {
        DetectarColisiones();
    }


    void CargarJSON()
    {
        TextAsset json = Resources.Load<TextAsset>("burbujas"); // tu archivo en Resources
        data = JsonUtility.FromJson<JuegoData>(json.text);
    }

    void CargarSiguienteGrupo()
    {
        int contador = 0;

        while (contador < maxEnPantalla && indiceBurbuja < data.etapas[etapaActual].burbujas.Count)
        {
            CrearBurbuja(data.etapas[etapaActual].burbujas[indiceBurbuja]);
            indiceBurbuja++;
            contador++;
        }
    }

    void CrearBurbuja(string palabra)
{
    GameObject nueva = Instantiate(prefabBurbuja, contenedor);

    BubbleUI script = nueva.GetComponent<BubbleUI>();
    script.contenido = palabra;
    script.manager = this;

    // 🎨 CAMBIAR SPRITE SEGÚN ETAPA
    UnityEngine.UI.Image img = nueva.GetComponent<UnityEngine.UI.Image>();
    if (img != null && etapaActual < spritesPorEtapa.Length)
    {
        img.sprite = spritesPorEtapa[etapaActual];
    }

    RectTransform rt = nueva.GetComponent<RectTransform>();
    rt.anchoredPosition = GenerarPosicion();

    burbujasActivas.Add(nueva);
}

    Vector2 GenerarPosicion()
    {
        RectTransform contRect = contenedor.GetComponent<RectTransform>();

        float ancho = contRect.rect.width;
        float alto = contRect.rect.height;

        Vector2 nuevaPos;
        bool posicionValida;

        int intentos = 0;

        do
        {
            posicionValida = true;

            float x = Random.Range(-ancho / 2 + 80, ancho / 2 - 80);
            float y = Random.Range(-alto / 2 + 80, alto / 2 - 80);

            nuevaPos = new Vector2(x, y);

            float nuevaBurbujaSize = prefabBurbuja.GetComponent<RectTransform>().rect.width;
            float radioNueva = nuevaBurbujaSize / 2f;

            foreach (var b in burbujasActivas)
            {
                if (b == null) continue;

                RectTransform rtExistente = b.GetComponent<RectTransform>();
                float radioExistente = rtExistente.rect.width / 2f;

                float distancia = Vector2.Distance(
                    rtExistente.anchoredPosition,
                    nuevaPos
                );

                // 🔥 VALIDACIÓN REAL (NO SUPERPOSICIÓN)
                if (distancia < (radioNueva + radioExistente))
                {
                    posicionValida = false;
                    break;
                }
            }


            intentos++;

        } while (!posicionValida && intentos < 50); // evita bucle infinito

        return nuevaPos;
    }



    // 🫧 Se llama cuando una burbuja muere
    public void BurbujaReventada(GameObject burbuja)
    {
        burbujasActivas.Remove(burbuja);

        // Crear otra si aún hay
        if (indiceBurbuja < data.etapas[etapaActual].burbujas.Count)
        {
            CrearBurbuja(data.etapas[etapaActual].burbujas[indiceBurbuja]);
            indiceBurbuja++;
        }
        else if (burbujasActivas.Count == 0)
        {
            Debug.Log("Etapa completada");
        }
    }

    // 🔘 BOTÓN
    public void Saltar()
    {
        // Revienta todas las actuales
        foreach (var b in new List<GameObject>(burbujasActivas))
        {
            if (b != null)
                Destroy(b);
        }

        burbujasActivas.Clear();

        // Si aún hay burbujas en la etapa
        if (indiceBurbuja < data.etapas[etapaActual].burbujas.Count)
        {
            CargarSiguienteGrupo();
        }
        else
        {
            // Pasar de etapa
            etapaActual++;
            indiceBurbuja = 0;

            if (etapaActual < data.etapas.Count)
            {
                CargarSiguienteGrupo();
            }
            else
            {
                Debug.Log("Juego terminado");
            }
        }
    }

    void DetectarColisiones()
    {
        for (int i = 0; i < burbujasActivas.Count; i++)
        {
            for (int j = i + 1; j < burbujasActivas.Count; j++)
            {
                if (burbujasActivas[i] == null || burbujasActivas[j] == null)
                    continue;

                RectTransform a = burbujasActivas[i].GetComponent<RectTransform>();
                RectTransform b = burbujasActivas[j].GetComponent<RectTransform>();

                BubblePhysicsUI physA = burbujasActivas[i].GetComponent<BubblePhysicsUI>();
                BubblePhysicsUI physB = burbujasActivas[j].GetComponent<BubblePhysicsUI>();

                if (physA == null || physB == null) continue;

                float radioA = physA.radio;
                float radioB = physB.radio;

                float distancia = Vector2.Distance(a.anchoredPosition, b.anchoredPosition);

                if (distancia < radioA + radioB)
                {
                    // 🔥 Dirección del choque
                    Vector2 dir = (a.anchoredPosition - b.anchoredPosition).normalized;

                    // 💨 Intercambiar direcciones (rebote simple)
                    Vector2 temp = physA.velocidad;
                    physA.velocidad = physB.velocidad;
                    physB.velocidad = temp;

                    // 💥 Separarlas (CLAVE para evitar que se peguen)
                    float overlap = (radioA + radioB) - distancia;

                    a.anchoredPosition += dir * overlap * 0.5f;
                    b.anchoredPosition -= dir * overlap * 0.5f;

                    // 🫧 EFECTO GEL
                    physA.Impacto();
                    physB.Impacto();
                }
            }
        }
    }

}
