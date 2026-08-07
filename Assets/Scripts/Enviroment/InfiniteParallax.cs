using UnityEngine;

public class InfiniteParallax : MonoBehaviour
{
    [SerializeField] private Transform jugador;
    [SerializeField] private SpriteRenderer cieloA;
    [SerializeField] private SpriteRenderer cieloB;
    [SerializeField] private SpriteRenderer cieloC;

    [Header("Parallax")]
    [SerializeField] private float factorParallax = 0.2f;
    [SerializeField] private float alturaCielo = -1f;
    [SerializeField] private float solapamiento = 0.05f;

    [Header("Movimiento automatico")]
    [SerializeField] private float velocidadMovimiento = 0.5f;

    private float ancho;
    private float distanciaEntreCielos;

    private float jugadorXInicial;
    private float centroInicial;
    private float desplazamientoAutomatico;

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

        if (
            jugador == null ||
            cieloA == null ||
            cieloB == null ||
            cieloC == null
        )
        {
            enabled = false;
            return;
        }

        ancho =
            cieloA.sprite.bounds.size.x *
            Mathf.Abs(cieloA.transform.lossyScale.x);

        distanciaEntreCielos =
            ancho - solapamiento;

        jugadorXInicial =
            jugador.position.x;

        centroInicial =
            jugador.position.x;

        ActualizarCielos();
    }

    private void LateUpdate()
    {
        desplazamientoAutomatico +=
            velocidadMovimiento *
            Time.deltaTime;

        ActualizarCielos();
    }

    private void ActualizarCielos()
    {
        float desplazamientoJugador =
            jugador.position.x -
            jugadorXInicial;

        float centroParallax =
            centroInicial +
            desplazamientoJugador *
            factorParallax +
            desplazamientoAutomatico;

        float diferencia =
            jugador.position.x -
            centroParallax;

        int desplazamientoBloques =
            Mathf.RoundToInt(
                diferencia /
                distanciaEntreCielos
            );

        float centroCorregido =
            centroParallax +
            desplazamientoBloques *
            distanciaEntreCielos;

        Posicionar(
            cieloA,
            centroCorregido -
            distanciaEntreCielos
        );

        Posicionar(
            cieloB,
            centroCorregido
        );

        Posicionar(
            cieloC,
            centroCorregido +
            distanciaEntreCielos
        );
    }

    private void Posicionar(
        SpriteRenderer cielo,
        float posicionX
    )
    {
        cielo.transform.position =
            new Vector3(
                posicionX,
                alturaCielo,
                cielo.transform.position.z
            );
    }
}