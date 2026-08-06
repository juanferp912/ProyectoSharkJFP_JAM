using UnityEngine;

public class SpawnerManager : MonoBehaviour
{
    [SerializeField] private GameObject[] pecesPrefabs;
    [SerializeField] private Transform jugador;
    [SerializeField] private int cantidadInicial = 12;
    [SerializeField] private int cantidadMaxima = 20;
    [SerializeField] private float tiempoEntreGeneraciones = 2f;
    [SerializeField] private float distanciaHorizontal = 20f;
    [SerializeField] private float distanciaMinimaJugador = 6f;
    [SerializeField] private float limiteInferiorY = -14f;
    [SerializeField] private float limiteSuperiorY = 6.5f;

    private float temporizador;

    private void Start()
    {
        if (jugador == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");

            if (player != null)
            {
                jugador = player.transform;
            }
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

        Vector3 posicion = ObtenerPosicion();

        GameObject prefab = pecesPrefabs[
            Random.Range(0, pecesPrefabs.Length)
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
            float x = Random.Range(
                jugador.position.x - distanciaHorizontal,
                jugador.position.x + distanciaHorizontal
            );

            float y = Random.Range(
                limiteInferiorY,
                limiteSuperiorY
            );

            Vector3 posicion = new Vector3(x, y, 0f);

            if (
                Vector2.Distance(posicion, jugador.position) >=
                distanciaMinimaJugador
            )
            {
                return posicion;
            }
        }

        return new Vector3(
            jugador.position.x + distanciaMinimaJugador,
            0f,
            0f
        );
    }

    private int ContarPeces()
    {
        return GameObject.FindGameObjectsWithTag("Fish").Length;
    }
}