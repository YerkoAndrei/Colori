using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Constantes;

public class ObstaculoAmarella : MonoBehaviour
{
    [Ocultar] public TipoObstáculo tipoObstáculo;

    [Header("Colores")]
    [SerializeField] private Color colorLento;
    [SerializeField] private Color colorRápido;

    [Header("Velocidades")]
    [SerializeField] private float velocidadLenta;
    [SerializeField] private float velocidadRápida;

    private Transform[] obstáculos;
    private Renderer[] renderers;

    private bool dirección;
    private float velocidad;
    private Vector2 inicio;
    private Vector2 objetivo;

    private ControladorAmarella controlador;
    private int cantidadObstáculos;
    private float posiciónPositivaMáxima = 20f;

    private void Start()
    {
        controlador = FindFirstObjectByType<ControladorAmarella>();

        // Obstáculos vacíos
        cantidadObstáculos = transform.childCount;
        if (cantidadObstáculos == 0)
            return;

        renderers = GetComponentsInChildren<Renderer>();

        // Encuentra y posiciona
        var lista = new List<Transform>();
        float espacio = ((posiciónPositivaMáxima * 2f) / (float)cantidadObstáculos);
        float posiciónAcumulada = posiciónPositivaMáxima; 

        for (int i = 0; i < cantidadObstáculos; i++)
        {
            lista.Add(transform.GetChild(i));
            lista[i].position = new Vector2(posiciónAcumulada, transform.position.y);
            posiciónAcumulada -= espacio;
        }
        obstáculos = lista.ToArray();

        // Velocidad y color aleatorios
        var esLento = Random.Range(0f, 1f) >= 0.2f;
        for (int i = 0; i < renderers.Length; i++)
        {
            if (esLento)
            {
                tipoObstáculo = TipoObstáculo.lento;
                renderers[i].material.color = colorLento;
                velocidad = velocidadLenta;
            }
            else
            {
                tipoObstáculo = TipoObstáculo.rápido;
                renderers[i].material.color = colorRápido;
                velocidad = velocidadRápida;
            }
        }

        // Dirección aleatoria
        dirección = Random.Range(0f, 1f) >= 0.5f;
        if (dirección)
        {
            objetivo = new Vector2(posiciónPositivaMáxima, transform.position.y);
            inicio = new Vector2(-(posiciónPositivaMáxima), transform.position.y);
        }
        else
        {
            objetivo = new Vector2(-(posiciónPositivaMáxima), transform.position.y);
            inicio = new Vector2(posiciónPositivaMáxima, transform.position.y);
        }
    }

    private void FixedUpdate()
    {
        if (!controlador.activo || cantidadObstáculos == 0)
            return;
        
        for (int i = 0; i < cantidadObstáculos; i++)
        {
            obstáculos[i].position = Vector2.MoveTowards(obstáculos[i].position, objetivo, velocidad * Time.fixedDeltaTime);
            
            if((dirección && (obstáculos[i].position.x >= objetivo.x)) ||
              (!dirección && (obstáculos[i].position.x <= objetivo.x)))
                obstáculos[i].position = inicio;
        }
    }

    public void Actualizar(Vector2 movimiento, float duraciónMovimiento)
    {
        StartCoroutine(AnimaciónMovimiento((transform.position + (Vector3)movimiento), duraciónMovimiento));

        if (cantidadObstáculos == 0)
            return;

        objetivo += movimiento;
        inicio += movimiento;
    }

    private IEnumerator AnimaciónMovimiento(Vector2 posiciónFinal, float duraciónLerp)
    {
        float tiempoLerp = 0;
        while (tiempoLerp < duraciónLerp)
        {
            var tiempo = SistemaAnimacion.EvaluarCurvaRápida(tiempoLerp / duraciónLerp);
            transform.position = Vector2.Lerp(transform.position, posiciónFinal, tiempo);

            tiempoLerp += Time.deltaTime;
            yield return null;
        }

        // Fin
        transform.position = (Vector3)posiciónFinal;
    }
}
