using UnityEngine;

public class GarbageController : MonoBehaviour
{
    [SerializeField] private float distanciaMaximaX = 45f;
    [SerializeField] private float distanciaMaximaY = 30f;
    [SerializeField] private float intervaloRevision = 2f;

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
            nameof(RevisarDistancia),
            intervaloRevision,
            intervaloRevision
        );
    }

    private void RevisarDistancia()
    {
        if (jugador == null)
        {
            return;
        }

        float distanciaX =
            Mathf.Abs(transform.position.x - jugador.position.x);

        float distanciaY =
            Mathf.Abs(transform.position.y - jugador.position.y);

        if (
            distanciaX > distanciaMaximaX ||
            distanciaY > distanciaMaximaY
        )
        {
            Destroy(gameObject);
        }
    }
}