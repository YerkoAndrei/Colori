using System.Collections.Generic;
using UnityEngine;
using static Constantes;

public class ControladorAmarella : MonoBehaviour, InterfazJuego
{
    [Header("Variables")]
    [SerializeField] private float tiempoClicPresionado;
    [SerializeField] private float tiempoEntreClics;
    [SerializeField] private float duraciónMovimiento;

    [Header("Colores")]
    [SerializeField] private Color colorCompleta;
    [SerializeField] private Color colorAlta;
    [SerializeField] private Color colorBaja;
    [SerializeField] private Color colorFinal;

    [Header("Referencias")]
    [SerializeField] private GameObject[] prefabsObstáculo;
    [SerializeField] private GameObject prefabPartículasDañoSuave;
    [SerializeField] private GameObject prefabPartículasDañoFuerte;
    [SerializeField] private Transform centro;
    [SerializeField] private SpriteRenderer estadoVida;

    private ControladorJuegos controlador;
    [HideInInspector] public bool activo;

    private List<ObstaculoAmarella> obstáculos;
    private Vector2 posicíonInstanciación;
    private Vector3 movimiento;

    private bool pensando;
    private bool puedeIrAtras;
    private float tempoClic;
    private float tempoMovimiento;

    private void Start()
    {
        controlador = FindFirstObjectByType<ControladorJuegos>();
        posicíonInstanciación = new Vector2(0, 10);
        movimiento = new Vector2(0, -2);

        estadoVida.color = colorCompleta;
        tempoClic = tiempoClicPresionado;
        tempoMovimiento = 0;

        Iniciar();
    }

    private void Iniciar()
    {
        controlador.IniciarVidas(10);
        obstáculos = new List<ObstaculoAmarella>();
        activo = true;
        puedeIrAtras = false;

        InstanciarObstaculo(new Vector2(0, 0));
        InstanciarObstaculo(new Vector2(0, 2));
        InstanciarObstaculo(new Vector2(0, 4));
        InstanciarObstaculo(new Vector2(0, 6));
        InstanciarObstaculo(new Vector2(0, 8));
    }

    private void Update()
    {
        if (!activo)
            return;

        if (tempoMovimiento > 0)
        {
            tempoMovimiento -= Time.deltaTime;
            return;
        }

        if (Input.GetMouseButtonDown(0))
            pensando = true;

        if (Input.GetMouseButton(0) && pensando)
        {
            tempoClic -= Time.deltaTime;

            if (tempoClic <= 0)
                Retroceder();
        }

        if (Input.GetMouseButtonUp(0) && tempoClic > 0 && pensando)
            Avanzar();
    }

    // Interfaz 
    public void Pausar(bool pausa)
    {
        activo = !pausa;
    }

    public void Perder()
    {
        activo = false;
    }

    public void Reiniciar()
    {
        Iniciar();
    }

    public void ReiniciarVisual()
    {
        var obstáculos = FindObjectsByType<ObstaculoAmarella>(FindObjectsSortMode.None);
        foreach (var obstáculo in obstáculos)
        {
            Destroy(obstáculo.gameObject);
        }

        estadoVida.color = colorCompleta;
        tempoClic = tiempoClicPresionado;
        tempoMovimiento = 0;
    }

    // Interfaz interna
    public void SumarPuntos(int sumar)
    {
        controlador.SumarPuntaje(sumar);
    }

    public void RestarVidas(int restar)
    {
        controlador.RestartVida(restar);
    }

    // Juego
    private void InstanciarObstaculo(Vector2 posición)
    {
        var aleatorio = Random.Range(0, prefabsObstáculo.Length);
        var nuevoObstáculo = Instantiate(prefabsObstáculo[aleatorio], posición, Quaternion.identity);
        obstáculos.Add(nuevoObstáculo.GetComponent<ObstaculoAmarella>());
    }

    private void Avanzar()
    {
        ReiniciarEstado();

        // Mueve todos
        for (int i = 0; i < obstáculos.Count; i++)
        {
            obstáculos[i].Actualizar(movimiento, duraciónMovimiento);
        }

        // No instancia si viene de vuelta
        if (!puedeIrAtras)
        {
            puedeIrAtras = true;
            return;
        }

        // Verificar actual
        SumarPuntos(1);
        puedeIrAtras = true;

        // Instancia nuevo
        InstanciarObstaculo(posicíonInstanciación);

        // Borra no usados
        if (obstáculos.Count > 12)
        {
            obstáculos.RemoveAt(0);
            //Destroy(obstáculos[0].gameObject);
        }
    }

    private void Retroceder()
    {
        ReiniciarEstado();

        if (!puedeIrAtras)
            return;

        // Mueve todos
        for (int i = 0; i < obstáculos.Count; i++)
        {
            obstáculos[i].Actualizar(movimiento * -1, duraciónMovimiento);
        }

        puedeIrAtras = false;
    }

    private void ReiniciarEstado()
    {
        tempoMovimiento = tiempoEntreClics;
        tempoClic = tiempoClicPresionado;
        pensando = false;
    }

    public void RestarVidas(TipoObstáculo tipoObstáculo)
    {
        switch (tipoObstáculo)
        {
            case TipoObstáculo.lento:
                RestarVidas(1);

                var suave = Instantiate(prefabPartículasDañoSuave, centro.position, Quaternion.identity);
                Destroy(suave, 1);
                // sonido
                break;
            case TipoObstáculo.rápido:
                RestarVidas(2);

                var fuerte = Instantiate(prefabPartículasDañoFuerte, centro.position, Quaternion.identity);
                Destroy(fuerte, 1);
                // sonido
                break;
        }
        
        // Colores
        var vidas = controlador.ObtenerVidas();
        if (vidas > 5)
            estadoVida.color = colorAlta;
        else if (vidas > 1)
            estadoVida.color = colorBaja;
        else if (vidas <= 1)
            estadoVida.color = colorFinal;
    }
}
