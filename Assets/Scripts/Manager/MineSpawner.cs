using UnityEngine;

public class MineSpawner : MonoBehaviour
{
    [SerializeField] private GameObject minaPrefab;
    [SerializeField] private Transform jugador;
    [SerializeField] private int cantidadMaxima = 5;
    [SerializeField] private float tiempoEntreGeneraciones = 7f;
    [SerializeField] private float distanciaHorizontalMinima = 8f;
    [SerializeField] private float distanciaHorizontalMaxima = 22f;
    [SerializeField] private float limiteInferiorY = -12f;
    [SerializeField] private float limiteSuperiorY = 5f;

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
        float lado = Random.value < 0.5f ? -1f : 1f;

        float distancia = Random.Range(
            distanciaHorizontalMinima,
            distanciaHorizontalMaxima
        );

        float x =
            jugador.position.x +
            distancia * lado;

        float y = Random.Range(
            limiteInferiorY,
            limiteSuperiorY
        );

        Instantiate(
            minaPrefab,
            new Vector3(x, y, 0f),
            Quaternion.identity
        );
    }

    private int ContarMinas()
    {
        return GameObject.FindGameObjectsWithTag("Mine").Length;
    }
}