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
    public List<int> burbujasReventadasPorEtapa = new List<int>();
    public int contadorEtapaActual = 0;
    [Header("UI Resultado")]
    public GameObject panelResultado;
    private int burbujasProcesadasEtapa = 0;
    [Header("Textos por etapa")]
    public TMPro.TextMeshProUGUI textoEtapa1;
    public TMPro.TextMeshProUGUI textoEtapa2;
    public TMPro.TextMeshProUGUI textoEtapa3;

    [Header("Texto estado final")]
    public TMPro.TextMeshProUGUI textoEstado;


    [Header("Termómetro")]
    public TermometroManager termometro;



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

        // ✅ contar reventadas
        contadorEtapaActual++;

    // 🔥 actualizar UI
    termometro.ActualizarTermometro(etapaActual, contadorEtapaActual);


        // ✅ contar progreso total
        burbujasProcesadasEtapa++;

        int totalEtapa = data.etapas[etapaActual].burbujas.Count;

        if (indiceBurbuja < totalEtapa)
        {
            CrearBurbuja(data.etapas[etapaActual].burbujas[indiceBurbuja]);
            indiceBurbuja++;
        }

        // 🔥 CONTROL REAL DE FIN DE ETAPA
        if (burbujasProcesadasEtapa >= totalEtapa)
        {
            // guardar resultado
            burbujasReventadasPorEtapa[etapaActual] = contadorEtapaActual;

            // reset contadores
            contadorEtapaActual = 0;
            burbujasProcesadasEtapa = 0;

            // avanzar etapa
            etapaActual++;
            indiceBurbuja = 0;

            // 👇 esto asegura que arranque limpia la nueva etapa
            termometro.ActualizarTermometro(etapaActual, 0);

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



    public void Saltar()
    {
        // ✅ contar cuántas burbujas se están saltando
        int cantidadSaltadas = burbujasActivas.Count;

        // sumar al progreso total
        burbujasProcesadasEtapa += cantidadSaltadas;

        // destruir burbujas actuales
        foreach (var b in new List<GameObject>(burbujasActivas))
        {
            if (b != null)
                Destroy(b);
        }

        burbujasActivas.Clear();

        int totalEtapa = data.etapas[etapaActual].burbujas.Count;

        // 🔥 MISMA LÓGICA DE FIN DE ETAPA
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
                Debug.Log("Juego terminado");
                MostrarResultadoFinal();
            }
        }
        else
        {
            // cargar más burbujas si aún no termina la etapa
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
        Debug.Log("MOSTRANDO RESULTADO FINAL");
        panelResultado.SetActive(true);

        // ✅ Mostrar resultados por etapa
        textoEtapa1.text = "Etapa 1: " + burbujasReventadasPorEtapa[0];
        textoEtapa2.text = "Etapa 2: " + burbujasReventadasPorEtapa[1];
        textoEtapa3.text = "Etapa 3: " + burbujasReventadasPorEtapa[2];

        // 🔥 Determinar nivel alcanzado
        string estado = "";
        string mensaje = "";

        if (burbujasReventadasPorEtapa[2] > 0)
        {
            estado = "URGENTE";
            mensaje = "Has identificado situaciones graves de violencia.";
        }
        else if (burbujasReventadasPorEtapa[1] > 0)
        {
            estado = "PELIGRO";
            mensaje = "Hay señales importantes de violencia.";
        }
        else if (burbujasReventadasPorEtapa[0] > 0)
        {
            estado = "ALERTA";
            mensaje = "Existen señales iniciales de violencia.";
        }
        else
        {
            estado = "SIN IDENTIFICAR";
            mensaje = "No identificaste situaciones.";
        }

        textoEstado.text = estado + "\n" + mensaje;
    }
}
