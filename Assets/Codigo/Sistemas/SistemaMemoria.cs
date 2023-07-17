using System.Text;
using UnityEngine;
using static Constantes;

public class SistemaMemoria
{
    protected static int llave = 08021996;

    // XOR
    private static string DesEncriptar(string texto)
    {
        var entrada = new StringBuilder(texto);
        var salida = new StringBuilder(texto.Length);
        char c;

        for (int i = 0; i < texto.Length; i++)
        {
            c = entrada[i];
            c = (char)(c ^ llave);
            salida.Append(c);
        }
        return salida.ToString();
    }

    private static int DesencriptarValor(string valor)
    {
        // Primera encriptación
        if (string.IsNullOrEmpty(PlayerPrefs.GetString(valor)))
            PlayerPrefs.SetString(valor, DesEncriptar("0"));

        var valorDesencriptado = DesEncriptar(PlayerPrefs.GetString(valor));
        return int.Parse(valorDesencriptado);
    }

    public static void SumarPuntaje(int sumar)
    {
        var tempPuntaje = DesencriptarValor(Valores.puntaje.ToString());
        tempPuntaje += sumar;
        PlayerPrefs.SetString(Valores.puntaje.ToString(), DesEncriptar(tempPuntaje.ToString()));
    }

    public static void RestarVidas(int restar)
    {
        var tempVidas = DesencriptarValor(Valores.vidas.ToString());
        tempVidas -= restar;
        PlayerPrefs.SetString(Valores.vidas.ToString(), DesEncriptar(tempVidas.ToString()));
    }

    public static void IniciarPuntaje()
    {
        PlayerPrefs.SetString(Valores.puntaje.ToString(), DesEncriptar("0"));
    }

    public static void IniciarVidas(int vidasTotales)
    {
        PlayerPrefs.SetString(Valores.vidas.ToString(), DesEncriptar(vidasTotales.ToString()));
    }

    public static bool AsignarNuevoPuntajeMáximo(Juegos juego)
    {
        if (ObtenerPuntaje() > ObtenerMaxPuntaje(juego))
        {
            PlayerPrefs.SetString(juego.ToString(), DesEncriptar(ObtenerPuntaje().ToString()));
            return true;
        }
        else
            return false;
    }

    public static int ObtenerPuntaje()
    {
        return DesencriptarValor(Valores.puntaje.ToString());
    }

    public static int ObtenerVidas()
    {
        return DesencriptarValor(Valores.vidas.ToString());
    }

    public static int ObtenerMaxPuntaje(Juegos juego)
    {
        return DesencriptarValor(juego.ToString());
    }
}
