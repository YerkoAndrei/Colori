// YerkoAndrei
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static Constantes;

public class ControladorJuegos : MonoBehaviour
{
    [Header("Variables")]
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

        // sistema memoria
        txtMaxPuntaje.text = "";
        txtPuntaje.text = puntaje.ToString();
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

        // animar puntaje
    }

    public void SumarPuntaje(int sumar)
    {
        // sistema memoria
        puntaje += sumar;
        txtPuntaje.text = puntaje.ToString();
    }

    public void RestartVida(int restar)
    {
        // sistema memoria
        vidas -= restar;

        if (vidas <= 0)
            Perder();
    }

    public void IniciarVidas(int vidasTotales)
    {
        // sistema memoria
        vidas = vidasTotales;
    }

    public int ObtenerVidas()
    {
        // sistema memoria
        return vidas;
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
        //sistemaPublicidad
    }

    public void EnClicSalir()
    {
        //SistemaEscenas
    }

    public void EnClicReintentar()
    {
        puntaje = 0;
        txtPuntaje.text = puntaje.ToString();
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
