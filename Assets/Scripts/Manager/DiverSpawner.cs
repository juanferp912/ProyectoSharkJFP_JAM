using UnityEngine;

public class DiverSpawner : MonoBehaviour
{
    [SerializeField] private GameObject diverPrefab;
    [SerializeField] private Transform jugador;
    [SerializeField] private int cantidadMaxima = 3;
    [SerializeField] private float tiempoEntreGeneraciones = 10f;
    [SerializeField] private float distanciaHorizontalMinima = 12f;
    [SerializeField] private float distanciaHorizontalMaxima = 24f;
    [SerializeField] private float limiteInferiorY = -10f;
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
        if (jugador == null || diverPrefab == null)
        {
            return;
        }

        temporizador += Time.deltaTime;

        if (
            temporizador >= tiempoEntreGeneraciones &&
            ContarBuzos() < cantidadMaxima
        )
        {
            GenerarBuzo();
            temporizador = 0f;
        }
    }

    private void GenerarBuzo()
    {
        float lado = Random.value < 0.5f ? -1f : 1f;

        float x =
            jugador.position.x +
            Random.Range(
                distanciaHorizontalMinima,
                distanciaHorizontalMaxima
            ) * lado;

        float y = Random.Range(
            limiteInferiorY,
            limiteSuperiorY
        );

        Instantiate(
            diverPrefab,
            new Vector3(x, y, 0f),
            Quaternion.identity
        );
    }

    private int ContarBuzos()
    {
        return GameObject.FindGameObjectsWithTag("Diver").Length;
    }
}