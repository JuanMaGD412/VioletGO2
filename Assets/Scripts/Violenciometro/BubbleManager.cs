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
    private List<int> burbujasReventadasPorEtapa = new List<int>();
    private int contadorEtapaActual = 0;
    [Header("UI Resultado")]
    public GameObject panelResultado;
    public TMPro.TextMeshProUGUI textoResultado;



    private List<GameObject> burbujasActivas = new List<GameObject>();

    void Start()
    {
        CargarJSON();

        // Inicializar contadores por etapa
        for (int i = 0; i < data.etapas.Count; i++)
        {
            burbujasReventadasPorEtapa.Add(0);
        }

        CargarSiguienteGrupo();
    }

    
    void Update()
    {
        DetectarColisiones();
    }


    void CargarJSON()
    {
        TextAsset json = Resources.Load<TextAsset>("burbujas"); 
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

                if (distancia < (radioNueva + radioExistente))
                {
                    posicionValida = false;
                    break;
                }
            }


            intentos++;

        } while (!posicionValida && intentos < 50); 

        return nuevaPos;
    }



    public void BurbujaReventada(GameObject burbuja)
    {
        burbujasActivas.Remove(burbuja);

        // ✅ contar
        contadorEtapaActual++;

        if (indiceBurbuja < data.etapas[etapaActual].burbujas.Count)
        {
            CrearBurbuja(data.etapas[etapaActual].burbujas[indiceBurbuja]);
            indiceBurbuja++;
        }
        else
        {
            // ya no hay más burbujas por crear en esta etapa

            // limpiar nulls por seguridad
            burbujasActivas.RemoveAll(b => b == null);

            if (burbujasActivas.Count == 0)
            {
                // guardar resultado de esta etapa
                burbujasReventadasPorEtapa[etapaActual] = contadorEtapaActual;

                contadorEtapaActual = 0;

                etapaActual++;
                indiceBurbuja = 0;

                if (etapaActual < data.etapas.Count)
                {
                    CargarSiguienteGrupo();
                }
                else
                {
                    Debug.Log("Juego terminado");
                    MostrarResultadoFinal();
                }
            }
        }

    }
    public void Saltar()
    {
        foreach (var b in new List<GameObject>(burbujasActivas))
        {
            if (b != null)
                Destroy(b);
        }

        burbujasActivas.Clear();

        if (indiceBurbuja < data.etapas[etapaActual].burbujas.Count)
        {
            CargarSiguienteGrupo();
        }
        else
        {
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
                    Vector2 dir = (a.anchoredPosition - b.anchoredPosition).normalized;

                    Vector2 temp = physA.velocidad;
                    physA.velocidad = physB.velocidad;
                    physB.velocidad = temp;

                    float overlap = (radioA + radioB) - distancia;

                    a.anchoredPosition += dir * overlap * 0.5f;
                    b.anchoredPosition -= dir * overlap * 0.5f;

                    physA.Impacto();
                    physB.Impacto();
                }
            }
        }
    }

    void MostrarResultadoFinal()
    {
        Debug.Log("MOSTRANDO RESULTADO FINAL"); // 👈 prue
        panelResultado.SetActive(true);

        string resumen = "RESULTADOS\n\n";

        for (int i = 0; i < burbujasReventadasPorEtapa.Count; i++)
        {
            resumen += "Etapa " + (i + 1) + ": " +
                    burbujasReventadasPorEtapa[i] + " burbujas\n";
        }

        textoResultado.text = resumen;
    }




}
