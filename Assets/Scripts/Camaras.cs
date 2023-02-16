using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Camaras
{
    public List<Camara> listaCamaras;
    
    // metodo que realiza la asignacion de los datos con la llamada a la utilidad de unity
    public static Camaras CreateFromJSON(string jsonString)
    {
        return JsonUtility.FromJson<Camaras>(jsonString);
    }

}
