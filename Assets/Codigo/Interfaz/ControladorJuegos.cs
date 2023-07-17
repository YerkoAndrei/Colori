// YerkoAndrei
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static Constantes;

public class ControladorJuegos : MonoBehaviour
{
    [Header("Variables (Solo lectura)")]
    [SerializeField] private Juegos juego;
    [Ocultar] [SerializeField] private int vidas;
    [Ocultar] [SerializeField] private int puntaje;

    [Header("Colores")]
    [SerializeField] private Color colorTextoSinGuardar;

    [Header("Puntaje")]
    [SerializeField] private TMP_Text txtMaxPuntaje;
    [SerializeField] private TMP_Text txtPuntaje;

    [Header("Publicidad")]
    [SerializeField] private float tiempoPublicidad;
    [SerializeField] private Image imgContadorPublicidad;
    [SerializeField] private Button btnPublicidad;

    [Header("Paneles")]
    [SerializeField] private GameObject panelJuego;
    [SerializeField] private GameObject panelPausa;
    [SerializeField] private Button[] botones;

    [Header("Animaciones")]
    [SerializeField] private RectTransform rectFinal;
    [SerializeField] private RectTransform rectReintentar;
    [SerializeField] private RectTransform rectBotonesPausa;
    [SerializeField] private RectTransform rectImagenesPausa;
    [SerializeField] private RectTransform rectReanudar;

    [HideInInspector] public bool activo;

    private bool contadorActivo;
    private float contadorTiempo;

    private InterfazJuego interfaz;

    private void Start()
    {
        interfaz = FindObjectsOfType<MonoBehaviour>().OfType<InterfazJuego>().FirstOrDefault();

        SistemaMemoria.IniciarPuntaje();
        txtMaxPuntaje.text = SistemaMemoria.ObtenerMaxPuntaje(juego).ToString();

        puntaje = SistemaMemoria.ObtenerPuntaje();
        txtPuntaje.text = SistemaMemoria.ObtenerPuntaje().ToString();
        txtPuntaje.color = colorTextoSinGuardar;

        contadorTiempo = tiempoPublicidad;
        btnPublicidad.interactable = true;

        panelJuego.SetActive(true);
        panelPausa.SetActive(false);
        rectFinal.gameObject.SetActive(false);
        rectReintentar.gameObject.SetActive(false);
        rectBotonesPausa.gameObject.SetActive(false);
        rectImagenesPausa.gameObject.SetActive(false);
        rectReanudar.gameObject.SetActive(false);
        activo = true;
    }

    private void Update()
    {
        if (!contadorActivo)
            return;

        contadorTiempo -= Time.deltaTime;
        imgContadorPublicidad.fillAmount = (contadorTiempo / tiempoPublicidad);

        if(contadorTiempo <= 0)
            GuardarPuntaje();
    }

    public void GuardarPuntaje()
    {
        btnPublicidad.interactable = false;
        txtPuntaje.color = Color.white;
        contadorActivo = false;

        if (SistemaMemoria.ObtenerPuntaje() > SistemaMemoria.ObtenerMaxPuntaje(juego))
        {
            SistemaMemoria.AsignarNuevoPuntajeMáximo(juego);
            // animar puntaje
        }
    }

    public void SumarPuntaje(int sumar)
    {
        SistemaMemoria.SumarPuntaje(sumar);
        puntaje = SistemaMemoria.ObtenerPuntaje();
        txtPuntaje.text = SistemaMemoria.ObtenerPuntaje().ToString();
    }

    public void RestartVida(int restar)
    {
        SistemaMemoria.RestarVidas(restar);
        vidas = SistemaMemoria.ObtenerVidas();

        if (SistemaMemoria.ObtenerVidas() <= 0)
            Perder();
    }

    public void IniciarVidas(int vidasTotales)
    {
        SistemaMemoria.IniciarVidas(vidasTotales);
        vidas = SistemaMemoria.ObtenerVidas();
    }

    public int ObtenerVidas()
    {
        return SistemaMemoria.ObtenerVidas();
    }

    public void Perder()
    {
        interfaz.Perder();

        activo = false;
        contadorActivo = true;

        rectFinal.gameObject.SetActive(true);
        rectReintentar.gameObject.SetActive(true);
        ActivarBotones(false);

        SistemaAnimacion.AnimarPanel(rectFinal, 1, true, true, Direcciones.arriba, null);
        SistemaAnimacion.AnimarPanel(rectReintentar, 1, true, true, Direcciones.abajo, () => ActivarBotones(true));
    }

    private void ActivarBotones(bool activar)
    {
        for (int i = 0; i < botones.Length; i++)
        {
            botones[i].interactable = activar;
        }
    }

    public void EnClicPausa()
    {
        activo = !activo;
        ActivarBotones(false);

        if (activo)
        {
            SistemaAnimacion.AnimarPanel(rectBotonesPausa, 1, false, true, Direcciones.izquierda, null);
            SistemaAnimacion.AnimarPanel(rectImagenesPausa, 1, false, true, Direcciones.arriba, null);
            SistemaAnimacion.AnimarPanel(rectReanudar, 1, false, true, Direcciones.abajo, () =>
            {
                interfaz.Pausar(false);
                panelPausa.SetActive(false);
                rectBotonesPausa.gameObject.SetActive(false);
                rectImagenesPausa.gameObject.SetActive(false);
                rectReanudar.gameObject.SetActive(false);
                });
        }
        else
        {
            interfaz.Pausar(true);
            panelPausa.SetActive(true);
            rectBotonesPausa.gameObject.SetActive(true);
            rectImagenesPausa.gameObject.SetActive(true);
            rectReanudar.gameObject.SetActive(true);

            SistemaAnimacion.AnimarPanel(rectBotonesPausa, 1, true, true, Direcciones.izquierda, null);
            SistemaAnimacion.AnimarPanel(rectImagenesPausa, 1, true, true, Direcciones.arriba, null);
            SistemaAnimacion.AnimarPanel(rectReanudar, 1, true, true, Direcciones.abajo, () => ActivarBotones(true));
        }
    }

    public void EnClicPublicidad()
    {
        //SistemaPublicidad
    }

    public void EnClicSalir()
    {
        SistemaEscenas.CambiarEscena(Juegos.menu);
    }

    public void EnClicReintentar()
    {
        SistemaMemoria.IniciarPuntaje();
        txtMaxPuntaje.text = SistemaMemoria.ObtenerMaxPuntaje(juego).ToString();

        puntaje = SistemaMemoria.ObtenerPuntaje();
        txtPuntaje.text = SistemaMemoria.ObtenerPuntaje().ToString();
        interfaz.ReiniciarVisual();

        SistemaAnimacion.AnimarPanel(rectFinal, 1, false, true, Direcciones.arriba, null);
        SistemaAnimacion.AnimarPanel(rectReintentar, 1, false, true, Direcciones.abajo, () =>
        {
            rectFinal.gameObject.SetActive(false);
            rectReintentar.gameObject.SetActive(false);

            interfaz.Reiniciar();
        });
    }

    public void EnClicCompartir()
    {

    }

    public void EnClicClasificar()
    {
        Application.OpenURL("market://details?id=" + Application.identifier);
    }
}
