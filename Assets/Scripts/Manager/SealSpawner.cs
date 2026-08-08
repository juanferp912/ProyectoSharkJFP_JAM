using UnityEngine;

public class SealSpawner : MonoBehaviour
{
    [SerializeField] private GameObject sealPrefab;

    [SerializeField] private int maximoFocas = 4;
    [SerializeField] private float intervaloSpawn = 9f;

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
            4f,
            intervaloSpawn
        );
    }

    private void IntentarSpawn()
    {
        if (
            jugador == null ||
            sealPrefab == null
        )
        {
            return;
        }

        int cantidad =
            GameObject.FindGameObjectsWithTag(
                "Seal"
            ).Length;

        if (cantidad >= maximoFocas)
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
            sealPrefab,
            new Vector3(
                x,
                y,
                0f
            ),
            Quaternion.identity
        );
    }
}