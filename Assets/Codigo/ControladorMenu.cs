// YerkoAndrei
using UnityEngine;
using UnityEngine.UI;
using static Constantes;

public class ControladorMenu : MonoBehaviour
{
    [Header("Juegos")]
    [SerializeField] private ElementoJuego[] juegos;
    [SerializeField] private Button[] botonesFlujo;

    private int juegoActual;
    private float tiempoAnimación = 0.8f;

    private void Start()
    {
        
    }

    public void EnClicSiguiente()
    {
        juegos[juegoActual].ActivarBotón(false);
        juegos[ObtenerJuegoSiguiente()].ActivarBotón(false);
        ActivarBotones(false);

        SistemaAnimacion.AnimarPanel(juegos[juegoActual].rectTransform, tiempoAnimación, false, false, Direcciones.izquierda, null);
        SistemaAnimacion.AnimarPanel(juegos[ObtenerJuegoSiguiente()].rectTransform, tiempoAnimación, true, true, Direcciones.derecha,
            () =>
            {
                juegos[juegoActual].ActivarBotón(true);
                ActivarBotones(true);
            });

        juegoActual = ObtenerJuegoSiguiente();
    }

    public void EnClicAnterior()
    {
        juegos[juegoActual].ActivarBotón(false);
        juegos[ObtenerJuegoAnterior()].ActivarBotón(false);
        ActivarBotones(false);

        SistemaAnimacion.AnimarPanel(juegos[juegoActual].rectTransform, tiempoAnimación, false, false, Direcciones.derecha, null);
        SistemaAnimacion.AnimarPanel(juegos[ObtenerJuegoAnterior()].rectTransform, tiempoAnimación, true, true, Direcciones.izquierda,
            () =>
            {
                juegos[juegoActual].ActivarBotón(true);
                ActivarBotones(true);
            });

        juegoActual = ObtenerJuegoAnterior();
    }

    private void ActivarBotones(bool activar)
    {
        for(int i=0; i< botonesFlujo.Length; i++)
        {
            botonesFlujo[i].interactable = activar;
        }
    }

    private int ObtenerJuegoSiguiente()
    {
        var juego = juegoActual + 1;

        if (juego >= juegos.Length)
            return 0;
        else 
            return juego;
    }

    private int ObtenerJuegoAnterior()
    {
        var juego = juegoActual - 1;

        if (juego < 0)
            return (juegos.Length - 1);
        else
            return juego;
    }
}
