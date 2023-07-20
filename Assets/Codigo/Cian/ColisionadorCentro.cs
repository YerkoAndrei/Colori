using UnityEngine;

public class ColisionadorCentro : MonoBehaviour
{
    private ControladorCian controlador;

    private void Start()
    {
        controlador = FindFirstObjectByType<ControladorCian>();
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        controlador.RestarVidas(collider.GetComponent<ProyectilCian>().tipoProyectil);
        Destroy(collider.gameObject);
    }
}
