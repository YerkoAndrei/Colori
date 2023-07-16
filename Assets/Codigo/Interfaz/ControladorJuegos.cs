// YerkoAndrei
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static Constantes;

public class ControladorJuegos : MonoBehaviour
{
    [Header("Puntaje")]
    [SerializeField] private int puntaje;

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

    private void Start()
    {
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
        {
            btnPublicidad.interactable = false;
            contadorActivo = false;
        }
    }

    public void AgregarPuntaje(int agregar)
    {
        // sistema memoria
        puntaje += agregar;
    }

    public void Perder()
    {
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
        //SistemaEscenas
    }

    public void EnClicCompartir()
    {

    }

    public void EnClicClasificar()
    {
        Application.OpenURL("market://details?id=" + Application.identifier);
    }
}
