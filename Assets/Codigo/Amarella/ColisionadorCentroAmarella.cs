using UnityEngine;

public class ColisionadorCentroAmarella : MonoBehaviour
{
    private ControladorAmarella controlador;

    private void Start()
    {
        controlador = FindFirstObjectByType<ControladorAmarella>();
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        controlador.RestarVidas(collider.transform.parent.GetComponent<ObstaculoAmarella>().tipoObstáculo);
        //Destroy(collider.gameObject);
    }
}
