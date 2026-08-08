using UnityEngine;

public class DolphinSpawner : MonoBehaviour
{
    [SerializeField] private GameObject dolphinPrefab;

    [SerializeField] private int maximoDelfines = 4;
    [SerializeField] private float intervaloSpawn = 8f;

    [SerializeField] private float distanciaMinimaX = 10f;
    [SerializeField] private float distanciaMaximaX = 22f;

    [SerializeField] private float alturaMinima = -10f;
    [SerializeField] private float alturaMaxima = 6f;

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
            3f,
            intervaloSpawn
        );
    }

    private void IntentarSpawn()
    {
        if (
            jugador == null ||
            dolphinPrefab == null
        )
        {
            return;
        }

        int cantidad =
            GameObject.FindGameObjectsWithTag(
                "Dolphin"
            ).Length;

        if (cantidad >= maximoDelfines)
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
            dolphinPrefab,
            new Vector3(
                x,
                y,
                0f
            ),
            Quaternion.identity
        );
    }
}