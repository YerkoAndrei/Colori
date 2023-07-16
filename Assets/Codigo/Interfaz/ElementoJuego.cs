using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ElementoJuego : MonoBehaviour
{
    public RectTransform rectTransform;
    [SerializeField] private Button btnJugar;
    [SerializeField] private TMP_Text txtPuntaje;

    public void Iniciar(int puntaje)
    {
        txtPuntaje.text = puntaje.ToString();
    }

    public void ActivarBotón(bool activar)
    {
        btnJugar.interactable = activar;
    }
}
