using UnityEngine;
using UnityEngine.UI;
using TMPro;
using static Constantes;

public class ElementoJuego : MonoBehaviour
{
    [Header("Referencias")]
    public Juegos juego;
    public RectTransform rectTransform;
    [SerializeField] private Button btnJugar;
    [SerializeField] private TMP_Text txtPuntaje;

    [Header("Personaje")]
    [SerializeField] private AnimadorPersonaje animadorPersonaje;

    private ControladorMenu controlador;

    public void Start()
    {
        controlador = FindFirstObjectByType<ControladorMenu>();
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

        animadorPersonaje.AnimarPersonaje(Animaciones.feliz);
    }

    public void AnimarPersonaje(Animaciones animacion)
    {
        animadorPersonaje.AnimarPersonaje(animacion);
    }
}
