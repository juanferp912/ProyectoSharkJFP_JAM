using UnityEngine;

public class JellyfishSpawner : MonoBehaviour
{
    [SerializeField] private GameObject medusaPrefab;
    [SerializeField] private Transform jugador;
    [SerializeField] private int cantidadMaxima = 4;
    [SerializeField] private float tiempoEntreGeneraciones = 9f;
    [SerializeField] private float distanciaHorizontalMinima = 8f;
    [SerializeField] private float distanciaHorizontalMaxima = 20f;
    [SerializeField] private float limiteInferiorY = -12f;
    [SerializeField] private float limiteSuperiorY = 4f;

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
        if (jugador == null || medusaPrefab == null)
        {
            return;
        }

        temporizador += Time.deltaTime;

        if (
            temporizador >= tiempoEntreGeneraciones &&
            ContarMedusas() < cantidadMaxima
        )
        {
            GenerarMedusa();
            temporizador = 0f;
        }
    }

    private void GenerarMedusa()
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
            medusaPrefab,
            new Vector3(x, y, 0f),
            Quaternion.identity
        );
    }

    private int ContarMedusas()
    {
        return GameObject.FindGameObjectsWithTag("Jellyfish").Length;
    }
}