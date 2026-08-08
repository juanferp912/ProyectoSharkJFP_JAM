using System.Collections;
using UnityEngine;

public class OrcaDamageFlash : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Color colorDanio = Color.red;
    [SerializeField] private float duracionFlash = 0.1f;
    [SerializeField] private int cantidadFlashes = 3;

    private Color colorOriginal;
    private Coroutine rutina;

    private void Start()
    {
        if (spriteRenderer == null)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        if (spriteRenderer != null)
        {
            colorOriginal = spriteRenderer.color;
        }
    }

    public void MostrarDanio()
    {
        if (spriteRenderer == null)
        {
            return;
        }

        if (rutina != null)
        {
            StopCoroutine(rutina);
        }

        rutina = StartCoroutine(Flash());
    }

    private IEnumerator Flash()
    {
        for (int i = 0; i < cantidadFlashes; i++)
        {
            spriteRenderer.color = colorDanio;

            yield return new WaitForSeconds(
                duracionFlash
            );

            spriteRenderer.color = colorOriginal;

            yield return new WaitForSeconds(
                duracionFlash
            );
        }

        spriteRenderer.color = colorOriginal;
        rutina = null;
    }
}