using UnityEngine;

public class OrcaAI : MonoBehaviour
{
    [Header("Sprites")]
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Sprite[] spritesNado;
    [SerializeField] private Sprite spriteAtaque;

    [Header("Weak Spots")]
    [SerializeField] private Transform weakSpots;
    [SerializeField] private BoxCollider2D weakSpotCola;
    [SerializeField] private BoxCollider2D weakSpotAleta;

    [Header("Movimiento")]
    [SerializeField] private float velocidadPatrulla = 2.6f;
    [SerializeField] private float velocidadPersecucion = 3.7f;
    [SerializeField] private float velocidadEmbestida = 6f;
    [SerializeField] private float limiteInferiorY = -13f;
    [SerializeField] private float nivelAgua = 8f;
    [SerializeField] private float margenSuperficie = 0.8f;

    [Header("Deteccion")]
    [SerializeField] private float distanciaDeteccionJugador = 8f;
    [SerializeField] private float distanciaEmbestida = 4f;
    [SerializeField] private float distanciaBuscarPeces = 5f;
    [SerializeField] private float distanciaComerPez = 1f;

    [Header("Ataque")]
    [SerializeField] private int danoJugador = 1;
    [SerializeField] private float tiempoEntreAtaques = 2f;
    [SerializeField] private float duracionEmbestida = 0.65f;
    [SerializeField] private float tiempoEntreEmbestidas = 3f;

    [Header("Vida")]
    [SerializeField] private int golpesNecesarios = 2;
    [SerializeField] private float tiempoEntreGolpes = 0.5f;
    [SerializeField] private int puntosAlMorir = 20;

    [Header("Animacion")]
    [SerializeField] private float velocidadAnimacion = 0.12f;

    private Transform jugador;
    private PlayerHealth playerHealth;
    private OrcaDamageFlash damageFlash;

    private Vector2 direccion;
    private Vector2 direccionEmbestida;

    private Vector3 escalaWeakSpotsInicial;

    private float contadorAnimacion;
    private float contadorAtaque;
    private float contadorEmbestida;
    private float contadorGolpe;
    private float tiempoEmbestidaRestante;
    private float contadorCambioDireccion;

    private int frameActual;
    private int golpesRecibidos;

    private bool embistiendo;
    private bool muerta;
    private bool bocaEstabaDentro;

    public bool Muerta => muerta;

    private void Start()
    {
        GameObject player =
            GameObject.FindGameObjectWithTag("Player");

        if (player != null)
        {
            jugador = player.transform;
            playerHealth =
                player.GetComponent<PlayerHealth>();
        }

        if (spriteRenderer == null)
        {
            spriteRenderer =
                GetComponent<SpriteRenderer>();
        }

        damageFlash =
            GetComponent<OrcaDamageFlash>();

        if (weakSpots != null)
        {
            escalaWeakSpotsInicial =
                weakSpots.localScale;
        }

        direccion =
            Random.value < 0.5f
                ? Vector2.left
                : Vector2.right;

        contadorEmbestida =
            tiempoEntreEmbestidas;

        contadorCambioDireccion = 2f;

        ActualizarDireccionVisual();
    }

    private void Update()
    {
        if (muerta)
        {
            return;
        }

        contadorAtaque -= Time.deltaTime;
        contadorEmbestida -= Time.deltaTime;
        contadorGolpe -= Time.deltaTime;
        contadorCambioDireccion -= Time.deltaTime;

        if (embistiendo)
        {
            ActualizarEmbestida();
        }
        else
        {
            TomarDecision();
        }

        RevisarWeakSpots();
        ComerPeces();
        ControlarAltura();
        Animar();
        ActualizarDireccionVisual();
    }

    private void TomarDecision()
    {
        if (jugador == null)
        {
            Patrullar();
            return;
        }

        float distanciaJugador =
            Vector2.Distance(
                transform.position,
                jugador.position
            );

        if (
            distanciaJugador <= distanciaEmbestida &&
            contadorEmbestida <= 0f
        )
        {
            EmpezarEmbestida();
            return;
        }

        if (
            distanciaJugador <=
            distanciaDeteccionJugador
        )
        {
            PerseguirJugador();
            return;
        }

        Transform pez =
            BuscarPezCercano();

        if (pez != null)
        {
            PerseguirPez(pez);
        }
        else
        {
            Patrullar();
        }
    }

