using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System;

public class TriviaLoader : MonoBehaviour
{
    public int numeroLaberinto = 1;

    private string baseURL = "https://violeta-go-default-rtdb.firebaseio.com/laberintos/";

    public Laberinto laberinto;

    public event Action OnTriviaLoaded;

    void Start()
    {
        StartCoroutine(CargarTrivia());
    }

    IEnumerator CargarTrivia()
    {
        string url = baseURL + numeroLaberinto + ".json";

        UnityWebRequest request = UnityWebRequest.Get(url);

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            string json = request.downloadHandler.text;

            laberinto = JsonUtility.FromJson<Laberinto>(json);

            Debug.Log("Laberinto cargado desde Firebase");

            OnTriviaLoaded?.Invoke(); // 🔥 Avisar que ya cargó
        }
        else
        {
            Debug.LogError(request.error);
        }
    }

    public Pregunta[] ObtenerPreguntas()
    {
        return laberinto.preguntas;
    }
}
