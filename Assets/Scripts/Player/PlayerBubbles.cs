using UnityEngine;

public class PlayerBubbles : MonoBehaviour
{
    [SerializeField] private PlayerController jugador;
    [SerializeField] private ParticleSystem burbujas;
    [SerializeField] private Rigidbody2D rb;

    [Header("Burbujas")]
    [SerializeField] private float velocidadMinima = 0.5f;
    [SerializeField] private float distanciaCola = 1.3f;

    private void Start()
    {
        if (jugador == null)
        {
            jugador =
                GetComponentInParent<PlayerController>();
        }

        if (rb == null && jugador != null)
        {
            rb =
                jugador.GetComponent<Rigidbody2D>();
        }

        if (burbujas == null)
        {
            burbujas =
                GetComponent<ParticleSystem>();
        }
    }

    private void Update()
    {
        if (
            jugador == null ||
            rb == null ||
            burbujas == null
        )
        {
            return;
        }

        ActualizarPosicion();

        bool debeEmitir =
            jugador.DentroDelAgua &&
            rb.linearVelocity.magnitude >= velocidadMinima;

        if (debeEmitir)
        {
            if (!burbujas.isPlaying)
            {
                burbujas.Play();
            }
        }
        else
        {
            if (burbujas.isPlaying)
            {
                burbujas.Stop(
                    true,
                    ParticleSystemStopBehavior.StopEmitting
                );
            }
        }
    }

    private void ActualizarPosicion()
    {
        Vector2 velocidad =
            rb.linearVelocity;

        if (velocidad.sqrMagnitude < 0.01f)
        {
            return;
        }

        Vector2 direccion =
            velocidad.normalized;

        Vector2 posicionCola =
            (Vector2)jugador.transform.position -
            direccion * distanciaCola;

        transform.position = new Vector3(
            posicionCola.x,
            posicionCola.y,
            transform.position.z
        );
    }
}