using UnityEngine;
using UnityEngine.UI;
using TMPro;
using static Constantes;

public class ElementoJuego : MonoBehaviour
{
    public Juegos juego;
    public RectTransform rectTransform;
    [SerializeField] private Button btnJugar;
    [SerializeField] private TMP_Text txtPuntaje;

    private ControladorMenu controlador;

    public void Start()
    {
        controlador = FindObjectOfType<ControladorMenu>();
        txtPuntaje.text = SistemaMemoria.ObtenerMaxPuntaje(juego).ToString();
    }

    public void ActivarBotón(bool activar)
    {
        btnJugar.interactable = activar;
    }

    public void EnClicJugar()
    {
        ActivarBotón(false);
        controlador.EnClicJuego(juego);
    }
}
