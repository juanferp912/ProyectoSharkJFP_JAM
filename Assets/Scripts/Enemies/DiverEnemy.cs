using UnityEngine;

public class DiverEnemy : MonoBehaviour
{
    [Header("Movimiento")]
    [SerializeField] private float velocidadMovimiento = 1.5f;
    [SerializeField] private float tiempoCambioDireccion = 3f;

    [Header("Detección")]
    [SerializeField] private float distanciaDeteccion = 7f;
    [SerializeField] private float distanciaParaDejarDeAtacar = 9f;

    [Header("Disparo")]
    [SerializeField] private GameObject lanzaPrefab;
    [SerializeField] private Transform puntoDisparo;
    [SerializeField] private float tiempoEntreDisparos = 2f;
    [SerializeField] private float correccionRotacionBuzo;

    [Header("Límites")]
    [SerializeField] private Vector2 limiteMinimo =
        new Vector2(-20f, -10f);

    [SerializeField] private Vector2 limiteMaximo =
        new Vector2(20f, 10f);

    private Transform jugador;
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private Vector2 direccionMovimiento;
    private float contadorDireccion;
    private float contadorDisparo;
    private bool armado;

    private void Start()
    {
        GameObject objetoJugador =
            GameObject.FindGameObjectWithTag("Player");

        if (objetoJugador != null)
        {
            jugador = objetoJugador.transform;
        }

        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();

        ElegirDireccionAleatoria();
        contadorDisparo = tiempoEntreDisparos;
    }

    private void Update()
    {
        if (jugador == null)
        {
            return;
        }

        float distancia = Vector2.Distance(
            transform.position,
            jugador.position
        );

        if (!armado && distancia <= distanciaDeteccion)
        {
            CambiarEstadoArmado(true);
        }
        else if (
            armado &&
            distancia >= distanciaParaDejarDeAtacar
        )
        {
            CambiarEstadoArmado(false);
        }

        if (armado)
        {
            Atacar();
        }
        else
        {
            Nadar();
        }

        ControlarLimites();
    }

    private void Nadar()
    {
        transform.rotation = Quaternion.identity;
        spriteRenderer.flipY = false;

        contadorDireccion -= Time.deltaTime;

        if (contadorDireccion <= 0f)
        {
            ElegirDireccionAleatoria();
        }

        transform.position +=
            (Vector3)(
                direccionMovimiento *
                velocidadMovimiento *
                Time.deltaTime
            );

        if (direccionMovimiento.x > 0.01f)
        {
            spriteRenderer.flipX = false;
        }
        else if (direccionMovimiento.x < -0.01f)
        {
            spriteRenderer.flipX = true;
        }
    }

    private void Atacar()
    {
        Vector2 direccionJugador =
            ((Vector2)jugador.position -
             (Vector2)transform.position).normalized;

        ApuntarAlJugador(direccionJugador);

        contadorDisparo -= Time.deltaTime;

        if (contadorDisparo <= 0f)
        {
            Disparar(direccionJugador);
            contadorDisparo = tiempoEntreDisparos;
        }
    }

    private void ApuntarAlJugador(Vector2 direccion)
    {
        float angulo =
            Mathf.Atan2(direccion.y, direccion.x) *
            Mathf.Rad2Deg;

        if (direccion.x < 0f)
        {
            angulo += 180f;
            spriteRenderer.flipX = true;
            spriteRenderer.flipY = true;
        }
        else
        {
            spriteRenderer.flipX = false;
            spriteRenderer.flipY = false;
        }

        transform.rotation = Quaternion.Euler(
            0f,
            0f,
            angulo + correccionRotacionBuzo
        );
    }

    private void Disparar(Vector2 direccion)
    {
        if (lanzaPrefab == null || puntoDisparo == null)
        {
            return;
        }

        GameObject nuevaLanza = Instantiate(
            lanzaPrefab,
            puntoDisparo.position,
            Quaternion.identity
        );

        HarpoonProjectile proyectil =
            nuevaLanza.GetComponent<HarpoonProjectile>();

        if (proyectil != null)
        {
            proyectil.ConfigurarDireccion(direccion);
        }
    }

    private void CambiarEstadoArmado(bool nuevoEstado)
    {
        armado = nuevoEstado;
        animator.SetBool("armado", armado);
        contadorDisparo = tiempoEntreDisparos;

        if (!armado)
        {
            transform.rotation = Quaternion.identity;
            spriteRenderer.flipY = false;
        }
    }

    private void ElegirDireccionAleatoria()
    {
        direccionMovimiento =
            Random.insideUnitCircle.normalized;

        contadorDireccion = tiempoCambioDireccion;
    }

    private void ControlarLimites()
    {
        Vector2 posicion = transform.position;

        if (
            posicion.x <= limiteMinimo.x ||
            posicion.x >= limiteMaximo.x
        )
        {
            direccionMovimiento.x *= -1f;
        }

        if (
            posicion.y <= limiteMinimo.y ||
            posicion.y >= limiteMaximo.y
        )
        {
            direccionMovimiento.y *= -1f;
        }

        posicion.x = Mathf.Clamp(
            posicion.x,
            limiteMinimo.x,
            limiteMaximo.x
        );

        posicion.y = Mathf.Clamp(
            posicion.y,
            limiteMinimo.y,
            limiteMaximo.y
        );

        transform.position = posicion;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(
            transform.position,
            distanciaDeteccion
        );
    }
}