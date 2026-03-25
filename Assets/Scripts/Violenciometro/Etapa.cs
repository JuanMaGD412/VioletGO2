using System.Collections.Generic;

[System.Serializable]
public class Etapa
{
    public int spriteIndex;
    public string mensaje;
    public List<string> burbujas;
}


[System.Serializable]
public class JuegoData
{
    public List<Etapa> etapas;
}
