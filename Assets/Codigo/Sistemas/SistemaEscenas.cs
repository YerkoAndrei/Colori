using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using static Constantes;

public class SistemaEscenas : MonoBehaviour
{
    private static SistemaEscenas instancia;

    [Header("Panel")]
    [SerializeField] private Image panelNegro;
    [SerializeField] private GameObject imgCarga;

    // Lerp
    private float tiempoLerp;
    private float duraciónEntrada = 1f;
    private float duraciónSalida = 0.8f;

    private void Start()
    {
        if (instancia != null)
            Destroy(gameObject);
        else
        {
            instancia = this;
            DontDestroyOnLoad(gameObject);
            Iniciar();
        }
    }

    private void Iniciar()
    {
        panelNegro.gameObject.SetActive(false);
        imgCarga.SetActive(false);
    }

    public static void CambiarEscena(Juegos escena)
    {
        instancia.StartCoroutine(instancia.CambiarEscenaAsíncrona(escena.ToString()));
    }

    private IEnumerator CambiarEscenaAsíncrona(string escena)
    {
        // Entrada
        panelNegro.gameObject.SetActive(true);
        tiempoLerp = 0;

        while (tiempoLerp < duraciónEntrada)
        {
            var entrada = tiempoLerp / duraciónEntrada;
            panelNegro.color = Color.Lerp(Color.clear, Color.black, entrada);

            tiempoLerp += Time.deltaTime;
            yield return null;
        }

        // Fin Lerp Entrada
        imgCarga.SetActive(true);
        panelNegro.color = Color.black;

        // Carga y cambia de escena
        var cargaAsíncrona = SceneManager.LoadSceneAsync(escena, LoadSceneMode.Single);
        yield return new WaitUntil(() => cargaAsíncrona.isDone);

        // Salida
        imgCarga.SetActive(false);
        tiempoLerp = 0;

        while (tiempoLerp < duraciónSalida)
        {
            var salida = tiempoLerp / duraciónSalida;
            panelNegro.color = Color.Lerp(Color.black, Color.clear, salida);

            tiempoLerp += Time.deltaTime;
            yield return null;
        }

        // Fin Lerp Salida
        panelNegro.color = Color.clear;
        panelNegro.gameObject.SetActive(false);
    }
}
