using UnityEngine;

public class SpawnerManager : MonoBehaviour
{
    [SerializeField] private GameObject[] pecesPrefabs;
    [SerializeField] private Transform jugador;

    [Header("Cantidad")]
    [SerializeField] private int cantidadInicial = 12;
    [SerializeField] private int cantidadMaxima = 20;
    [SerializeField] private float tiempoEntreGeneraciones = 2f;

    [Header("Generacion")]
    [SerializeField] private float distanciaHorizontalMinima = 8f;
    [SerializeField] private float distanciaHorizontalMaxima = 22f;
    [SerializeField] private float distanciaMinimaJugador = 6f;
    [SerializeField] private float limiteInferiorY = -14f;
    [SerializeField] private float limiteSuperiorY = 6.5f;

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

        if (jugador == null)
        {
            return;
        }

        for (int i = 0; i < cantidadInicial; i++)
        {
            GenerarPez();
        }
    }

    private void Update()
    {
        if (jugador == null)
        {
            return;
        }

        temporizador += Time.deltaTime;

        if (
            temporizador >= tiempoEntreGeneraciones &&
            ContarPeces() < cantidadMaxima
        )
        {
            GenerarPez();
            temporizador = 0f;
        }
    }

    private void GenerarPez()
    {
        if (
            pecesPrefabs == null ||
            pecesPrefabs.Length == 0 ||
            jugador == null
        )
        {
            return;
        }

        Vector3 posicion =
            ObtenerPosicion();

        GameObject prefab =
            pecesPrefabs[
                Random.Range(
                    0,
                    pecesPrefabs.Length
                )
            ];

        Instantiate(
            prefab,
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

            float distanciaX =
                Random.Range(
                    distanciaHorizontalMinima,
                    distanciaHorizontalMaxima
                );

            float x =
                jugador.position.x +
                distanciaX * lado;

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

    private int ContarPeces()
    {
        return GameObject.FindGameObjectsWithTag("Fish").Length;
    }
}