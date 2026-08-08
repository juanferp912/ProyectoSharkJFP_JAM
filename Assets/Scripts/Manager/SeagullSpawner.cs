using UnityEngine;

public class SeagullSpawner : MonoBehaviour
{
    [SerializeField] private GameObject seagullPrefab;

    [SerializeField] private int maximoGaviotas = 5;

    [SerializeField] private float intervaloSpawn = 7f;

    [SerializeField] private float distanciaMinimaX = 10f;
    [SerializeField] private float distanciaMaximaX = 20f;

    [SerializeField] private float alturaMinima = 9f;
    [SerializeField] private float alturaMaxima = 11f;

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
            seagullPrefab == null
        )
        {
            return;
        }

        int cantidad =
            GameObject.FindGameObjectsWithTag(
                "Seagull"
            ).Length;

        if (cantidad >= maximoGaviotas)
        {
            return;
        }

        float lado =
            Random.value < 0.5f
                ? -1f
                : 1f;

        float distancia = Random.Range(
            distanciaMinimaX,
            distanciaMaximaX
        );

        float x =
            jugador.position.x +
            distancia *
            lado;

        float y = Random.Range(
            alturaMinima,
            alturaMaxima
        );

        Instantiate(
            seagullPrefab,
            new Vector3(
                x,
                y,
                0f
            ),
            Quaternion.identity
        );
    }
}