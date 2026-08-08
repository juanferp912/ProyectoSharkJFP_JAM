using UnityEngine;

public class DolphinAI : MonoBehaviour
{
    [Header("Sprites")]
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Sprite[] spritesNado;
    [SerializeField] private Sprite spriteSalto;

    [Header("Movimiento")]
    [SerializeField] private float velocidadNormal = 3.2f;
    [SerializeField] private float velocidadHuida = 4.8f;
    [SerializeField] private float velocidadCaza = 4f;
    [SerializeField] private float limiteInferiorY = -13f;
    [SerializeField] private float nivelAgua = 8f;
    [SerializeField] private float margenSuperficie = 0.7f;

    [Header("Inteligencia")]
    [SerializeField] private float distanciaDeteccionJugador = 5f;
    [SerializeField] private float distanciaBuscarPeces = 4.5f;
    [SerializeField] private float distanciaComerPez = 0.8f;
    [SerializeField] private float tiempoCambioDireccion = 2f;

    [Header("Salto")]
    [SerializeField] private float probabilidadSalto = 0.12f;
    [SerializeField] private float intervaloIntentoSalto = 5f;
    [SerializeField] private float alturaSalto = 2.5f;
    [SerializeField] private float duracionSalto = 1.3f;

    [Header("Animacion")]
    [SerializeField] private float velocidadAnimacion = 0.14f;

    [Header("Puntuacion")]
    [SerializeField] private int puntos = 3;

    private Transform jugador;
    private Vector2 direccion;

    private float contadorDireccion;
    private float contadorAnimacion;
    private float contadorSalto;

    private int frameActual;

    private bool saltando;
    private bool comido;

    private Vector3 inicioSalto;
    private Vector3 destinoSalto;
    private float progresoSalto;

    private void Start()
    {
        GameObject player =
            GameObject.FindGameObjectWithTag("Player");

        if (player != null)
        {
            jugador = player.transform;
        }

        if (spriteRenderer == null)
        {
            spriteRenderer =
                GetComponent<SpriteRenderer>();
        }

        ElegirDireccionAleatoria();

        contadorSalto =
            intervaloIntentoSalto;
    }

    private void Update()
    {
        if (comido)
        {
            return;
        }

        if (saltando)
        {
            ActualizarSalto();
            return;
        }

        contadorSalto -=
            Time.deltaTime;

        if (JugadorCerca())
        {
            HuirDelJugador();
        }
        else
        {
            Transform pez =
                BuscarPezCercano();

            if (pez != null)
            {
                CazarPez(pez);
            }
            else
            {
                NadarAleatoriamente();
            }
        }

        IntentarComerPeces();
        ControlarAltura();
        IntentarSaltar();
        AnimarNado();
        ActualizarDireccionVisual();
    }

    private bool JugadorCerca()
    {
        if (jugador == null)
        {
            return false;
        }

        float distancia =
            Vector2.Distance(
                transform.position,
                jugador.position
            );

        return distancia <=
            distanciaDeteccionJugador;
    }

    private void HuirDelJugador()
    {
        direccion =
            (
                (Vector2)transform.position -
                (Vector2)jugador.position
            ).normalized;

        if (Mathf.Abs(direccion.x) < 0.3f)
        {
            direccion.x =
                transform.position.x >= jugador.position.x
                    ? 0.3f
                    : -0.3f;
        }

        float limiteSuperior =
            nivelAgua -
            margenSuperficie;

        if (
            transform.position.y >=
            limiteSuperior - 0.4f &&
            direccion.y > 0f
        )
        {
            direccion.y =
                -Mathf.Abs(
                    direccion.y
                );
        }

        direccion.Normalize();

        transform.position +=
            (Vector3)(
                direccion *
                velocidadHuida *
                Time.deltaTime
            );
    }

