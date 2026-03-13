[System.Serializable]
public class Laberinto
{
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
