using UnityEngine;

public class JellyfishEnemy : MonoBehaviour
{
    [Header("Movimiento")]
    [SerializeField] private float velocidadVertical = 1f;
    [SerializeField] private float alturaMovimiento = 1.5f;
    [SerializeField] private float velocidadHorizontal = 0.4f;

    [Header("Agua")]
    [SerializeField] private float nivelSuperficieAgua = 8f;
    [SerializeField] private float margenSuperficie = 1f;
    [SerializeField] private float limiteInferiorY = -13f;

    [Header("Daño")]
    [SerializeField] private int danio = 1;
    [SerializeField] private float tiempoEntreDanios = 1.5f;

    private float posicionInicialY;
    private float tiempo;
    private float ultimoDanio = -100f;
    private float direccionHorizontal;

    private void Start()
    {
        float limiteSuperior = nivelSuperficieAgua - margenSuperficie;

        Vector3 posicion = transform.position;

        posicion.y = Mathf.Clamp(
            posicion.y,
            limiteInferiorY,
            limiteSuperior
        );

        transform.position = posicion;

        posicionInicialY = transform.position.y;

        direccionHorizontal =
            Random.value < 0.5f ? -1f : 1f;
    }

    private void Update()
    {
        tiempo += Time.deltaTime;

        float limiteSuperior =
            nivelSuperficieAgua - margenSuperficie;

        float nuevaY =
            posicionInicialY +
            Mathf.Sin(tiempo * velocidadVertical) *
            alturaMovimiento;

        nuevaY = Mathf.Clamp(
            nuevaY,
            limiteInferiorY,
            limiteSuperior
        );

        float nuevaX =
            transform.position.x +
            direccionHorizontal *
            velocidadHorizontal *
            Time.deltaTime;

        transform.position = new Vector3(
            nuevaX,
            nuevaY,
            transform.position.z
        );

        if (
            transform.position.y >= limiteSuperior &&
            nuevaY >= limiteSuperior
        )
        {
            posicionInicialY =
                limiteSuperior - alturaMovimiento;
        }

        if (
            transform.position.y <= limiteInferiorY &&
            nuevaY <= limiteInferiorY
        )
        {
            posicionInicialY =
                limiteInferiorY + alturaMovimiento;
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        PlayerHealth playerHealth =
            other.GetComponent<PlayerHealth>();

        if (playerHealth == null)
        {
            playerHealth =
                other.GetComponentInParent<PlayerHealth>();
        }

        if (playerHealth == null)
        {
            return;
        }

        if (
            Time.time <
            ultimoDanio + tiempoEntreDanios
        )
        {
            return;
        }

        playerHealth.RecibirDanio(danio);
        ultimoDanio = Time.time;
    }
}