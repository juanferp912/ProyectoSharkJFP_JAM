using UnityEngine;

public class HarpoonProjectile : MonoBehaviour
{
    [SerializeField] private float velocidad = 8f;
    [SerializeField] private int danio = 1;
    [SerializeField] private float tiempoDeVida = 5f;
    [SerializeField] private float correccionRotacion;

    [Header("Aire")]
    [SerializeField] private float nivelSuperficieAgua = 8f;
    [SerializeField] private float gravedadAire = 8f;
    [SerializeField] private float velocidadMaximaCaidaAire = 12f;

    [Header("Regreso al agua")]
    [SerializeField] private float factorFrenadoEntradaAgua = 0.3f;
    [SerializeField] private float gravedadBajoAgua = 0.8f;
    [SerializeField] private float velocidadMaximaCaidaAgua = 2.5f;

    private Vector2 velocidadActual;
    private bool direccionConfigurada;
    private bool salioDelAgua;
    private bool volvioAlAgua;

    private void Start()
    {
        Destroy(
            gameObject,
            tiempoDeVida
        );
    }

    public void ConfigurarDireccion(
        Vector2 nuevaDireccion
    )
    {
        velocidadActual =
            nuevaDireccion.normalized *
            velocidad;

        direccionConfigurada = true;

        ActualizarRotacion();
    }

    private void Update()
    {
        if (!direccionConfigurada)
        {
            return;
        }

        RevisarEstado();

        if (volvioAlAgua)
        {
            MoverDespuesDeEntrarAlAgua();
        }
        else if (salioDelAgua)
        {
            MoverEnAire();
        }

        transform.position +=
            (Vector3)(
                velocidadActual *
                Time.deltaTime
            );

        ActualizarRotacion();
    }

    private void RevisarEstado()
    {
        if (
            !salioDelAgua &&
            transform.position.y >
            nivelSuperficieAgua
        )
        {
            salioDelAgua = true;
        }

        if (
            salioDelAgua &&
            !volvioAlAgua &&
            transform.position.y <=
            nivelSuperficieAgua &&
            velocidadActual.y < 0f
        )
        {
            volvioAlAgua = true;

            velocidadActual *=
                factorFrenadoEntradaAgua;
        }
    }

    private void MoverEnAire()
    {
        velocidadActual.y -=
            gravedadAire *
            Time.deltaTime;

        velocidadActual.y =
            Mathf.Max(
                velocidadActual.y,
                -velocidadMaximaCaidaAire
            );
    }

    private void MoverDespuesDeEntrarAlAgua()
    {
        velocidadActual.y -=
            gravedadBajoAgua *
            Time.deltaTime;

        velocidadActual.y =
            Mathf.Max(
                velocidadActual.y,
                -velocidadMaximaCaidaAgua
            );

        velocidadActual.x =
            Mathf.MoveTowards(
                velocidadActual.x,
                0f,
                1.5f *
                Time.deltaTime
            );
    }

    private void ActualizarRotacion()
    {
        if (
            velocidadActual.sqrMagnitude <=
            0.01f
        )
        {
            return;
        }

        float angulo =
            Mathf.Atan2(
                velocidadActual.y,
                velocidadActual.x
            ) *
            Mathf.Rad2Deg;

        transform.rotation =
            Quaternion.Euler(
                0f,
                0f,
                angulo +
                correccionRotacion
            );
    }

    private void OnTriggerEnter2D(
        Collider2D collision
    )
    {
        if (!collision.CompareTag("Player"))
        {
            return;
        }

        PlayerHealth saludJugador =
            collision.GetComponent<PlayerHealth>();

        if (saludJugador == null)
        {
            saludJugador =
                collision.GetComponentInParent<PlayerHealth>();
        }

        if (saludJugador != null)
        {
            saludJugador.RecibirDanio(danio);
        }

        Destroy(gameObject);
    }
}