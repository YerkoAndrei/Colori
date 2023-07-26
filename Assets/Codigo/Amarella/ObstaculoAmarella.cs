using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Constantes;

public class ObstaculoAmarella : MonoBehaviour
{
    [Ocultar] public TipoObst�culo tipoObst�culo;

    [Header("Colores")]
    [SerializeField] private Color colorLento;
    [SerializeField] private Color colorR�pido;

    [Header("Velocidades")]
    [SerializeField] private float velocidadLenta;
    [SerializeField] private float velocidadR�pida;

    private Transform[] obst�culos;
    private Renderer[] renderers;

    private bool direcci�n;
    private float velocidad;
    private Vector2 inicio;
    private Vector2 objetivo;

    private ControladorAmarella controlador;
    private int cantidadObst�culos;
    private float posici�nPositivaM�xima = 20f;

    private void Start()
    {
        controlador = FindFirstObjectByType<ControladorAmarella>();

        // Obst�culos vac�os
        cantidadObst�culos = transform.childCount;
        if (cantidadObst�culos == 0)
            return;

        renderers = GetComponentsInChildren<Renderer>();

        // Encuentra y posiciona
        var lista = new List<Transform>();
        float espacio = ((posici�nPositivaM�xima * 2f) / (float)cantidadObst�culos);
        float posici�nAcumulada = posici�nPositivaM�xima; 

        for (int i = 0; i < cantidadObst�culos; i++)
        {
            lista.Add(transform.GetChild(i));
            lista[i].position = new Vector2(posici�nAcumulada, transform.position.y);
            posici�nAcumulada -= espacio;
        }
        obst�culos = lista.ToArray();

        // Velocidad y color aleatorios
        var esLento = Random.Range(0f, 1f) >= 0.2f;
        for (int i = 0; i < renderers.Length; i++)
        {
            if (esLento)
            {
                tipoObst�culo = TipoObst�culo.lento;
                renderers[i].material.color = colorLento;
                velocidad = velocidadLenta;
            }
            else
            {
                tipoObst�culo = TipoObst�culo.r�pido;
                renderers[i].material.color = colorR�pido;
                velocidad = velocidadR�pida;
            }
        }

        // Direcci�n aleatoria
        direcci�n = Random.Range(0f, 1f) >= 0.5f;
        if (direcci�n)
        {
            objetivo = new Vector2(posici�nPositivaM�xima, transform.position.y);
            inicio = new Vector2(-(posici�nPositivaM�xima), transform.position.y);
        }
        else
        {
            objetivo = new Vector2(-(posici�nPositivaM�xima), transform.position.y);
            inicio = new Vector2(posici�nPositivaM�xima, transform.position.y);
        }
    }

    private void FixedUpdate()
    {
        if (!controlador.activo || cantidadObst�culos == 0)
            return;
        
        for (int i = 0; i < cantidadObst�culos; i++)
        {
            obst�culos[i].position = Vector2.MoveTowards(obst�culos[i].position, objetivo, velocidad * Time.fixedDeltaTime);
            
            if((direcci�n && (obst�culos[i].position.x >= objetivo.x)) ||
              (!direcci�n && (obst�culos[i].position.x <= objetivo.x)))
                obst�culos[i].position = inicio;
        }
    }

    public void Actualizar(Vector2 movimiento, float duraci�nMovimiento)
    {
        StartCoroutine(Animaci�nMovimiento((transform.position + (Vector3)movimiento), duraci�nMovimiento));

        if (cantidadObst�culos == 0)
            return;

        objetivo += movimiento;
        inicio += movimiento;
    }

    private IEnumerator Animaci�nMovimiento(Vector2 posici�nFinal, float duraci�nLerp)
    {
        float tiempoLerp = 0;
        while (tiempoLerp < duraci�nLerp)
        {
            var tiempo = SistemaAnimacion.EvaluarCurvaR�pida(tiempoLerp / duraci�nLerp);
            transform.position = Vector2.Lerp(transform.position, posici�nFinal, tiempo);

            tiempoLerp += Time.deltaTime;
            yield return null;
        }

        // Fin
        transform.position = (Vector3)posici�nFinal;
    }
}
