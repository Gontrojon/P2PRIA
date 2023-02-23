using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    private string webAPI = "https://servizos.meteogalicia.gal/mgrss/observacion/jsonCamaras.action";
    private Camaras camaras;
    public RawImage imagen;
    public List<Text> texto;
    // variable para el control de descargas
    private bool descarga;
    private int nCamara;

    // Start is called before the first frame update
    void Start()
    {
        descarga = false;
        // Iniciar una corrutina para pedir los datos a una api de una web pasando como parametro la llmada get
        StartCoroutine(GetRequest(webAPI));
    }

    // Update is called once per frame
    void Update()
    {
        // If que controla la pulsacion del espacio para cargar una nueva imagen y que solo admita otra pulsacion cuando la descarga anterior finalice
        if (Input.GetKeyDown(KeyCode.Space) && descarga)
        {
            descarga = false;
            nCamara = Random.Range(0, camaras.listaCamaras.Count);
            StartCoroutine(GetImage(camaras.listaCamaras[nCamara].imaxeCamara));
        }
    }

    IEnumerator GetRequest(string uri)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {
            // Peticion y espera a que la petición sea procesada.
            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.Success)
            {
                // guardamos en un string el contenido json
                string jsonString = webRequest.downloadHandler.text;
                // creamos la clase que almacenara todos los datos
                camaras = Camaras.CreateFromJSON(jsonString);
                // llamamos a la corrutina para la descarga de la imagen
                nCamara = Random.Range(0, camaras.listaCamaras.Count);
                StartCoroutine(GetImage(camaras.listaCamaras[nCamara].imaxeCamara));
            }else{
                Debug.Log("Algo salio mal en la llamada a la API");
            }


        }
    }

    IEnumerator GetImage(string uri)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {
            webRequest.downloadHandler = new DownloadHandlerTexture();
            // Peticion y espera a que la petición sea procesada.
            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.Success)
            {
                // guardado de la imagen descarga en el objeto imagen y mostrada en pantalla
                imagen.texture = ((DownloadHandlerTexture)webRequest.downloadHandler).texture;

                // Asignacion del texto
                texto[0].text = camaras.listaCamaras[nCamara].concello;
                texto[1].text = camaras.listaCamaras[nCamara].dataUltimaAct;
                texto[2].text = camaras.listaCamaras[nCamara].provincia;
                texto[3].text = camaras.listaCamaras[nCamara].nomeCamara;

            }else{
                Debug.Log("Algo salio mal en la descarga de la imagen");
            }

            // cambiamos el bool de control para que el usuario pueda realizar otra descarga
            descarga = true;

        }
    }
}
