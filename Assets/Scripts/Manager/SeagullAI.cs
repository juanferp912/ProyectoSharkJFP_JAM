using UnityEngine;

public class SeagullAI : MonoBehaviour
{
    [Header("Sprites")]
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Sprite spriteReposo;
    [SerializeField] private Sprite[] spritesVuelo;

    [Header("Movimiento")]
    [SerializeField] private float velocidadVuelo = 2.2f;
    [SerializeField] private float velocidadEscape = 4.5f;
    [SerializeField] private float alturaMinima = 8.8f;
    [SerializeField] private float alturaMaxima = 11.5f;
    [SerializeField] private float nivelAgua = 8f;

    [Header("Inteligencia")]
    [SerializeField] private float distanciaDeteccion = 5f;
    [SerializeField] private float distanciaPanico = 2.5f;
    [SerializeField] private float tiempoMinimoVuelo = 4f;
    [SerializeField] private float tiempoMaximoVuelo = 8f;
    [SerializeField] private float tiempoMinimoReposo = 2f;
    [SerializeField] private float tiempoMaximoReposo = 5f;

    [Header("Animacion")]
    [SerializeField] private float velocidadAnimacion = 0.12f;

    [Header("Puntuacion")]
    [SerializeField] private int puntos = 2;

    private Transform jugador;
    private Vector2 direccion;
    private float alturaObjetivo;

    private bool reposando;
    private bool escapando;
    private bool comida;

    private float temporizadorEstado;
    private float temporizadorAnimacion;
    private int frameActual;

    private void Start()
    {
        GameObject player =
            GameObject.FindGameObjectWithTag("Player");

        if (player != null)
        {
            jugador = player.transform;
        }

        if (spriteRenderer == null)
        {
            spriteRenderer =
                GetComponent<SpriteRenderer>();
        }

        direccion =
            Random.value < 0.5f
                ? Vector2.left
                : Vector2.right;

        alturaObjetivo =
            Random.Range(
                alturaMinima,
                alturaMaxima
            );

        temporizadorEstado =
            Random.Range(
                tiempoMinimoVuelo,
                tiempoMaximoVuelo
            );

        ActualizarDireccionVisual();
    }

    private void Update()
    {
        if (comida)
        {
            return;
        }

        DetectarJugador();

        if (escapando)
        {
            Escapar();
        }
        else if (reposando)
        {
            Reposar();
        }
        else
        {
            Volar();
        }

        if (!reposando)
        {
            AnimarVuelo();
        }

        ActualizarDireccionVisual();
    }

    private void DetectarJugador()
    {
        if (jugador == null)
        {
            return;
        }

        float distancia =
            Vector2.Distance(
                transform.position,
                jugador.position
            );

        if (distancia <= distanciaDeteccion)
        {
            escapando = true;
            reposando = false;

            Vector2 alejamiento =
                (
                    (Vector2)transform.position -
                    (Vector2)jugador.position
                ).normalized;

            float direccionX =
                alejamiento.x >= 0f
                    ? 1f
                    : -1f;

            direccion =
                new Vector2(
                    direccionX,
                    0.5f
                ).normalized;

            alturaObjetivo =
                Mathf.Min(
                    alturaMaxima,
                    transform.position.y + 2f
                );

            if (distancia <= distanciaPanico)
            {
                direccion.y = 0.8f;
                direccion.Normalize();
            }
        }
        else
        {
            escapando = false;
        }
    }

    private void Volar()
    {
        temporizadorEstado -=
            Time.deltaTime;

        float diferenciaY =
            alturaObjetivo -
            transform.position.y;

        Vector2 movimiento =
            new Vector2(
                direccion.x,
                Mathf.Clamp(
                    diferenciaY,
                    -0.5f,
                    0.5f
                )
            ).normalized;

        transform.position +=
            (Vector3)(
                movimiento *
                velocidadVuelo *
                Time.deltaTime
            );

        float y =
            Mathf.Clamp(
                transform.position.y,
                alturaMinima,
                alturaMaxima
            );

        transform.position =
            new Vector3(
                transform.position.x,
                y,
                transform.position.z
            );

        if (temporizadorEstado <= 0f)
        {
            if (Random.value < 0.5f)
            {
                EmpezarReposo();
            }
            else
            {
                CambiarVuelo();
            }
        }
    }

    private void Escapar()
    {
        Vector2 movimiento =
            new Vector2(
                direccion.x,
                Mathf.Max(
                    direccion.y,
                    0.25f
                )
            ).normalized;

        transform.position +=
            (Vector3)(
                movimiento *
                velocidadEscape *
                Time.deltaTime
            );

        float y =
            Mathf.Clamp(
                transform.position.y,
                nivelAgua + 0.3f,
                alturaMaxima
            );

        transform.position =
            new Vector3(
                transform.position.x,
                y,
                transform.position.z
            );
    }

    private void EmpezarReposo()
    {
        reposando = true;
        escapando = false;

        transform.position =
            new Vector3(
                transform.position.x,
                nivelAgua + 0.15f,
                transform.position.z
            );

        temporizadorEstado =
            Random.Range(
                tiempoMinimoReposo,
                tiempoMaximoReposo
            );

        if (
            spriteRenderer != null &&
            spriteReposo != null
        )
        {
            spriteRenderer.sprite =
                spriteReposo;
        }
    }

    private void Reposar()
    {
        temporizadorEstado -=
            Time.deltaTime;

        transform.position =
            new Vector3(
                transform.position.x,
                nivelAgua + 0.15f,
                transform.position.z
            );

        if (temporizadorEstado <= 0f)
        {
            reposando = false;

            direccion =
                Random.value < 0.5f
                    ? Vector2.left
                    : Vector2.right;

            alturaObjetivo =
                Random.Range(
                    alturaMinima,
                    alturaMaxima
                );

            temporizadorEstado =
                Random.Range(
                    tiempoMinimoVuelo,
                    tiempoMaximoVuelo
                );
        }
    }

    private void CambiarVuelo()
    {
        if (Random.value < 0.35f)
        {
            direccion.x *= -1f;
        }

        alturaObjetivo =
            Random.Range(
                alturaMinima,
                alturaMaxima
            );

        temporizadorEstado =
            Random.Range(
                tiempoMinimoVuelo,
                tiempoMaximoVuelo
            );
    }

    private void AnimarVuelo()
    {
        if (
            spritesVuelo == null ||
            spritesVuelo.Length == 0 ||
            spriteRenderer == null
        )
        {
            return;
        }

        temporizadorAnimacion +=
            Time.deltaTime;

        if (
            temporizadorAnimacion <
            velocidadAnimacion
        )
        {
            return;
        }

        temporizadorAnimacion = 0f;

        frameActual++;

        if (
            frameActual >=
            spritesVuelo.Length
        )
        {
            frameActual = 0;
        }

        spriteRenderer.sprite =
            spritesVuelo[frameActual];
    }

    private void ActualizarDireccionVisual()
    {
        if (spriteRenderer == null)
        {
            return;
        }

        spriteRenderer.flipX =
            direccion.x < 0f;
    }

    private void OnTriggerEnter2D(
        Collider2D other
    )
    {
        if (
            comida ||
            !other.CompareTag("Mouth")
        )
        {
            return;
        }

        comida = true;

        PlayerController player =
            other.GetComponentInParent<PlayerController>();

        if (player != null)
        {
            player.Comer();
        }

        if (GameManager.Instance != null)
        {
            GameManager.Instance.SumarPuntos(
                puntos
            );
        }

        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.ReproducirComer();
        }

        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(
            transform.position,
            distanciaDeteccion
        );
    }
}