using UnityEngine;

public class MineSpawner : MonoBehaviour
{
    [SerializeField] private GameObject minaPrefab;
    [SerializeField] private Transform jugador;

    [Header("Cantidad")]
    [SerializeField] private int cantidadMaxima = 5;
    [SerializeField] private float tiempoEntreGeneraciones = 7f;

    [Header("Generacion")]
    [SerializeField] private float distanciaHorizontalMinima = 8f;
    [SerializeField] private float distanciaHorizontalMaxima = 22f;
    [SerializeField] private float distanciaMinimaJugador = 7f;
    [SerializeField] private float limiteInferiorY = -12f;
    [SerializeField] private float limiteSuperiorY = 5f;

    private float temporizador;

    private void Start()
    {
        if (jugador == null)
        {
            GameObject player =
                GameObject.FindGameObjectWithTag("Player");

            if (player != null)
            {
                jugador = player.transform;
            }
        }
    }

    private void Update()
    {
        if (jugador == null || minaPrefab == null)
        {
            return;
        }

        temporizador += Time.deltaTime;

        if (
            temporizador >= tiempoEntreGeneraciones &&
            ContarMinas() < cantidadMaxima
        )
        {
            GenerarMina();
            temporizador = 0f;
        }
    }

    private void GenerarMina()
    {
        Vector3 posicion =
            ObtenerPosicion();

        Instantiate(
            minaPrefab,
            posicion,
            Quaternion.identity
        );
    }

    private Vector3 ObtenerPosicion()
    {
        for (int i = 0; i < 20; i++)
        {
            float lado =
                Random.value < 0.5f
                ? -1f
                : 1f;

            float distancia =
                Random.Range(
                    distanciaHorizontalMinima,
                    distanciaHorizontalMaxima
                );

            float x =
                jugador.position.x +
                distancia * lado;

            float y =
                Random.Range(
                    limiteInferiorY,
                    limiteSuperiorY
                );

            Vector3 posicion =
                new Vector3(
                    x,
                    y,
                    0f
                );

            if (
                Vector2.Distance(
                    posicion,
                    jugador.position
                ) >= distanciaMinimaJugador
            )
            {
                return posicion;
            }
        }

        float ladoFallback =
            Random.value < 0.5f
            ? -1f
            : 1f;

        return new Vector3(
            jugador.position.x +
            distanciaHorizontalMinima *
            ladoFallback,
            Random.Range(
                limiteInferiorY,
                limiteSuperiorY
            ),
            0f
        );
    }

    private int ContarMinas()
    {
        return GameObject.FindGameObjectsWithTag("Mine").Length;
    }
}