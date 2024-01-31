// YerkoAndrei
using System;

public class Constantes
{
    public static Random aleatorio;

    public enum Valores
    {
        puntaje,
        vidas
    }

    public enum Juegos
    {
        menu,
        cian,           // color
        amarella,       // color
        magenta         // color
    }

    public enum Animaciones
    {
        normal,
        entrarDerecha,
        entrarIzquierda,
        salirDerecha,
        salirIzquierda,
        feliz,
        enojada,
        sorprendida
    }

    public enum Direcciones
    {
        arriba,
        abajo,
        izquierda,
        derecha
    }

    public enum TipoProyectil
    {
        suave,
        fuerte
    }

    public enum TipoObstáculo
    {
        lento,
        rápido
    }
}