    private void Patrullar()
    {
        if (contadorCambioDireccion <= 0f)
        {
            contadorCambioDireccion =
                Random.Range(1.5f, 3f);

            if (Random.value < 0.35f)
            {
                direccion.x *= -1f;
            }

            direccion.y =
                Random.Range(-0.3f, 0.3f);
        }

        if (Mathf.Abs(direccion.x) < 0.5f)
        {
            direccion.x =
                Random.value < 0.5f
                    ? -1f
                    : 1f;
        }

        direccion.Normalize();

        transform.position +=
            (Vector3)(
                direccion *
                velocidadPatrulla *
                Time.deltaTime
            );
    }

    private void PerseguirJugador()
    {
        direccion =
            (
                (Vector2)jugador.position -
                (Vector2)transform.position
            ).normalized;

        transform.position +=
            (Vector3)(
                direccion *
                velocidadPersecucion *
                Time.deltaTime
            );
    }

    private void EmpezarEmbestida()
    {
        if (jugador == null)
        {
            return;
        }

        embistiendo = true;

        tiempoEmbestidaRestante =
            duracionEmbestida;

        contadorEmbestida =
            tiempoEntreEmbestidas;

        direccionEmbestida =
            (
                (Vector2)jugador.position -
                (Vector2)transform.position
            ).normalized;

        direccion =
            direccionEmbestida;
    }

    private void ActualizarEmbestida()
    {
        tiempoEmbestidaRestante -=
            Time.deltaTime;

        transform.position +=
            (Vector3)(
                direccionEmbestida *
                velocidadEmbestida *
                Time.deltaTime
            );

        if (tiempoEmbestidaRestante <= 0f)
        {
            embistiendo = false;
            contadorAnimacion = 0f;
        }
    }

    private void RevisarWeakSpots()
    {
        bool bocaDentro =
            BocaDentroDeCollider(weakSpotCola) ||
            BocaDentroDeCollider(weakSpotAleta);

        if (
            bocaDentro &&
            !bocaEstabaDentro &&
            contadorGolpe <= 0f
        )
        {
            RecibirGolpe();
        }

        bocaEstabaDentro =
            bocaDentro;
    }

    private bool BocaDentroDeCollider(
        BoxCollider2D weakSpot
    )
    {
        if (weakSpot == null)
        {
            return false;
        }

        Vector2 centro =
            weakSpot.transform.TransformPoint(
                weakSpot.offset
            );

        Vector3 escala =
            weakSpot.transform.lossyScale;

        Vector2 tamano =
            new Vector2(
                weakSpot.size.x *
                Mathf.Abs(escala.x),
                weakSpot.size.y *
                Mathf.Abs(escala.y)
            );

        Collider2D[] contactos =
            Physics2D.OverlapBoxAll(
                centro,
                tamano,
                weakSpot.transform.eulerAngles.z
            );

        foreach (Collider2D contacto in contactos)
        {
            if (contacto.CompareTag("Mouth"))
            {
                return true;
            }
        }

        return false;
    }

    private void RecibirGolpe()
    {
        contadorGolpe =
            tiempoEntreGolpes;

        golpesRecibidos++;

        PlayerController playerController =
            jugador != null
                ? jugador.GetComponent<PlayerController>()
                : null;

        if (playerController != null)
        {
            playerController.Comer();
        }

        if (damageFlash != null)
        {
            damageFlash.MostrarDanio();
        }

        if (
            golpesRecibidos >=
            golpesNecesarios
        )
        {
            Morir();
        }
    }

    private Transform BuscarPezCercano()
    {
        GameObject[] peces =
            GameObject.FindGameObjectsWithTag(
                "Fish"
            );

        Transform mejorPez = null;

        float mejorDistancia =
            distanciaBuscarPeces;

        foreach (GameObject pez in peces)
        {
            float distancia =
                Vector2.Distance(
                    transform.position,
                    pez.transform.position
                );

            if (distancia < mejorDistancia)
            {
                mejorDistancia =
                    distancia;

                mejorPez =
                    pez.transform;
            }
        }

        return mejorPez;
    }

