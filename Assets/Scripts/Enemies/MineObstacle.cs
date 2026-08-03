using UnityEngine;

public class MineObstacle : MonoBehaviour
{
    [SerializeField] private int danio = 1;
    [SerializeField] private GameObject explosionPrefab;
    [SerializeField] private float tiempoAntesDeDestruir = 0.05f;

    private bool explotando;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (explotando || !collision.CompareTag("Player"))
        {
            return;
        }

        PlayerHealth saludJugador = collision.GetComponent<PlayerHealth>();

        if (saludJugador == null)
        {
            return;
        }

        explotando = true;
        saludJugador.RecibirDanio(danio);

        if (explosionPrefab != null)
        {
            Instantiate(
                explosionPrefab,
                transform.position,
                Quaternion.identity
            );
        }

        Collider2D colisionMina = GetComponent<Collider2D>();

        if (colisionMina != null)
        {
            colisionMina.enabled = false;
        }

        SpriteRenderer spriteMina = GetComponent<SpriteRenderer>();

        if (spriteMina != null)
        {
            spriteMina.enabled = false;
        }

        Destroy(gameObject, tiempoAntesDeDestruir);
    }
}