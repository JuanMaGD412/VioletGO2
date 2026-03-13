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
            Debug.Log("Correcto");
        else
            Debug.Log("Incorrecto");

        preguntaActual++;

        if (preguntaActual < preguntas.Length)
            MostrarPregunta();
        else
            Debug.Log("Trivia terminada");
    }
}
