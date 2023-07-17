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

    [Header("Referencias")]
    [SerializeField] private RectTransform panelFinal;
    [SerializeField] private RectTransform panelReintentar;
    [SerializeField] private Button[] botones;

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

        panelFinal.gameObject.SetActive(false);
        panelReintentar.gameObject.SetActive(false);
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

        panelFinal.gameObject.SetActive(true);
        panelReintentar.gameObject.SetActive(true);
        ActivarBotones(false);

        SistemaAnimacion.AnimarPanel(panelFinal, 1, true, true, Direcciones.arriba, null);
        SistemaAnimacion.AnimarPanel(panelReintentar, 1, true, true, Direcciones.abajo, () => ActivarBotones(true));
    }

    private void ActivarBotones(bool activar)
    {
        for (int i = 0; i < botones.Length; i++)
        {
            botones[i].interactable = activar;
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

        SistemaAnimacion.AnimarPanel(panelFinal, 1, false, true, Direcciones.arriba, null);
        SistemaAnimacion.AnimarPanel(panelReintentar, 1, false, true, Direcciones.abajo, () =>
        {
            panelFinal.gameObject.SetActive(false);
            panelReintentar.gameObject.SetActive(false);

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
