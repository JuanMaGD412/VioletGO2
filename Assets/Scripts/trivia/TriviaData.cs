[System.Serializable]
public class TriviaData
{
    public Laberinto[] laberintos;
}

[System.Serializable]
public class Laberinto
{
    public int id;
    public Pregunta[] preguntas;
}

[System.Serializable]
public class Pregunta
{
    public string categoria;
    public string pregunta;
    public string[] opciones;
    public int respuestaCorrecta;
}
