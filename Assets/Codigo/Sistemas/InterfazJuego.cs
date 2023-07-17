public interface InterfazJuego
{
    // Externo
    void Perder();
    void Reiniciar();
    void ReiniciarVisual();

    // Interno
    void SumarPuntos(int sumar);
    void RestarVidas(int restar);
}
