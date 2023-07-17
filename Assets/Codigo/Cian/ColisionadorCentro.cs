using UnityEngine;

public class ColisionadorCentro : MonoBehaviour
{
    private ControladorCian controlador;

    private void Start()
    {
        controlador = FindObjectOfType<ControladorCian>();
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        controlador.RestarVidas(collider.GetComponent<Proyectil>().tipoProyectil);
        Destroy(collider.gameObject);
    }
}
