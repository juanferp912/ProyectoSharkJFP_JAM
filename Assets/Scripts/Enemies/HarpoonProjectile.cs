using UnityEngine;

public class HarpoonProjectile : MonoBehaviour
{
    [SerializeField] private float velocidad = 8f;
    [SerializeField] private int danio = 1;
    [SerializeField] private float tiempoDeVida = 5f;
    [SerializeField] private float correccionRotacion;

    private Vector2 direccion;

    private void Start()
    {
        Destroy(gameObject, tiempoDeVida);
    }

    public void ConfigurarDireccion(Vector2 nuevaDireccion)
    {
        direccion = nuevaDireccion.normalized;

        float angulo =
            Mathf.Atan2(direccion.y, direccion.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(
            0f,
            0f,
            angulo + correccionRotacion
        );
    }

    private void Update()
    {
        transform.position +=
            (Vector3)(direccion * velocidad * Time.deltaTime);
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