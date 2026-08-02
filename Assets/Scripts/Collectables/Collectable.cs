using UnityEngine;

public class Collectable : MonoBehaviour
{
    [Header("Movimiento")]
    [SerializeField] private float velocidadNormal = 1.8f;
    [SerializeField] private float velocidadHuida = 3f;
    [SerializeField] private float tiempoCambioDireccion = 2f;

    [Header("Detección")]
    [SerializeField] private float distanciaDeteccion = 4f;

    [Header("Límites del mundo")]
    [SerializeField] private Vector2 limiteMinimo = new Vector2(-15f, -8f);
    [SerializeField] private Vector2 limiteMaximo = new Vector2(15f, 8f);

    private Transform jugador;
    private SpriteRenderer spriteRenderer;
    private Vector2 direccionMovimiento;
    private float contadorDireccion;
    private bool recogido;

    private void Start()
    {
        GameObject objetoJugador = GameObject.FindGameObjectWithTag("Player");

        if (objetoJugador != null)
        {
            jugador = objetoJugador.transform;
        }

        spriteRenderer = GetComponent<SpriteRenderer>();
        ElegirDireccionAleatoria();
    }

    private void Update()
    {
        if (jugador == null || recogido)
        {
            return;
        }

        float distancia = Vector2.Distance(transform.position, jugador.position);

        if (distancia <= distanciaDeteccion)
        {
            Huir();
        }
        else
        {
            MoverAleatoriamente();
        }

        ControlarLimites();
        GirarSprite();
    }

    private void MoverAleatoriamente()
    {
        contadorDireccion -= Time.deltaTime;

        if (contadorDireccion <= 0f)
        {
            ElegirDireccionAleatoria();
        }

        transform.position +=
            (Vector3)(direccionMovimiento * velocidadNormal * Time.deltaTime);
    }

    private void Huir()
    {
        direccionMovimiento =
            ((Vector2)transform.position - (Vector2)jugador.position).normalized;

        transform.position +=
            (Vector3)(direccionMovimiento * velocidadHuida * Time.deltaTime);
    }

    private void ElegirDireccionAleatoria()
    {
        direccionMovimiento = Random.insideUnitCircle.normalized;
        contadorDireccion = tiempoCambioDireccion;
    }

    private void ControlarLimites()
    {
        Vector2 posicion = transform.position;

        if (posicion.x <= limiteMinimo.x || posicion.x >= limiteMaximo.x)
        {
            direccionMovimiento.x *= -1f;
        }

        if (posicion.y <= limiteMinimo.y || posicion.y >= limiteMaximo.y)
        {
            direccionMovimiento.y *= -1f;
        }

        posicion.x = Mathf.Clamp(posicion.x, limiteMinimo.x, limiteMaximo.x);
        posicion.y = Mathf.Clamp(posicion.y, limiteMinimo.y, limiteMaximo.y);

        transform.position = posicion;
    }

    private void GirarSprite()
    {
        if (direccionMovimiento.x > 0.01f)
        {
            spriteRenderer.flipX = false;
        }
        else if (direccionMovimiento.x < -0.01f)
        {
            spriteRenderer.flipX = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
{
    if (recogido)
    {
        return;
    }

    if (collision.CompareTag("Mouth"))
    {
        recogido = true;

        if (GameManager.Instance != null)
        {
            GameManager.Instance.SumarPuntos(puntos);
        }

        Destroy(gameObject);
    }
}

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, distanciaDeteccion);
    }
}