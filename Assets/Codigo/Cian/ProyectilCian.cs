﻿using UnityEngine;
using static Constantes;

public class ProyectilCian : MonoBehaviour
{
    [Header("Variables")]
    [Ocultar] public TipoProyectil tipoProyectil;
    [SerializeField] private float velocidadSuave;
    [SerializeField] private float velocidadFuerte;

    [Header("Referencias")]
    [SerializeField] private SpriteRenderer imagen;

    [Header("Colores")]
    [SerializeField] private Color colorSuave;
    [SerializeField] private Color colorFuerte;

    private ControladorCian controlador;
    private float velocidad;

    private void Start()
    {
        controlador = FindFirstObjectByType<ControladorCian>();

        float aleatorio = Random.Range(0, 10);
        if (aleatorio < 8)
        {
            tipoProyectil = TipoProyectil.suave;
            velocidad = velocidadSuave;
            imagen.color = colorSuave;
            imagen.sortingOrder = 1;
        }
        else
        {
            tipoProyectil = TipoProyectil.fuerte;
            velocidad = velocidadFuerte;
            imagen.color = colorFuerte;
            imagen.sortingOrder = 2;
        }
    }

    private void FixedUpdate()
    {
        if (!controlador.activo)
            return;

        transform.position = Vector2.MoveTowards(transform.position, Vector2.zero, velocidad * Time.fixedDeltaTime);
    }
}