    private void NadarAleatoriamente()
    {
        contadorDireccion -=
            Time.deltaTime;

        if (contadorDireccion <= 0f)
        {
            ElegirDireccionAleatoria();
        }

        transform.position +=
            (Vector3)(
                direccion *
                velocidadNormal *
                Time.deltaTime
            );
    }

    private void ElegirDireccionAleatoria()
    {
        direccion =
            Random.insideUnitCircle.normalized;

        if (Mathf.Abs(direccion.x) < 0.4f)
        {
            direccion.x =
                Random.value < 0.5f
                    ? -0.4f
                    : 0.4f;
        }

        direccion.Normalize();

        contadorDireccion =
            tiempoCambioDireccion;
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

    private void CazarPez(Transform pez)
    {
        direccion =
            (
                (Vector2)pez.position -
                (Vector2)transform.position
            ).normalized;

        transform.position +=
            (Vector3)(
                direccion *
                velocidadCaza *
                Time.deltaTime
            );
    }

    private void IntentarComerPeces()
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
                Destroy(
                    objeto.gameObject
                );

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

    private void IntentarSaltar()
    {
        if (contadorSalto > 0f)
        {
            return;
        }

        contadorSalto =
            intervaloIntentoSalto;

        float limiteSuperior =
            nivelAgua -
            margenSuperficie;

        bool cercaSuperficie =
            transform.position.y >
            limiteSuperior - 1.5f;

        if (
            cercaSuperficie &&
            Random.value <= probabilidadSalto &&
            !JugadorCerca()
        )
        {
            EmpezarSalto();
        }
    }

    private void EmpezarSalto()
    {
        saltando = true;
        progresoSalto = 0f;

        inicioSalto =
            new Vector3(
                transform.position.x,
                nivelAgua - 0.2f,
                transform.position.z
            );

        float direccionX =
            direccion.x >= 0f
                ? 1f
                : -1f;

        destinoSalto =
            new Vector3(
                inicioSalto.x +
                direccionX * 3.5f,
                nivelAgua - 0.2f,
                inicioSalto.z
            );

        if (
            spriteRenderer != null &&
            spriteSalto != null
        )
        {
            spriteRenderer.sprite =
                spriteSalto;
        }
    }

    private void ActualizarSalto()
    {
        progresoSalto +=
            Time.deltaTime /
            duracionSalto;

        float t =
            Mathf.Clamp01(
                progresoSalto
            );

        Vector3 posicionBase =
            Vector3.Lerp(
                inicioSalto,
                destinoSalto,
                t
            );

        float altura =
            Mathf.Sin(
                t * Mathf.PI
            ) *
            alturaSalto;

        posicionBase.y +=
            altura;

        transform.position =
            posicionBase;

        ActualizarDireccionVisual();

        if (t >= 1f)
        {
            saltando = false;

            transform.position =
                new Vector3(
                    transform.position.x,
                    nivelAgua -
                    margenSuperficie,
                    transform.position.z
                );

            ElegirDireccionAleatoria();
        }
    }

    private void AnimarNado()
    {
        if (
            spritesNado == null ||
            spritesNado.Length == 0 ||
            spriteRenderer == null
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
        }
        else if (direccion.x < -0.05f)
        {
            spriteRenderer.flipX = false;
        }
    }

    private void OnTriggerEnter2D(
        Collider2D other
    )
    {
        if (
            comido ||
            !other.CompareTag("Mouth")
        )
        {
            return;
        }

        comido = true;

        PlayerController player =
            other.GetComponentInParent<PlayerController>();

        if (player != null)
        {
            player.Comer();
        }

        if (GameManager.Instance != null)
        {
            GameManager.Instance.SumarPuntos(
                puntos
            );
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
            distanciaDeteccionJugador
        );

        Gizmos.DrawWireSphere(
            transform.position,
            distanciaBuscarPeces
        );

        Gizmos.DrawWireSphere(
            transform.position,
            distanciaComerPez
        );
    }
}