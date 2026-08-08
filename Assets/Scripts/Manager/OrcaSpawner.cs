using UnityEngine;

public class OrcaSpawner : MonoBehaviour
{
    [SerializeField] private GameObject orcaPrefab;

    [SerializeField] private int maximoOrcas = 1;
    [SerializeField] private float intervaloSpawn = 18f;

    [SerializeField] private float distanciaMinimaX = 14f;
    [SerializeField] private float distanciaMaximaX = 28f;

    [SerializeField] private float alturaMinima = -10f;
    [SerializeField] private float alturaMaxima = 5f;

    private Transform jugador;

    private void Start()
    {
        GameObject player =
            GameObject.FindGameObjectWithTag("Player");

        if (player != null)
        {
            jugador = player.transform;
        }

        InvokeRepeating(
            nameof(IntentarSpawn),
            8f,
            intervaloSpawn
        );
    }

    private void IntentarSpawn()
    {
        if (
            jugador == null ||
            orcaPrefab == null
        )
        {
            return;
        }

        int cantidad =
            GameObject.FindGameObjectsWithTag(
                "Orca"
            ).Length;

        if (cantidad >= maximoOrcas)
        {
            return;
        }

        float lado =
            Random.value < 0.5f
                ? -1f
                : 1f;

        float distancia =
            Random.Range(
                distanciaMinimaX,
                distanciaMaximaX
            );

        float x =
            jugador.position.x +
            distancia *
            lado;

        float y =
            Random.Range(
                alturaMinima,
                alturaMaxima
            );

        Instantiate(
            orcaPrefab,
            new Vector3(
                x,
                y,
                0f
            ),
            Quaternion.identity
        );
    }
}