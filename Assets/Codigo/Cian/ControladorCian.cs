// YerkoAndrei
using UnityEngine;
using static Constantes;

public class ControladorCian : MonoBehaviour, InterfazJuego
{
    [Header("Variables")]
    [Ocultar] [SerializeField] private int vidas;
    [Ocultar] [SerializeField] private int puntaje;
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

    private bool activo;
    private bool dirección;
    private bool acelerando;
    private float velocidad;

    private float tempoClic;
    private float tempoAceleración;
    private float tempoInstanciación;

    private void Start()
    {
        vidas = 10;
        dirección = true;
        activo = true;
        estadoVida.color = colorCompleta;

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
    public int ObtenerPuntaje()
    {
        return puntaje;
    }

    public void Perder()
    {
        activo = false;

        var proyectiles = FindObjectsOfType<Proyectil>();
        foreach(var proyectil in proyectiles)
        {
            Destroy(proyectil.gameObject);
        }

        FindObjectOfType<ControladorJuegos>().Perder();
    }

    public void Reiniciar()
    {

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
                posiciónExacta = new Vector2(Random.Range(6, 8), Random.Range(-8, 8));
                break;
            case 1:
                posiciónExacta = new Vector2(Random.Range(-6, -8), Random.Range(-8, 8));
                break;
            case 2:
                posiciónExacta = new Vector2(Random.Range(-8, 8), Random.Range(-6, -8));
                break;
            case 3:
                posiciónExacta = new Vector2(Random.Range(-8, 8), Random.Range(6, 8));
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
            decoCentro.Rotate(0, 0, velocidad * Time.deltaTime * 100);
            espada.Rotate(0, 0, velocidad * Time.deltaTime * -100);
        }
        else
        {
            decoCentro.Rotate(0, 0, velocidad * Time.deltaTime * -100);
            espada.Rotate(0, 0, velocidad * Time.deltaTime * 100);
        }
    }

    // Centro
    public void RestarVida(TipoProyectil tipoProyectil)
    {
        switch(tipoProyectil)
        {
            case TipoProyectil.suave:
                vidas -= 1;
                // sonido
                // efecto
                break;
            case TipoProyectil.fuerte:
                vidas -= 2;
                // sonido
                // efecto
                break;
        }

        // Colores
        if (vidas > 5)
            estadoVida.color = colorAlta;
        else if (vidas > 1)
            estadoVida.color = colorBaja;
        else if (vidas <= 1)
            estadoVida.color = colorFinal;

        if (vidas <= 0)
            Perder();
    }

    // Espada
    public void SumarPuntos(TipoProyectil tipoProyectil)
    {
        switch (tipoProyectil)
        {
            case TipoProyectil.suave:
                puntaje += 1;
                // sonido
                // efecto
                break;
            case TipoProyectil.fuerte:
                puntaje += 2;
                // sonido
                // efecto
                break;
        }
    }
}
