// YerkoAndrei
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using static Constantes;

public class SistemaAnimacion : MonoBehaviour
{
    private static SistemaAnimacion instancia;

    [Header("Curvas")]
    [SerializeField] private AnimationCurve curvaAnimaciónEstandar;

    private void Start()
    {
        if (instancia != null)
            Destroy(gameObject);
        else
        {
            instancia = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public static float EvaluarCurva(float tiempo)
    {
        return instancia.curvaAnimaciónEstandar.Evaluate(tiempo);
    }

    public static void AnimarPanel(RectTransform elemento, float duraciónLerp, bool entrando, bool conCurva, Direcciones dirección, Action alFinal)
    {
        instancia.StartCoroutine(AnimaciónMovimiento(elemento, duraciónLerp, entrando, conCurva, dirección, alFinal));
    }

    public static void AnimarColor(Image elemento, float duraciónLerp, Color colorInicio, Color colorFinal, Action alFinal)
    {
        instancia.StartCoroutine(AnimaciónColor(elemento, duraciónLerp, colorInicio, colorFinal, alFinal));
    }

    public static void AnimarNúmeros(TMP_Text texto, int númeroInicio, int númeroFinal, Action alFinal)
    {
        instancia.StartCoroutine(AnimaciónNúmeros(texto, númeroInicio, númeroFinal, alFinal));
    }

    // Intercalación lineal sin curva
    private static IEnumerator AnimaciónMovimiento(RectTransform elemento, float duraciónLerp, bool entrando, bool conCurva, Direcciones dirección, Action alFinal)
    {
        var posiciónFuera = Vector2.zero;

        switch (dirección)
        {
            case Direcciones.arriba:
                posiciónFuera = new Vector2(0, 2000);
                break;
            case Direcciones.abajo:
                posiciónFuera = new Vector2(0, -2000);
                break;
            case Direcciones.izquierda:
                posiciónFuera = new Vector2(-1500, 0);
                break;
            case Direcciones.derecha:
                posiciónFuera = new Vector2(1500, 0);
                break;
        }

        if (entrando)
            elemento.anchoredPosition = posiciónFuera;
        else
            elemento.anchoredPosition = Vector2.zero;

        float tiempoLerp = 0;
        float tiempo = 0;
        while (tiempoLerp < duraciónLerp)
        {
            if(conCurva)
                tiempo = EvaluarCurva(tiempoLerp / duraciónLerp);
            else
                tiempo = tiempoLerp / duraciónLerp;

            if (entrando)
                elemento.anchoredPosition = Vector2.Lerp(posiciónFuera, Vector2.zero, tiempo);
            else
                elemento.anchoredPosition = Vector2.Lerp(Vector2.zero, posiciónFuera, tiempo);

            tiempoLerp += Time.deltaTime;
            yield return null;
        }

        // Fin
        if (entrando)
            elemento.anchoredPosition = Vector2.zero;
        else
            elemento.anchoredPosition = posiciónFuera;

        if (alFinal != null)
            alFinal.Invoke();
    }

    private static IEnumerator AnimaciónColor(Image elemento, float duraciónLerp, Color colorInicio, Color colorFinal, Action alFinal)
    {
        elemento.color = colorInicio;

        float tiempoLerp = 0;
        while (tiempoLerp < duraciónLerp)
        {
            var tiempo = EvaluarCurva(tiempoLerp / duraciónLerp);
            elemento.color = Color.Lerp(colorInicio, colorFinal, tiempo);

            tiempoLerp += Time.deltaTime;
            yield return null;
        }

        // Fin
        elemento.color = colorFinal;

        if (alFinal != null)
            alFinal.Invoke();
    }

    private static IEnumerator AnimaciónNúmeros(TMP_Text texto, int númeroInicio, int númeroFinal, Action alFinal)
    {
        int númeroActual = númeroInicio;
        yield return new WaitForSeconds(0.4f);

        while (númeroActual != númeroFinal)
        {
            if (númeroActual < númeroFinal)
                texto.text = (númeroActual++).ToString();
            else if (númeroActual > númeroFinal)
                texto.text = (númeroActual--).ToString();

            yield return new WaitForSeconds(0.05f);
        }

        // Fin
        texto.text = númeroFinal.ToString();

        if (alFinal != null)
            alFinal.Invoke();
    }
}