    private void PerseguirPez(
        Transform pez
    )
    {
        direccion =
            (
                (Vector2)pez.position -
                (Vector2)transform.position
            ).normalized;

        transform.position +=
            (Vector3)(
                direccion *
                velocidadPatrulla *
                Time.deltaTime
            );
    }

    private void ComerPeces()
    {
        Collider2D[] objetos =
            Physics2D.OverlapCircleAll(
                transform.position,
                distanciaComerPez
            );

        foreach (Collider2D objeto in objetos)
        {
            if (objeto.CompareTag("Fish"))
            {
                Destroy(objeto.gameObject);
                return;
            }
        }
    }

    private void ControlarAltura()
    {
        Vector3 posicion =
            transform.position;

        float limiteSuperior =
            nivelAgua -
            margenSuperficie;

        if (posicion.y > limiteSuperior)
        {
            posicion.y =
                limiteSuperior;

            direccion.y =
                -Mathf.Abs(
                    direccion.y
                );
        }

        if (posicion.y < limiteInferiorY)
        {
            posicion.y =
                limiteInferiorY;

            direccion.y =
                Mathf.Abs(
                    direccion.y
                );
        }

        transform.position =
            posicion;
    }

    private void Animar()
    {
        if (spriteRenderer == null)
        {
            return;
        }

        if (embistiendo)
        {
            if (spriteAtaque != null)
            {
                spriteRenderer.sprite =
                    spriteAtaque;
            }

            return;
        }

        if (
            spritesNado == null ||
            spritesNado.Length == 0
        )
        {
            return;
        }

        contadorAnimacion +=
            Time.deltaTime;

        if (
            contadorAnimacion <
            velocidadAnimacion
        )
        {
            return;
        }

        contadorAnimacion = 0f;

        frameActual++;

        if (
            frameActual >=
            spritesNado.Length
        )
        {
            frameActual = 0;
        }

        spriteRenderer.sprite =
            spritesNado[frameActual];
    }

    private void ActualizarDireccionVisual()
    {
        if (spriteRenderer == null)
        {
            return;
        }

        if (direccion.x > 0.05f)
        {
            spriteRenderer.flipX = true;

            if (weakSpots != null)
            {
                weakSpots.localScale =
                    new Vector3(
                        -Mathf.Abs(
                            escalaWeakSpotsInicial.x
                        ),
                        escalaWeakSpotsInicial.y,
                        escalaWeakSpotsInicial.z
                    );
            }
        }
        else if (direccion.x < -0.05f)
        {
            spriteRenderer.flipX = false;

            if (weakSpots != null)
            {
                weakSpots.localScale =
                    new Vector3(
                        Mathf.Abs(
                            escalaWeakSpotsInicial.x
                        ),
                        escalaWeakSpotsInicial.y,
                        escalaWeakSpotsInicial.z
                    );
            }
        }
    }

    private void Morir()
    {
        if (muerta)
        {
            return;
        }

        muerta = true;

        if (GameManager.Instance != null)
        {
            GameManager.Instance.SumarPuntos(
                puntosAlMorir
            );
        }

        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.ReproducirComer();
        }

        Destroy(gameObject, 0.15f);
    }

    private void OnTriggerEnter2D(
        Collider2D other
    )
    {
        if (
            muerta ||
            !other.CompareTag("Player") ||
            contadorAtaque > 0f
        )
        {
            return;
        }

        if (playerHealth == null)
        {
            playerHealth =
                other.GetComponent<PlayerHealth>();
        }

        if (playerHealth != null)
        {
            playerHealth.RecibirDanio(
                danoJugador
            );
        }

        contadorAtaque =
            tiempoEntreAtaques;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(
            transform.position,
            distanciaDeteccionJugador
        );

        Gizmos.DrawWireSphere(
            transform.position,
            distanciaEmbestida
        );

        Gizmos.DrawWireSphere(
            transform.position,
            distanciaComerPez
        );
    }
}