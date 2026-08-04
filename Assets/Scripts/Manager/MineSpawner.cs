using UnityEngine;

public class MineSpawner : MonoBehaviour
{
    [SerializeField] private GameObject minaPrefab;

    [Header("Cantidad")]
    [SerializeField] private int cantidadInicial = 3;
    [SerializeField] private int cantidadMaxima = 5;

    [Header("Generación")]
    [SerializeField] private float tiempoEntreGeneraciones = 8f;
    [SerializeField] private Vector2 limiteMinimo = new Vector2(-20f, -10f);
    [SerializeField] private Vector2 limiteMaximo = new Vector2(20f, 10f);

    [Header("Distancia del jugador")]
    [SerializeField] private Transform jugador;
    [SerializeField] private float distanciaMinimaJugador = 5f;

    private float contador;

    private void Start()
    {
        contador = tiempoEntreGeneraciones;

        for (int i = 0; i < cantidadInicial; i++)
        {
            GenerarMina();
        }
    }

    private void Update()
    {
        contador -= Time.deltaTime;

        if (contador > 0f)
        {
            return;
        }

        int minasActuales = GameObject.FindGameObjectsWithTag("Mine").Length;

        if (minasActuales < cantidadMaxima)
        {
            GenerarMina();
        }

        contador = tiempoEntreGeneraciones;
    }

    private void GenerarMina()
    {
        if (minaPrefab == null)
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

        Instantiate(minaPrefab, posicion, Quaternion.identity);
    }
}