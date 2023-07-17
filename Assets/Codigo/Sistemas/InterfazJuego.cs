public interface InterfazJuego
{
    // Externo
    void Pausar(bool pausa);
    void Perder();
    void Reiniciar();
    void ReiniciarVisual();

    // Interno
    void SumarPuntos(int sumar);
    void RestarVidas(int restar);
}
