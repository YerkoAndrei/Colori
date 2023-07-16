using UnityEngine;

public class ColisionadorEspada : MonoBehaviour
{
    private ControladorCian controlador;

    private void Start()
    {
        controlador = FindObjectOfType<ControladorCian>();
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        controlador.SumarPuntos(collider.GetComponent<Proyectil>().tipoProyectil);
        Destroy(collider.gameObject);
    }
}
