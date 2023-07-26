using System.Collections.Generic;
using UnityEngine;
using static Constantes;

public class ControladorAmarella : MonoBehaviour, InterfazJuego
{
    [Header("Variables")]
    [SerializeField] private float tiempoClicPresionado;
    [SerializeField] private float tiempoEntreClics;
    [SerializeField] private float duraci�nMovimiento;

    [Header("Colores")]
    [SerializeField] private Color colorCompleta;
    [SerializeField] private Color colorAlta;
    [SerializeField] private Color colorBaja;
    [SerializeField] private Color colorFinal;

    [Header("Referencias")]
    [SerializeField] private GameObject[] prefabsObst�culo;
    [SerializeField] private GameObject prefabPart�culasDa�oSuave;
    [SerializeField] private GameObject prefabPart�culasDa�oFuerte;
    [SerializeField] private Transform centro;
    [SerializeField] private SpriteRenderer estadoVida;

    private ControladorJuegos controlador;
    [HideInInspector] public bool activo;

    private List<ObstaculoAmarella> obst�culos;
    private Vector2 posic�onInstanciaci�n;
    private Vector3 movimiento;

    private bool pensando;
    private bool puedeIrAtras;
    private float tempoClic;
    private float tempoMovimiento;

    private void Start()
    {
        controlador = FindFirstObjectByType<ControladorJuegos>();
        posic�onInstanciaci�n = new Vector2(0, 10);
        movimiento = new Vector2(0, -2);

        estadoVida.color = colorCompleta;
        tempoClic = tiempoClicPresionado;
        tempoMovimiento = 0;

        Iniciar();
    }

    private void Iniciar()
    {
        controlador.IniciarVidas(10);
        obst�culos = new List<ObstaculoAmarella>();
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
        var obst�culos = FindObjectsByType<ObstaculoAmarella>(FindObjectsSortMode.None);
        foreach (var obst�culo in obst�culos)
        {
            Destroy(obst�culo.gameObject);
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
    private void InstanciarObstaculo(Vector2 posici�n)
    {
        var aleatorio = Random.Range(0, prefabsObst�culo.Length);
        var nuevoObst�culo = Instantiate(prefabsObst�culo[aleatorio], posici�n, Quaternion.identity);
        obst�culos.Add(nuevoObst�culo.GetComponent<ObstaculoAmarella>());
    }

    private void Avanzar()
    {
        ReiniciarEstado();

        // Mueve todos
        for (int i = 0; i < obst�culos.Count; i++)
        {
            obst�culos[i].Actualizar(movimiento, duraci�nMovimiento);
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
        InstanciarObstaculo(posic�onInstanciaci�n);

        // Borra no usados
        if (obst�culos.Count > 12)
        {
            obst�culos.RemoveAt(0);
            //Destroy(obst�culos[0].gameObject);
        }
    }

    private void Retroceder()
    {
        ReiniciarEstado();

        if (!puedeIrAtras)
            return;

        // Mueve todos
        for (int i = 0; i < obst�culos.Count; i++)
        {
            obst�culos[i].Actualizar(movimiento * -1, duraci�nMovimiento);
        }

        puedeIrAtras = false;
    }

    private void ReiniciarEstado()
    {
        tempoMovimiento = tiempoEntreClics;
        tempoClic = tiempoClicPresionado;
        pensando = false;
    }

    public void RestarVidas(TipoObst�culo tipoObst�culo)
    {
        switch (tipoObst�culo)
        {
            case TipoObst�culo.lento:
                RestarVidas(1);

                var suave = Instantiate(prefabPart�culasDa�oSuave, centro.position, Quaternion.identity);
                Destroy(suave, 1);
                // sonido
                break;
            case TipoObst�culo.r�pido:
                RestarVidas(2);

                var fuerte = Instantiate(prefabPart�culasDa�oFuerte, centro.position, Quaternion.identity);
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
