using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TriviaManager : MonoBehaviour
{
    public TriviaLoader loader;

    public TMP_Text textoPregunta;
    public Button[] botonesRespuesta;
    public TMP_Text[] textosBotones;

    private Pregunta[] preguntas;
    private int preguntaActual = 0;
    private int respuestasCorrectas = 0;
    public int numeroLaberinto; // importante asignarlo en Unity
    public TMP_Text textoRangoFinal;

    [Header("Panel Resultado")]
    public GameObject panelResultado;

    public TMP_Text textoAciertos;
    public TMP_Text textoFallos;
    public TMP_Text textoRango;

    void Start()
    {
        loader.OnTriviaLoaded += InicializarTrivia;
    }

    void InicializarTrivia()
    {
        preguntas = loader.ObtenerPreguntas();

        if (preguntas == null || preguntas.Length == 0)
        {
            Debug.LogError("No hay preguntas");
            return;
        }

        preguntaActual = 0;
        MostrarPregunta();
    }

    void MostrarPregunta()
    {
        Pregunta p = preguntas[preguntaActual];

        textoPregunta.text = p.pregunta;

        for (int i = 0; i < botonesRespuesta.Length; i++)
        {
            if (i < p.opciones.Length)
            {
                botonesRespuesta[i].gameObject.SetActive(true);
                textosBotones[i].text = p.opciones[i];

                int index = i;

                botonesRespuesta[i].onClick.RemoveAllListeners();
                botonesRespuesta[i].onClick.AddListener(() => Responder(index));
            }
            else
            {
                botonesRespuesta[i].gameObject.SetActive(false);
            }
        }
    }

    void Responder(int opcion)
    {
        Pregunta p = preguntas[preguntaActual];

        if (opcion == p.respuestaCorrecta)
        {
            Debug.Log("Correcto");
            respuestasCorrectas++; // 👈 IMPORTANTE
        }
        else
        {
            Debug.Log("Incorrecto");
        }

        preguntaActual++;

        if (preguntaActual < preguntas.Length)
            MostrarPregunta();
        else
            FinalizarTrivia(); // 👈 en lugar de solo log
    }

    void FinalizarTrivia()
    {
        Debug.Log("Trivia terminada");

        int labIndex = numeroLaberinto - 1;

        int rango = RangoManager.Instance.CalcularRango(labIndex, respuestasCorrectas);

        string nombreRango = RangosData.ObtenerNombreRango(labIndex, rango);

        int fallos = preguntas.Length - respuestasCorrectas;


        Debug.Log("Aciertos: " + respuestasCorrectas);
        Debug.Log("Rango obtenido: " + nombreRango);

        MostrarPanelResultado(respuestasCorrectas, fallos, nombreRango);        
    }

    void MostrarPanelResultado(int aciertos, int fallos, string rango)
    {
        panelResultado.SetActive(true);

        textoAciertos.text = "Correctas: " + aciertos;
        textoFallos.text = "Incorrectas: " + fallos;

        string icono = "";

        if (rango.Contains("crítico") || rango.Contains("activo") || rango.Contains("transformación"))
            icono = "🥇 ";
        else if (rango.Contains("Analista") || rango.Contains("Identificador") || rango.Contains("Constructor"))
            icono = "🥈 ";
        else
            icono = "🥉 ";

        textoRango.text = "Rango: " + icono + rango;
    }




}
