using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Text;

public class FirebaseTriviaEditor : MonoBehaviour
{
    string baseURL = "https://violeta-go-default-rtdb.firebaseio.com/";

    public IEnumerator ActualizarPregunta(int laberinto, int indicePregunta, Pregunta pregunta)
    {
        string url = baseURL + "laberintos/" + laberinto + "/preguntas/" + indicePregunta + ".json";

        string json = JsonUtility.ToJson(pregunta);

        byte[] body = Encoding.UTF8.GetBytes(json);

        UnityWebRequest request = new UnityWebRequest(url, "PUT");
        request.uploadHandler = new UploadHandlerRaw(body);
        request.downloadHandler = new DownloadHandlerBuffer();

        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
            Debug.Log("Pregunta actualizada");
        else
            Debug.LogError(request.error);
    }

    public IEnumerator CrearPregunta(int laberinto, Pregunta pregunta)
    {
        string url = baseURL + "laberintos/" + laberinto + "/preguntas.json";

        string json = JsonUtility.ToJson(pregunta);

        byte[] body = Encoding.UTF8.GetBytes(json);

        UnityWebRequest request = new UnityWebRequest(url, "POST");
        request.uploadHandler = new UploadHandlerRaw(body);
        request.downloadHandler = new DownloadHandlerBuffer();

        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
            Debug.Log("Pregunta creada");
        else
            Debug.LogError(request.error);
    }

    public IEnumerator BorrarPregunta(int laberinto, int indice)
    {
        string url = baseURL + "laberintos/" + laberinto + "/preguntas/" + indice + ".json";

        UnityWebRequest request = UnityWebRequest.Delete(url);

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
            Debug.Log("Pregunta eliminada");
        else
            Debug.LogError(request.error);
    }
}
