﻿// YerkoAndrei
using UnityEngine;
using static Constantes;

public class ControladorCian : MonoBehaviour, InterfazJuego
{
    [Header("Variables")]
    [SerializeField] private float tiempoInstanciación;
    [SerializeField] private float tiempoAceleración;
    [SerializeField] private float tiempoClicPresionado;
    [SerializeField] private float velocidadNormal;
    [SerializeField] private float velocidadRápida;

    [Header("Colores")]
    [SerializeField] private Color colorCompleta;
    [SerializeField] private Color colorAlta;
    [SerializeField] private Color colorBaja;
    [SerializeField] private Color colorFinal;

    [Header("Movimiento")]
    [SerializeField] private Transform decoCentro;
    [SerializeField] private Transform espada;

    [Header("Referencias")]
    [SerializeField] private SpriteRenderer estadoVida;
    [SerializeField] private GameObject prefabProyectil;
    [SerializeField] private GameObject prefabPartículasDañoSuave;
    [SerializeField] private GameObject prefabPartículasDañoFuerte;
    [SerializeField] private GameObject prefabPartículasPuntoSuave;
    [SerializeField] private GameObject prefabPartículasPuntoFuerte;

    private ControladorJuegos controlador;
    [HideInInspector] public bool activo;

    private bool dirección;
    private bool acelerando;
    private float velocidad;

    private float tempoClic;
    private float tempoAceleración;
    private float tempoInstanciación;

    private void Start()
    {
        controlador = FindFirstObjectByType<ControladorJuegos>();
        Iniciar();
    }

    private void Iniciar()
    {
        controlador.IniciarVidas(10);

        activo = true;
        dirección = true;
        estadoVida.color = colorCompleta;
        espada.rotation = Quaternion.identity;
        decoCentro.rotation = Quaternion.identity;

        velocidad = velocidadNormal;
        tempoAceleración = tiempoAceleración;
        tempoClic = tiempoClicPresionado;
        tempoInstanciación = tiempoInstanciación;
    }

    private void Update()
    {
        if (!activo)
            return;

        if (Input.GetMouseButtonDown(0))
            acelerando = true;

        if (Input.GetMouseButton(0) && acelerando)
        {
            tempoClic -= Time.deltaTime;

            if(tempoClic <= 0)
                Acelerar();
        }

        if (Input.GetMouseButtonUp(0) && tempoClic > 0 && acelerando)
        {
            CambiarDirección();
        }
        else if (Input.GetMouseButtonUp(0) && tempoClic <= 0)
        {
            Desactivar();
        }

        tempoInstanciación -= Time.deltaTime;
        if (tempoInstanciación <= 0)
            InstanciarProyectil();
    }

    private void FixedUpdate()
    {
        if (!activo)
            return;

        Rotar();
    }

    // Interfaz 
    public void Pausar(bool pausa)
    {
        activo = !pausa;
        Desactivar();
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
        estadoVida.color = colorCompleta;
        espada.rotation = Quaternion.identity;
        decoCentro.rotation = Quaternion.identity;

        var proyectiles = FindObjectsByType<ProyectilCian>(FindObjectsSortMode.None);
        foreach (var proyectil in proyectiles)
        {
            Destroy(proyectil.gameObject);
        }
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
    private void InstanciarProyectil()
    {
        tempoInstanciación = tiempoInstanciación;
        var posiciónAleatoria = Random.Range(0, 4);
        var posiciónExacta = Vector2.zero;

        switch (posiciónAleatoria)
        {
            case 0:
                posiciónExacta = new Vector2(Random.Range(12, 14), Random.Range(-10, 10));
                break;
            case 1:
                posiciónExacta = new Vector2(Random.Range(-12, -14), Random.Range(-10, 10));
                break;
            case 2:
                posiciónExacta = new Vector2(Random.Range(-10, 10), Random.Range(-12, -14));
                break;
            case 3:
                posiciónExacta = new Vector2(Random.Range(-10, 10), Random.Range(12, 14));
                break;
        }

        Instantiate(prefabProyectil, posiciónExacta, Quaternion.identity);
    }

    private void CambiarDirección()
    {
        dirección = !dirección;
        tempoClic = tiempoClicPresionado;
        acelerando = false;
    }

    private void Acelerar()
    {
        velocidad = velocidadRápida;
        tempoAceleración -= Time.deltaTime;

        if (tempoAceleración <= 0)
            Desactivar();
    }

    private void Desactivar()
    {
        acelerando = false;
        tempoClic = tiempoClicPresionado;

        tempoAceleración = tiempoAceleración;
        velocidad = velocidadNormal;
    }

    private void Rotar()
    {
        if (dirección)
        {            
            decoCentro.Rotate(0, 0, velocidad * Time.fixedDeltaTime * 100);
            espada.Rotate(0, 0, velocidad * Time.fixedDeltaTime * -100);
        }
        else
        {
            decoCentro.Rotate(0, 0, velocidad * Time.fixedDeltaTime * -100);
            espada.Rotate(0, 0, velocidad * Time.fixedDeltaTime * 100);
        }
    }

    // Centro
    public void RestarVidas(TipoProyectil tipoProyectil, Vector2 posición)
    {
        switch(tipoProyectil)
        {
            case TipoProyectil.suave:
                RestarVidas(1);

                var suave = Instantiate(prefabPartículasDañoSuave, posición, Quaternion.identity);
                Destroy(suave, 1);
                // sonido
                break;
            case TipoProyectil.fuerte:
                RestarVidas(2);

                var fuerte = Instantiate(prefabPartículasDañoFuerte, posición, Quaternion.identity);
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

    // Espada
    public void SumarPuntos(TipoProyectil tipoProyectil, Vector2 posición)
    {
        switch (tipoProyectil)
        {
            case TipoProyectil.suave:
                SumarPuntos(1);

                var suave = Instantiate(prefabPartículasPuntoSuave, posición, Quaternion.identity);
                Destroy(suave, 0.6f);
                // sonido
                break;
            case TipoProyectil.fuerte:
                SumarPuntos(2);

                var fuerte = Instantiate(prefabPartículasPuntoFuerte, posición, Quaternion.identity);
                Destroy(fuerte, 0.6f);
                // sonido
                break;
        }
    }
}
