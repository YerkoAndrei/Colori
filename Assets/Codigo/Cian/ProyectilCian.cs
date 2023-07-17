using UnityEngine;
using static Constantes;

public class ProyectilCian : MonoBehaviour
{
    [Header("Variables")]
    [Ocultar] public TipoProyectil tipoProyectil;

    [Header("Referencias")]
    [SerializeField] private SpriteRenderer imagen;

    [Header("Colores")]
    [SerializeField] private Color colorSuave;
    [SerializeField] private Color colorFuerte;

    private ControladorCian controlador;
    private float velocidad;

    private void Start()
    {
        controlador = FindObjectOfType<ControladorCian>();

        float aleatorio = Random.Range(0, 10);
        if (aleatorio < 7)
        {
            tipoProyectil = TipoProyectil.suave;
            velocidad = 2f;
            imagen.color = colorSuave;
            imagen.sortingOrder = 0;
        }
        else
        {
            tipoProyectil = TipoProyectil.fuerte;
            velocidad = 4f;
            imagen.color = colorFuerte;
            imagen.sortingOrder = 1;
        }
    }

    private void FixedUpdate()
    {
        if (!controlador.activo)
            return;

        transform.position = Vector2.MoveTowards(transform.position, Vector2.zero, velocidad * Time.fixedDeltaTime);
    }
}
