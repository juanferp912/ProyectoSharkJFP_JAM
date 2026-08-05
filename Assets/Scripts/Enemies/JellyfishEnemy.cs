using UnityEngine;

public class JellyfishEnemy : MonoBehaviour
{
    [Header("Movimiento")]
    [SerializeField] private float velocidadVertical = 1f;
    [SerializeField] private float alturaMovimiento = 1.5f;
    [SerializeField] private float velocidadHorizontal = 0.4f;

    [Header("Daño")]
    [SerializeField] private int danio = 1;
    [SerializeField] private float tiempoEntreDanios = 1.5f;

    [Header("Límites")]
    [SerializeField] private Vector2 limiteMinimo = new Vector2(-25f, -12f);
    [SerializeField] private Vector2 limiteMaximo = new Vector2(25f, 12f);

    private Vector3 posicionInicial;
    private float direccionHorizontal;
    private float ultimoDanio = -100f;

    private void Start()
    {
        posicionInicial = transform.position;
        direccionHorizontal = Random.value < 0.5f ? -1f : 1f;
    }

    private void Update()
    {
        float posicionY =
            posicionInicial.y +
            Mathf.Sin(Time.time * velocidadVertical) * alturaMovimiento;

        float posicionX =
            transform.position.x +
            direccionHorizontal * velocidadHorizontal * Time.deltaTime;

        if (posicionX <= limiteMinimo.x || posicionX >= limiteMaximo.x)
        {
            direccionHorizontal *= -1f;
        }

        posicionX = Mathf.Clamp(
            posicionX,
            limiteMinimo.x,
            limiteMaximo.x
        );

        posicionY = Mathf.Clamp(
            posicionY,
            limiteMinimo.y,
            limiteMaximo.y
        );

        transform.position = new Vector3(
            posicionX,
            posicionY,
            transform.position.z
        );
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        IntentarHacerDanio(collision);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        IntentarHacerDanio(collision);
    }

    private void IntentarHacerDanio(Collider2D collision)
    {
        if (!collision.CompareTag("Player"))
        {
            return;
        }

        if (Time.time < ultimoDanio + tiempoEntreDanios)
        {
            return;
        }

        PlayerHealth saludJugador =
            collision.GetComponent<PlayerHealth>();

        if (saludJugador == null)
        {
            return;
        }

        ultimoDanio = Time.time;
        saludJugador.RecibirDanio(danio);
    }
}