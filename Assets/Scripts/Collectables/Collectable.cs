using UnityEngine;

public class Collectable : MonoBehaviour
{
    [Header("Movimiento")]
    [SerializeField] private float velocidadNormal = 1.8f;
    [SerializeField] private float velocidadHuida = 3f;
    [SerializeField] private float tiempoCambioDireccion = 2f;

    [Header("Detección")]
    [SerializeField] private float distanciaDeteccion = 4f;

    [Header("Limites verticales")]
    [SerializeField] private float limiteInferiorY = -14f;
    [SerializeField] private float nivelSuperficieAgua = 8f;
    [SerializeField] private float margenSuperficie = 0.7f;

    [Header("Puntuación")]
    [SerializeField] private int puntos = 1;

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

        CorregirPosicionInicial();
        ElegirDireccionAleatoria();
    }

    private void Update()
    {
        if (recogido)
        {
            return;
        }

        if (jugador != null)
        {
            float distancia = Vector2.Distance(
                transform.position,
                jugador.position
            );

            if (distancia <= distanciaDeteccion)
            {
                Huir();
            }
            else
            {
                MoverAleatoriamente();
            }
        }
        else
        {
            MoverAleatoriamente();
        }

        ControlarAltura();
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

        float limiteSuperior =
            nivelSuperficieAgua - margenSuperficie;

        if (
            transform.position.y >= limiteSuperior - 0.3f &&
            direccionMovimiento.y > 0f
        )
        {
            direccionMovimiento.y = -Mathf.Abs(direccionMovimiento.y);
        }

        transform.position +=
            (Vector3)(direccionMovimiento * velocidadHuida * Time.deltaTime);
    }

    private void ElegirDireccionAleatoria()
    {
        direccionMovimiento = Random.insideUnitCircle.normalized;

        float limiteSuperior =
            nivelSuperficieAgua - margenSuperficie;

        if (
            transform.position.y >= limiteSuperior - 0.5f &&
            direccionMovimiento.y > 0f
        )
        {
            direccionMovimiento.y = -Mathf.Abs(direccionMovimiento.y);
        }

        if (
            transform.position.y <= limiteInferiorY + 0.5f &&
            direccionMovimiento.y < 0f
        )
        {
            direccionMovimiento.y = Mathf.Abs(direccionMovimiento.y);
        }

        contadorDireccion = tiempoCambioDireccion;
    }

    private void ControlarAltura()
    {
        Vector3 posicion = transform.position;

        float limiteSuperior =
            nivelSuperficieAgua - margenSuperficie;

        if (posicion.y > limiteSuperior)
        {
            posicion.y = limiteSuperior;
            direccionMovimiento.y = -Mathf.Abs(direccionMovimiento.y);
        }

        if (posicion.y < limiteInferiorY)
        {
            posicion.y = limiteInferiorY;
            direccionMovimiento.y = Mathf.Abs(direccionMovimiento.y);
        }

        transform.position = posicion;
    }

    private void CorregirPosicionInicial()
    {
        Vector3 posicion = transform.position;

        float limiteSuperior =
            nivelSuperficieAgua - margenSuperficie;

        posicion.y = Mathf.Clamp(
            posicion.y,
            limiteInferiorY,
            limiteSuperior
        );

        transform.position = posicion;
    }

    private void GirarSprite()
    {
        if (spriteRenderer == null)
        {
            return;
        }

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

        if (!collision.CompareTag("Mouth"))
        {
            return;
        }

        recogido = true;

        PlayerController jugadorController =
            collision.GetComponentInParent<PlayerController>();

        if (jugadorController != null)
        {
            jugadorController.Comer();
        }

        if (GameManager.Instance != null)
        {
            GameManager.Instance.SumarPuntos(puntos);
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