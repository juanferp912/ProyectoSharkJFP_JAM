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

    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private Vector2 movimiento;
    private float turboRestante;
    private float recargaRestante;
    private bool usandoTurbo;
    private bool mirandoIzquierda;

    public float TurboRestante => turboRestante;
    public float DuracionTurbo => duracionTurbo;
    public float RecargaRestante => recargaRestante;
    public float TiempoRecargaTurbo => tiempoRecargaTurbo;
    public bool UsandoTurbo => usandoTurbo;
    public bool TurboEnRecarga => recargaRestante > 0f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = visual.GetComponent<SpriteRenderer>();
        animator = visual.GetComponent<Animator>();
        turboRestante = duracionTurbo;
    }

    private void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        movimiento = new Vector2(horizontal, vertical).normalized;

        ActualizarOrientacion();
        ActualizarTurbo();
    }

    private void FixedUpdate()
    {
        float velocidadActual = usandoTurbo ? velocidadTurbo : velocidad;
        rb.linearVelocity = movimiento * velocidadActual;
    }

    private void ActualizarOrientacion()
    {
        if (movimiento.sqrMagnitude <= 0.01f)
        {
            return;
        }

        if (movimiento.x > 0.01f)
        {
            mirandoIzquierda = false;
        }
        else if (movimiento.x < -0.01f)
        {
            mirandoIzquierda = true;
        }

        float angulo;

        if (mirandoIzquierda)
        {
            spriteRenderer.flipX = true;

            angulo = Mathf.Atan2(
                -movimiento.y,
                Mathf.Abs(movimiento.x)
            ) * Mathf.Rad2Deg;
        }
        else
        {
            spriteRenderer.flipX = false;

            angulo = Mathf.Atan2(
                movimiento.y,
                Mathf.Abs(movimiento.x)
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