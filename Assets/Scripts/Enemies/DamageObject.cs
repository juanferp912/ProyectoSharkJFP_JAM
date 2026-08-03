using UnityEngine;

public class DamageObject : MonoBehaviour
{
    [SerializeField] private int danio = 1;
    [SerializeField] private bool destruirAlGolpear = true;

    private bool golpeRealizado;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (golpeRealizado || !collision.CompareTag("Player"))
        {
            return;
        }

        PlayerHealth saludJugador = collision.GetComponent<PlayerHealth>();

        if (saludJugador == null)
        {
            return;
        }

        golpeRealizado = true;
        saludJugador.RecibirDanio(danio);

        if (destruirAlGolpear)
        {
            Destroy(gameObject);
        }
    }
}