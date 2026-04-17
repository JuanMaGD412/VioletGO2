using System.Collections.Generic;
using UnityEngine;
using TMPro;

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

    public List<int> burbujasReventadasPorEtapa = new List<int>();
    public int contadorEtapaActual = 0;

    [Header("Colores de texto por etapa")]
    public Color[] coloresTextoPorEtapa;    


    [Header("UI Resultado")]
    public GameObject panelResultado;
    private int burbujasProcesadasEtapa = 0;

    [Header("Textos por etapa")]
    public TextMeshProUGUI textoEtapa1;
    public TextMeshProUGUI textoEtapa2;
    public TextMeshProUGUI textoEtapa3;

    [Header("Texto estado final")]
    public TextMeshProUGUI textoEstado;

    [Header("Termómetro")]
    public TermometroManager termometro;

    private List<GameObject> burbujasActivas = new List<GameObject>();

    void Start()
    {
        CargarJSON();

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

        if (script.texto != null && etapaActual < coloresTextoPorEtapa.Length)
        {
            Color c = coloresTextoPorEtapa[etapaActual];
            c.a = 1f; // 🔥 esto es lo que te está faltando
            script.texto.color = c;

        }



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

        contadorEtapaActual++;

        termometro.ActualizarTermometro(etapaActual, contadorEtapaActual);

        burbujasProcesadasEtapa++;

        int totalEtapa = data.etapas[etapaActual].burbujas.Count;

        if (indiceBurbuja < totalEtapa)
        {
            CrearBurbuja(data.etapas[etapaActual].burbujas[indiceBurbuja]);
            indiceBurbuja++;
        }

        if (burbujasProcesadasEtapa >= totalEtapa)
        {
            burbujasReventadasPorEtapa[etapaActual] = contadorEtapaActual;

            contadorEtapaActual = 0;
            burbujasProcesadasEtapa = 0;

            etapaActual++;
            indiceBurbuja = 0;

            termometro.ActualizarTermometro(etapaActual, 0);

            if (etapaActual < data.etapas.Count)
            {
                CargarSiguienteGrupo();
            }
            else
            {
                MostrarResultadoFinal();
            }
        }
    }

    public void Saltar()
    {
        int cantidadSaltadas = burbujasActivas.Count;

        burbujasProcesadasEtapa += cantidadSaltadas;

        foreach (var b in new List<GameObject>(burbujasActivas))
        {
            if (b != null)
                Destroy(b);
        }

        burbujasActivas.Clear();

        int totalEtapa = data.etapas[etapaActual].burbujas.Count;

        if (burbujasProcesadasEtapa >= totalEtapa)
        {
            burbujasReventadasPorEtapa[etapaActual] = contadorEtapaActual;

            contadorEtapaActual = 0;
            burbujasProcesadasEtapa = 0;

            etapaActual++;
            indiceBurbuja = 0;

            if (etapaActual < data.etapas.Count)
            {
                CargarSiguienteGrupo();
            }
            else
            {
                MostrarResultadoFinal();
            }
        }
        else
        {
            CargarSiguienteGrupo();
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
        panelResultado.SetActive(true);

        Color colorEtapa1 = new Color(1f, 0.9f, 0.3f);
        Color colorEtapa2 = new Color(1f, 0.6f, 0.1f);
        Color colorEtapa3 = new Color(1f, 0.2f, 0.2f);

        textoEtapa1.text = "Etapa 1: " + burbujasReventadasPorEtapa[0];
        textoEtapa2.text = "Etapa 2: " + burbujasReventadasPorEtapa[1];
        textoEtapa3.text = "Etapa 3: " + burbujasReventadasPorEtapa[2];

        textoEtapa1.color = colorEtapa1;
        textoEtapa2.color = colorEtapa2;
        textoEtapa3.color = colorEtapa3;

        AplicarOutline(textoEtapa1);
        AplicarOutline(textoEtapa2);
        AplicarOutline(textoEtapa3);
        AplicarOutline(textoEstado);

        string estado = "";
        string mensaje = "";
        int sizeEstado = 50;

        if (burbujasReventadasPorEtapa[2] > 0)
        {
            estado = "URGENTE";
            mensaje = "Has identificado situaciones graves de violencia.";
            textoEstado.color = new Color(1f, 0.1f, 0.1f);
            sizeEstado = 60;
        }
        else if (burbujasReventadasPorEtapa[1] > 0)
        {
            estado = "PELIGRO";
            mensaje = "Hay señales importantes de violencia.";
            textoEstado.color = new Color(1f, 0.5f, 0f);
            sizeEstado = 55;
        }
        else if (burbujasReventadasPorEtapa[0] > 0)
        {
            estado = "ALERTA";
            mensaje = "Existen señales iniciales de violencia.";
            textoEstado.color = new Color(1f, 0.85f, 0.2f);
            sizeEstado = 50;
        }
        else
        {
            estado = "SIN IDENTIFICAR";
            mensaje = "No identificaste situaciones.";
            textoEstado.color = Color.white;
            sizeEstado = 45;
        }

        textoEstado.text =
            "<size=" + sizeEstado + ">" + estado + "</size>\n" +
            "<size=35><color=#FFD966>" + mensaje + "</color></size>";
    }

    void AplicarOutline(TextMeshProUGUI texto)
    {
        texto.fontMaterial.EnableKeyword("OUTLINE_ON");
        texto.outlineWidth = 0.25f;
        texto.outlineColor = Color.black;
    }
}