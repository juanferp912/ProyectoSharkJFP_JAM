using UnityEngine;

public class DiverSpawner : MonoBehaviour
{
    [SerializeField] private GameObject buzoPrefab;

    [Header("Cantidad")]
    [SerializeField] private int cantidadInicial = 2;
    [SerializeField] private int cantidadMaxima = 3;

    [Header("Generación")]
    [SerializeField] private float tiempoEntreGeneraciones = 12f;
    [SerializeField] private Vector2 limiteMinimo = new Vector2(-20f, -10f);
    [SerializeField] private Vector2 limiteMaximo = new Vector2(20f, 10f);

    [Header("Jugador")]
    [SerializeField] private Transform jugador;
    [SerializeField] private float distanciaMinimaJugador = 8f;

    private float contador;

    private void Start()
    {
        contador = tiempoEntreGeneraciones;

        for (int i = 0; i < cantidadInicial; i++)
        {
            GenerarBuzo();
        }
    }

    private void Update()
    {
        contador -= Time.deltaTime;

        if (contador > 0f)
        {
            return;
        }

        int buzosActuales = GameObject.FindGameObjectsWithTag("Diver").Length;

        if (buzosActuales < cantidadMaxima)
        {
            GenerarBuzo();
        }

        contador = tiempoEntreGeneraciones;
    }

    private void GenerarBuzo()
    {
        if (buzoPrefab == null)
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
            intentos < 20
        );

        Instantiate(buzoPrefab, posicion, Quaternion.identity);
    }
}