using UnityEngine;

public class TriviaLoader : MonoBehaviour
{
    public TextAsset jsonFile;

    public TriviaData data;

    void Awake()
    {
        data = JsonUtility.FromJson<TriviaData>(jsonFile.text);
    }

    public Pregunta[] ObtenerPreguntasLaberinto(int id)
    {
        foreach (var l in data.laberintos)
        {
            if (l.id == id)
                return l.preguntas;
        }

        return null;
    }
}
