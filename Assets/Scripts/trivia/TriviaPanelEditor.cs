using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class TriviaPanelEditor : MonoBehaviour
{
    [Header("Referencias")]
    public TriviaLoader loader;
    public FirebaseTriviaEditor firebase;

    [Header("Lista preguntas")]
    public Transform listaPreguntasContent;
    public GameObject prefabPreguntaItem;

    [Header("Editor")]
    public TMP_InputField inputPregunta;
    public TMP_InputField[] opciones;
    public TMP_Dropdown correcta;

    [Header("Info")]
    public TMP_Text tituloLaberinto;

    private Pregunta[] preguntas;

    int indiceActual = -1;
    int laberintoActual = 1;

    void Start()
    {
        loader.OnTriviaLoaded += Inicializar;
    }

    void Inicializar()
    {
        preguntas = loader.ObtenerPreguntas();

        tituloLaberinto.text = "Laberinto " + laberintoActual;

        GenerarLista();
    }

    void GenerarLista()
    {
        foreach (Transform hijo in listaPreguntasContent)
            Destroy(hijo.gameObject);

        if (preguntas == null)
            return;

        for (int i = 0; i < preguntas.Length; i++)
        {
            int index = i;

            GameObject item = Instantiate(prefabPreguntaItem, listaPreguntasContent);

            TMP_Text texto = item.GetComponentInChildren<TMP_Text>();

            string preview = preguntas[i].pregunta;

            if (preview.Length > 25)
                preview = preview.Substring(0, 25) + "...";

            texto.text = (i + 1) + ". " + preview;

            Button btn = item.GetComponent<Button>();

            btn.onClick.AddListener(() =>
            {
                CargarPregunta(index);
            });
        }
    }

    void CargarPregunta(int index)
    {
        indiceActual = index;

        Pregunta p = preguntas[index];

        inputPregunta.text = p.pregunta;

        for (int i = 0; i < opciones.Length; i++)
        {
            if (i < p.opciones.Length)
                opciones[i].text = p.opciones[i];
            else
                opciones[i].text = "";
        }

        correcta.value = p.respuestaCorrecta;
    }

    public void ActualizarPregunta()
    {
        if (indiceActual < 0)
            return;

        Pregunta p = new Pregunta();

        p.categoria = "VBG";
        p.pregunta = inputPregunta.text;

        p.opciones = new string[]
        {
            opciones[0].text,
            opciones[1].text,
            opciones[2].text,
            opciones[3].text
        };

        p.respuestaCorrecta = correcta.value;

        StartCoroutine(firebase.ActualizarPregunta(laberintoActual, indiceActual, p));
    }

    public void CrearPregunta()
    {
        Pregunta p = new Pregunta();

        p.categoria = "VBG";
        p.pregunta = inputPregunta.text;

        p.opciones = new string[]
        {
            opciones[0].text,
            opciones[1].text,
            opciones[2].text,
            opciones[3].text
        };

        p.respuestaCorrecta = correcta.value;

        StartCoroutine(firebase.CrearPregunta(laberintoActual, p));
    }

    public void EliminarPregunta()
    {
        if (indiceActual < 0)
            return;

        StartCoroutine(firebase.BorrarPregunta(laberintoActual, indiceActual));
    }

    public void NuevaPregunta()
    {
        indiceActual = -1;

        inputPregunta.text = "";

        foreach (var op in opciones)
            op.text = "";

        correcta.value = 0;
    }

    public void SiguienteLaberinto()
    {
        laberintoActual++;

        loader.numeroLaberinto = laberintoActual;

        StartCoroutine(loader.CargarTrivia());
    }

    public void LaberintoAnterior()
    {
        if (laberintoActual <= 1)
            return;

        laberintoActual--;

        loader.numeroLaberinto = laberintoActual;

        StartCoroutine(loader.CargarTrivia());
    }
}
