using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movimiento")]
    [SerializeField] private float velocidad = 5f;
    [SerializeField] private Transform visual;
    [SerializeField] private Transform boca;
    [SerializeField] private float posicionBocaX = 1.2f;

    [Header("Turbo")]
    [SerializeField] private float velocidadTurbo = 9f;
    [SerializeField] private float duracionTurbo = 1.2f;
    [SerializeField] private float tiempoRecargaTurbo = 2.5f;

    [Header("Agua y aire")]
    [SerializeField] private float nivelSuperficieAgua = 8f;
    [SerializeField] private float gravedadAire = 3.5f;
    [SerializeField] private float controlHorizontalAire = 3f;
    [SerializeField] private float velocidadMaximaAireX = 7f;
    [SerializeField] private float impulsoSalidaAgua = 4f;
    [SerializeField] private float impulsoSalidaTurbo = 8f;

    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private Vector2 movimiento;

    private float turboRestante;
    private float recargaRestante;

    private bool usandoTurbo;
    private bool mirandoIzquierda;
    private bool dentroDelAgua = true;

    public float TurboRestante => turboRestante;
    public float DuracionTurbo => duracionTurbo;
    public float RecargaRestante => recargaRestante;
    public float TiempoRecargaTurbo => tiempoRecargaTurbo;
    public bool UsandoTurbo => usandoTurbo;
    public bool TurboEnRecarga => recargaRestante > 0f;
    public bool DentroDelAgua => dentroDelAgua;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = visual.GetComponent<SpriteRenderer>();
        animator = visual.GetComponent<Animator>();

        turboRestante = duracionTurbo;
        rb.gravityScale = 0f;
    }

    private void Update()
    {
        RevisarSuperficie();

        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        movimiento = new Vector2(horizontal, vertical).normalized;

        ActualizarOrientacion();

        if (dentroDelAgua)
        {
            ActualizarTurbo();
        }
        else
        {
            usandoTurbo = false;
        }
    }

    private void FixedUpdate()
    {
        if (dentroDelAgua)
        {
            MovimientoEnAgua();
        }
        else
        {
            MovimientoEnAire();
        }
    }

    private void RevisarSuperficie()
    {
        bool nuevoEstado = transform.position.y <= nivelSuperficieAgua;

        if (nuevoEstado != dentroDelAgua)
        {
            CambiarEstadoAgua(nuevoEstado);
        }
    }

    private void MovimientoEnAgua()
    {
        rb.gravityScale = 0f;

        float velocidadActual = usandoTurbo
            ? velocidadTurbo
            : velocidad;

        rb.linearVelocity = movimiento * velocidadActual;
    }

    private void MovimientoEnAire()
    {
        rb.gravityScale = gravedadAire;

        float nuevaVelocidadX =
            rb.linearVelocity.x +
            movimiento.x * controlHorizontalAire * Time.fixedDeltaTime;

        nuevaVelocidadX = Mathf.Clamp(
            nuevaVelocidadX,
            -velocidadMaximaAireX,
            velocidadMaximaAireX
        );

        rb.linearVelocity = new Vector2(
            nuevaVelocidadX,
            rb.linearVelocity.y
        );
    }

    public void CambiarEstadoAgua(bool estaDentro)
    {
        bool estabaDentro = dentroDelAgua;
        dentroDelAgua = estaDentro;

        if (dentroDelAgua)
        {
            rb.gravityScale = 0f;
            return;
        }

        rb.gravityScale = gravedadAire;

        if (estabaDentro && rb.linearVelocity.y > 0f)
        {
            float impulso = usandoTurbo
                ? impulsoSalidaTurbo
                : impulsoSalidaAgua;

            rb.linearVelocity = new Vector2(
                rb.linearVelocity.x,
                rb.linearVelocity.y + impulso
            );
        }

        usandoTurbo = false;
    }

    private void ActualizarOrientacion()
    {
        Vector2 direccion;

        if (dentroDelAgua)
        {
            direccion = movimiento;
        }
        else
        {
            direccion = rb.linearVelocity.normalized;
        }

        if (direccion.sqrMagnitude <= 0.01f)
        {
            return;
        }

        if (direccion.x > 0.01f)
        {
            mirandoIzquierda = false;
        }
        else if (direccion.x < -0.01f)
        {
            mirandoIzquierda = true;
        }

        float angulo;

        if (mirandoIzquierda)
        {
            spriteRenderer.flipX = true;

            angulo = Mathf.Atan2(
                -direccion.y,
                Mathf.Abs(direccion.x)
            ) * Mathf.Rad2Deg;
        }
        else
        {
            spriteRenderer.flipX = false;

            angulo = Mathf.Atan2(
                direccion.y,
                Mathf.Abs(direccion.x)
            ) * Mathf.Rad2Deg;
        }

        visual.localRotation = Quaternion.Euler(0f, 0f, angulo);

        ActualizarPosicionBoca();
    }

    private void ActualizarPosicionBoca()
    {
        if (boca == null)
        {
            return;
        }

        float nuevaX = mirandoIzquierda
            ? -Mathf.Abs(posicionBocaX)
            : Mathf.Abs(posicionBocaX);

        boca.localPosition = new Vector3(
            nuevaX,
            boca.localPosition.y,
            boca.localPosition.z
        );
    }

    private void ActualizarTurbo()
    {
        if (recargaRestante > 0f)
        {
            recargaRestante -= Time.deltaTime;
            usandoTurbo = false;

            if (recargaRestante <= 0f)
            {
                recargaRestante = 0f;
                turboRestante = duracionTurbo;
            }

            return;
        }

        bool quiereTurbo =
            Input.GetKey(KeyCode.M) &&
            movimiento != Vector2.zero &&
            turboRestante > 0f;

        usandoTurbo = quiereTurbo;

        if (!usandoTurbo)
        {
            return;
        }

        turboRestante -= Time.deltaTime;

        if (turboRestante <= 0f)
        {
            turboRestante = 0f;
            usandoTurbo = false;
            recargaRestante = tiempoRecargaTurbo;
        }
    }

    public void Comer()
    {
        if (animator != null)
        {
            animator.SetTrigger("comer");
        }
    }
}