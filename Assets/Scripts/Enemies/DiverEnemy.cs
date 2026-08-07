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

    [Header("Agua")]
    [SerializeField] private float nivelSuperficieAgua = 8f;
    [SerializeField] private float margenSuperficie = 1f;
    [SerializeField] private float limiteInferiorY = -12f;

    private Transform jugador;
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private Vector2 direccionMovimiento;
    private float contadorDireccion;
    private float contadorDisparo;
    private bool armado;
    private bool comido;

    private void Start()
    {
        GameObject objetoJugador =
            GameObject.FindGameObjectWithTag("Player");

        if (objetoJugador != null)
        {
            jugador = objetoJugador.transform;
        }

        spriteRenderer =
            GetComponent<SpriteRenderer>();

        animator =
            GetComponent<Animator>();

        CorregirPosicionInicial();
        ElegirDireccionAleatoria();

        contadorDisparo =
            tiempoEntreDisparos;
    }

    private void Update()
    {
        if (jugador == null || comido)
        {
            return;
        }

        float distancia =
            Vector2.Distance(
                transform.position,
                jugador.position
            );

        if (
            !armado &&
            distancia <= distanciaDeteccion
        )
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

        ControlarAltura();
    }

    private void Nadar()
    {
        transform.rotation =
            Quaternion.identity;

        if (spriteRenderer != null)
        {
            spriteRenderer.flipY = false;
        }

        contadorDireccion -=
            Time.deltaTime;

        if (contadorDireccion <= 0f)
        {
            ElegirDireccionAleatoria();
        }

        float limiteSuperior =
            nivelSuperficieAgua -
            margenSuperficie;

        if (
            transform.position.y >=
            limiteSuperior - 0.3f &&
            direccionMovimiento.y > 0f
        )
        {
            direccionMovimiento.y =
                -Mathf.Abs(
                    direccionMovimiento.y
                );
        }

        if (
            transform.position.y <=
            limiteInferiorY + 0.3f &&
            direccionMovimiento.y < 0f
        )
        {
            direccionMovimiento.y =
                Mathf.Abs(
                    direccionMovimiento.y
                );
        }

        transform.position +=
            (Vector3)(
                direccionMovimiento *
                velocidadMovimiento *
                Time.deltaTime
            );

        if (spriteRenderer == null)
        {
            return;
        }

        if (direccionMovimiento.x > 0.01f)
        {
            spriteRenderer.flipX = false;
        }
        else if (
            direccionMovimiento.x < -0.01f
        )
        {
            spriteRenderer.flipX = true;
        }
    }

    private void Atacar()
    {
        Vector2 direccionJugador =
            (
                (Vector2)jugador.position -
                (Vector2)transform.position
            ).normalized;

        ApuntarAlJugador(
            direccionJugador
        );

        contadorDisparo -=
            Time.deltaTime;

        if (contadorDisparo <= 0f)
        {
            Disparar(
                direccionJugador
            );

            contadorDisparo =
                tiempoEntreDisparos;
        }
    }

    private void ApuntarAlJugador(
        Vector2 direccion
    )
    {
        float angulo =
            Mathf.Atan2(
                direccion.y,
                direccion.x
            ) *
            Mathf.Rad2Deg;

        if (direccion.x < 0f)
        {
            angulo += 180f;

            if (spriteRenderer != null)
            {
                spriteRenderer.flipX = true;
                spriteRenderer.flipY = true;
            }
        }
        else
        {
            if (spriteRenderer != null)
            {
                spriteRenderer.flipX = false;
                spriteRenderer.flipY = false;
            }
        }

        transform.rotation =
            Quaternion.Euler(
                0f,
                0f,
                angulo +
                correccionRotacionBuzo
            );
    }

    private void Disparar(
        Vector2 direccion
    )
    {
        if (
            lanzaPrefab == null ||
            puntoDisparo == null
        )
        {
            return;
        }

        GameObject nuevaLanza =
            Instantiate(
                lanzaPrefab,
                puntoDisparo.position,
                Quaternion.identity
            );

        if (AudioManager.Instance != null)
        {
            AudioManager.Instance
                .ReproducirDisparo();
        }

        HarpoonProjectile proyectil =
            nuevaLanza
                .GetComponent<HarpoonProjectile>();

        if (proyectil != null)
        {
            proyectil
                .ConfigurarDireccion(
                    direccion
                );
        }
    }

    private void CambiarEstadoArmado(
        bool nuevoEstado
    )
    {
        armado = nuevoEstado;

        if (animator != null)
        {
            animator.SetBool(
                "armado",
                armado
            );
        }

        contadorDisparo =
            tiempoEntreDisparos;

        if (!armado)
        {
            transform.rotation =
                Quaternion.identity;

            if (spriteRenderer != null)
            {
                spriteRenderer.flipY = false;
            }
        }
    }

    private void ElegirDireccionAleatoria()
    {
        direccionMovimiento =
            Random.insideUnitCircle.normalized;

        float limiteSuperior =
            nivelSuperficieAgua -
            margenSuperficie;

        if (
            transform.position.y >=
            limiteSuperior - 0.5f &&
            direccionMovimiento.y > 0f
        )
        {
            direccionMovimiento.y =
                -Mathf.Abs(
                    direccionMovimiento.y
                );
        }

        if (
            transform.position.y <=
            limiteInferiorY + 0.5f &&
            direccionMovimiento.y < 0f
        )
        {
            direccionMovimiento.y =
                Mathf.Abs(
                    direccionMovimiento.y
                );
        }

        contadorDireccion =
            tiempoCambioDireccion;
    }

    private void ControlarAltura()
    {
        Vector3 posicion =
            transform.position;

        float limiteSuperior =
            nivelSuperficieAgua -
            margenSuperficie;

        if (posicion.y > limiteSuperior)
        {
            posicion.y =
                limiteSuperior;

            direccionMovimiento.y =
                -Mathf.Abs(
                    direccionMovimiento.y
                );
        }

        if (posicion.y < limiteInferiorY)
        {
            posicion.y =
                limiteInferiorY;

            direccionMovimiento.y =
                Mathf.Abs(
                    direccionMovimiento.y
                );
        }

        transform.position =
            posicion;
    }

    private void CorregirPosicionInicial()
    {
        Vector3 posicion =
            transform.position;

        float limiteSuperior =
            nivelSuperficieAgua -
            margenSuperficie;

        posicion.y =
            Mathf.Clamp(
                posicion.y,
                limiteInferiorY,
                limiteSuperior
            );

        transform.position =
            posicion;
    }

    private void OnTriggerEnter2D(
        Collider2D collision
    )
    {
        if (comido)
        {
            return;
        }

        if (!collision.CompareTag("Mouth"))
        {
            return;
        }

        comido = true;

        PlayerController player =
            collision.GetComponentInParent<PlayerController>();

        if (player != null)
        {
            player.Comer();
        }

        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.ReproducirComer();
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