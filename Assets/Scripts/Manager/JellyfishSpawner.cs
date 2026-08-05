using System.Collections.Generic;
using UnityEngine;

public class JellyfishSpawner : MonoBehaviour
{
    [SerializeField] private GameObject medusaPrefab;

    [Header("Jugador")]
    [SerializeField] private Transform jugador;
    [SerializeField] private float distanciaMinimaJugador = 7f;

    [Header("Cantidad")]
    [SerializeField] private int cantidadInicial = 3;
    [SerializeField] private int cantidadMaxima = 5;

    [Header("Generación")]
    [SerializeField] private float tiempoEntreGeneraciones = 10f;
    [SerializeField] private Vector2 limiteMinimo = new Vector2(-25f, -12f);
    [SerializeField] private Vector2 limiteMaximo = new Vector2(25f, 12f);

    private readonly List<GameObject> medusasGeneradas = new List<GameObject>();
    private float contador;

    private void Start()
    {
        contador = tiempoEntreGeneraciones;

        for (int i = 0; i < cantidadInicial; i++)
        {
            GenerarMedusa();
        }
    }

    private void Update()
    {
        medusasGeneradas.RemoveAll(medusa => medusa == null);

        contador -= Time.deltaTime;

        if (contador > 0f)
        {
            return;
        }

        if (medusasGeneradas.Count < cantidadMaxima)
        {
            GenerarMedusa();
        }

        contador = tiempoEntreGeneraciones;
    }

    private void GenerarMedusa()
    {
        if (medusaPrefab == null)
        {
            return;
        }

        Vector2 posicion;
        int intentos = 0;

        do
        {
            posicion = new Vector2(
                Random.Range(limiteMinimo.x, limiteMaximo.x),
                Random.Range(limiteMinimo.y, limiteMaximo.y)
            );

            intentos++;
        }
        while (
            jugador != null &&
            Vector2.Distance(posicion, jugador.position) < distanciaMinimaJugador &&
            intentos < 25
        );

        GameObject medusa = Instantiate(
            medusaPrefab,
            posicion,
            Quaternion.identity
        );

        medusasGeneradas.Add(medusa);
    }
}