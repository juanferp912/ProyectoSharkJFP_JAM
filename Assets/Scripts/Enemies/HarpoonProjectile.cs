using UnityEngine;

public class HarpoonProjectile : MonoBehaviour
{
    [SerializeField] private float velocidad = 8f;
    [SerializeField] private int danio = 1;
    [SerializeField] private float tiempoDeVida = 5f;
    [SerializeField] private float correccionRotacion;

    [Header("Agua y aire")]
    [SerializeField] private float nivelSuperficieAgua = 8f;
    [SerializeField] private float gravedadAire = 8f;
    [SerializeField] private float velocidadMaximaCaida = 12f;

    private Vector2 direccion;
    private float velocidadVerticalAire;
    private bool enAire;

    private void Start()
    {
        Destroy(gameObject, tiempoDeVida);
    }

    public void ConfigurarDireccion(Vector2 nuevaDireccion)
    {
        direccion = nuevaDireccion.normalized;

        float angulo =
            Mathf.Atan2(direccion.y, direccion.x) *
            Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(
            0f,
            0f,
            angulo + correccionRotacion
        );
    }

    private void Update()
    {
        if (transform.position.y > nivelSuperficieAgua)
        {
            MoverEnAire();
        }
        else
        {
            MoverEnAgua();
        }
    }

    private void MoverEnAgua()
    {
        enAire = false;
        velocidadVerticalAire = 0f;

        transform.position +=
            (Vector3)(direccion * velocidad * Time.deltaTime);
    }

    private void MoverEnAire()
    {
        if (!enAire)
        {
            enAire = true;

            velocidadVerticalAire =
                direccion.y * velocidad;
        }

        velocidadVerticalAire -=
            gravedadAire * Time.deltaTime;

        velocidadVerticalAire = Mathf.Max(
            velocidadVerticalAire,
            -velocidadMaximaCaida
        );

        float movimientoX =
            direccion.x * velocidad * Time.deltaTime;

        float movimientoY =
            velocidadVerticalAire * Time.deltaTime;

        transform.position += new Vector3(
            movimientoX,
            movimientoY,
            0f
        );

        Vector2 direccionVisual = new Vector2(
            direccion.x * velocidad,
            velocidadVerticalAire
        ).normalized;

        if (direccionVisual.sqrMagnitude > 0.01f)
        {
            float angulo =
                Mathf.Atan2(
                    direccionVisual.y,
                    direccionVisual.x
                ) * Mathf.Rad2Deg;

            transform.rotation = Quaternion.Euler(
                0f,
                0f,
                angulo + correccionRotacion
            );
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player"))
        {
            return;
        }

        PlayerHealth saludJugador =
            collision.GetComponent<PlayerHealth>();

        if (saludJugador != null)
        {
            saludJugador.RecibirDanio(danio);
        }

        Destroy(gameObject);
    }
}