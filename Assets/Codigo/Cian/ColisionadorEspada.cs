using UnityEngine;

public class ColisionadorEspada : MonoBehaviour
{
    private ControladorCian controlador;

    private void Start()
    {
        controlador = FindFirstObjectByType<ControladorCian>();
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        controlador.SumarPuntos(collider.GetComponent<ProyectilCian>().tipoProyectil, collider.transform.position);
        Destroy(collider.gameObject);
    }
}
