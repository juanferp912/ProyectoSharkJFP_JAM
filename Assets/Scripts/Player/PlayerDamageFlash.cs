using System.Collections;
using UnityEngine;

public class PlayerDamageFlash : MonoBehaviour
{
    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private float duracionFlash = 0.12f;
    [SerializeField] private int cantidadFlashes = 3;

    private Color colorOriginal;
    private int vidasAnteriores;
    private Coroutine rutinaFlash;

    private void Start()
    {
        if (playerHealth == null)
        {
            playerHealth =
                GetComponentInParent<PlayerHealth>();
        }

        if (spriteRenderer == null)
        {
            spriteRenderer =
                GetComponent<SpriteRenderer>();
        }

        if (spriteRenderer != null)
        {
            colorOriginal =
                spriteRenderer.color;
        }

        if (playerHealth != null)
        {
            vidasAnteriores =
                playerHealth.VidasActuales;

            playerHealth.VidasCambiadas +=
                RevisarDanio;
        }
    }

    private void OnDestroy()
    {
        if (playerHealth != null)
        {
            playerHealth.VidasCambiadas -=
                RevisarDanio;
        }
    }

    private void RevisarDanio(int vidasNuevas)
    {
        if (vidasNuevas < vidasAnteriores)
        {
            if (rutinaFlash != null)
            {
                StopCoroutine(rutinaFlash);
            }

            rutinaFlash =
                StartCoroutine(HacerFlash());
        }

        vidasAnteriores =
            vidasNuevas;
    }

    private IEnumerator HacerFlash()
    {
        for (int i = 0; i < cantidadFlashes; i++)
        {
            spriteRenderer.color =
                new Color(
                    1f,
                    0.25f,
                    0.25f,
                    1f
                );

            yield return new WaitForSeconds(
                duracionFlash
            );

            spriteRenderer.color =
                colorOriginal;

            yield return new WaitForSeconds(
                duracionFlash
            );
        }

        spriteRenderer.color =
            colorOriginal;
    }
}