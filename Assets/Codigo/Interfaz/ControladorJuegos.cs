﻿// YerkoAndrei
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
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
    [SerializeField] private TMP_Text txtPuntaje;
    [SerializeField] private TMP_Text txtMaxPuntaje;
    [SerializeField] private TMP_Text txtMaxPuntajePausa;

    [Header("Publicidad")]
    [SerializeField] private float tiempoPublicidad;
    [SerializeField] private Image imgContadorPublicidad;
    [SerializeField] private Button btnPublicidad;

    [Header("Paneles")]
    [SerializeField] private GameObject panelJuego;
    [SerializeField] private GameObject panelFinal;
    [SerializeField] private GameObject panelPausa;
    [SerializeField] private Button btnPausa;
    [SerializeField] private Button[] botones;

    [Header("Animaciones")]
    [SerializeField] private RectTransform rectFinal;
    [SerializeField] private RectTransform rectBotonesFinal;
    [SerializeField] private RectTransform rectReintentar;
    [SerializeField] private RectTransform rectPausa;
    [SerializeField] private RectTransform rectBotonesPausa;
    [SerializeField] private RectTransform rectReanudar;

    [Header("Animaciones personaje")]
    [SerializeField] private AnimadorPersonaje personajePausa;
    [SerializeField] private AnimadorPersonaje personajeFinal;

    private bool activo;
    private bool contadorActivo;
    private float contadorTiempo;

    private InterfazJuego interfaz;

    private void Start()
    {
        interfaz = FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None).OfType<InterfazJuego>().FirstOrDefault();

        SistemaMemoria.IniciarPuntaje();
        txtMaxPuntaje.text = SistemaMemoria.ObtenerMaxPuntaje(juego).ToString();
        txtMaxPuntajePausa.text = SistemaMemoria.ObtenerMaxPuntaje(juego).ToString();

        puntaje = SistemaMemoria.ObtenerPuntaje();
        txtPuntaje.text = SistemaMemoria.ObtenerPuntaje().ToString();
        txtPuntaje.color = colorTextoSinGuardar;

        contadorTiempo = tiempoPublicidad;
        btnPublicidad.interactable = true;
        btnPausa.interactable = true;

        panelJuego.SetActive(true);
        panelFinal.SetActive(false);
        panelPausa.SetActive(false);

        rectFinal.gameObject.SetActive(true);
        rectBotonesFinal.gameObject.SetActive(true);
        rectReintentar.gameObject.SetActive(true);
        rectPausa.gameObject.SetActive(true);
        rectBotonesPausa.gameObject.SetActive(true);
        rectReanudar.gameObject.SetActive(true);
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
        contadorActivo = false;

        // Animación nuevo puntaje
        if (SistemaMemoria.AsignarNuevoPuntajeMáximo(juego))
        {
            txtPuntaje.color = Color.white;
            var actualPuntaje =  int.Parse(txtPuntaje.text);
            var actualMaxPuntaje = int.Parse(txtMaxPuntaje.text);

            personajeFinal.AnimarPersonaje(Animaciones.feliz);

            SistemaAnimacion.AnimarNúmeros(txtMaxPuntaje, actualMaxPuntaje, SistemaMemoria.ObtenerMaxPuntaje(juego), null);
            SistemaAnimacion.AnimarNúmeros(txtPuntaje, actualPuntaje, 0, () =>
            {
                txtPuntaje.color = colorTextoSinGuardar;
                personajeFinal.AnimarPersonaje(Animaciones.normal);
            });
        }
        else
            personajeFinal.AnimarPersonaje(Animaciones.normal);
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
        btnPausa.interactable = false;

        panelFinal.SetActive(true);
        ActivarBotones(false);

        personajeFinal.AnimarPersonaje(Animaciones.normal);

        SistemaAnimacion.AnimarPanel(rectFinal, 1, true, true, Direcciones.arriba, null);
        SistemaAnimacion.AnimarPanel(rectBotonesFinal, 1, true, true, Direcciones.izquierda, null);
        SistemaAnimacion.AnimarPanel(rectReintentar, 1, true, true, Direcciones.abajo, () =>
        {
            ActivarBotones(true);
            personajeFinal.AnimarPersonaje(Animaciones.sorprendida);
        });        
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
            personajePausa.AnimarPersonaje(Animaciones.feliz);

            SistemaAnimacion.AnimarPanel(rectBotonesPausa, 1, false, true, Direcciones.izquierda, null);
            SistemaAnimacion.AnimarPanel(rectPausa, 1, false, true, Direcciones.arriba, null);
            SistemaAnimacion.AnimarPanel(rectReanudar, 1, false, true, Direcciones.abajo, () =>
            {
                interfaz.Pausar(false);
                panelPausa.SetActive(false);
                btnPausa.interactable = true;
            });
        }
        else
        {
            interfaz.Pausar(true);
            panelPausa.SetActive(true);
            btnPausa.interactable = false;

            personajePausa.AnimarPersonaje(Animaciones.normal);

            SistemaAnimacion.AnimarPanel(rectBotonesPausa, 1, true, true, Direcciones.izquierda, null);
            SistemaAnimacion.AnimarPanel(rectPausa, 1, true, true, Direcciones.arriba, null);
            SistemaAnimacion.AnimarPanel(rectReanudar, 1, true, true, Direcciones.abajo, () => ActivarBotones(true));
        }
    }

    public void EnClicPublicidad()
    {
        //SistemaPublicidad
        RecompensaPublicidad();
    }

    private void RecompensaPublicidad()
    {
        SistemaAnimacion.AnimarPanel(rectFinal, 1, false, true, Direcciones.arriba, null);
        SistemaAnimacion.AnimarPanel(rectBotonesFinal, 1, false, true, Direcciones.izquierda, null);
        SistemaAnimacion.AnimarPanel(rectReintentar, 1, false, true, Direcciones.abajo, () =>
        {
            panelFinal.SetActive(false);
            btnPausa.interactable = true;

            contadorActivo = false;
            contadorTiempo = tiempoPublicidad;
            btnPublicidad.interactable = true;

            personajePausa.AnimarPersonaje(Animaciones.feliz);
            interfaz.Pausar(false);
        });
    }

    public void EnClicSalir()
    {
        // Guardar puntaje
        if (contadorActivo)
        {
            contadorActivo = false;
            btnPublicidad.interactable = false;
            SistemaMemoria.AsignarNuevoPuntajeMáximo(juego);
        }

        if (panelFinal.activeSelf)
            personajeFinal.AnimarPersonaje(Animaciones.enojada);
        else
            personajePausa.AnimarPersonaje(Animaciones.enojada);
        
        ActivarBotones(false);
        SistemaEscenas.CambiarEscena(Juegos.menu);
    }

    public void EnClicReintentar()
    {
        // Guardar puntaje
        if (contadorActivo)
        {
            contadorActivo = false;
            btnPublicidad.interactable = false;
            SistemaMemoria.AsignarNuevoPuntajeMáximo(juego);
        }

        SistemaMemoria.IniciarPuntaje();
        txtMaxPuntaje.text = SistemaMemoria.ObtenerMaxPuntaje(juego).ToString();
        txtMaxPuntajePausa.text = SistemaMemoria.ObtenerMaxPuntaje(juego).ToString();

        puntaje = SistemaMemoria.ObtenerPuntaje();
        txtPuntaje.text = SistemaMemoria.ObtenerPuntaje().ToString();
        txtPuntaje.color = colorTextoSinGuardar;

        contadorActivo = false;
        contadorTiempo = tiempoPublicidad;
        btnPublicidad.interactable = true;
        ActivarBotones(false);
        interfaz.ReiniciarVisual();

        personajeFinal.AnimarPersonaje(Animaciones.feliz);

        SistemaAnimacion.AnimarPanel(rectFinal, 1, false, true, Direcciones.arriba, null);
        SistemaAnimacion.AnimarPanel(rectBotonesFinal, 1, false, true, Direcciones.arriba, null);
        SistemaAnimacion.AnimarPanel(rectReintentar, 1, false, true, Direcciones.abajo, () =>
        {
            panelFinal.SetActive(false);
            btnPausa.interactable = true;
            activo = true;

            interfaz.Reiniciar();
        });
    }

    public void EnClicClasificar()
    {
        Application.OpenURL("market://details?id=" + Application.identifier);
    }
}
