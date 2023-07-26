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
        controlador.RestarVidas(collider.GetComponent<ProyectilCian>().tipoProyectil, collider.transform.position);
        Destroy(collider.gameObject);
    }
}
