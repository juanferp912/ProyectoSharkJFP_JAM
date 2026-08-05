using System.Collections;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class UIFade : MonoBehaviour
{
    [SerializeField] private float duracion = 0.35f;
    [SerializeField] private bool aparecerAlIniciar = true;

    private CanvasGroup canvasGroup;
    private Coroutine animacionActual;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    private void Start()
    {
        if (aparecerAlIniciar)
        {
            Mostrar();
        }
    }

    public void Mostrar()
    {
        IniciarAnimacion(1f, true);
    }

    public void Ocultar()
    {
        IniciarAnimacion(0f, false);
    }

    private void IniciarAnimacion(float objetivo, bool interactuable)
    {
        if (animacionActual != null)
        {
            StopCoroutine(animacionActual);
        }

        gameObject.SetActive(true);
        animacionActual = StartCoroutine(
            CambiarTransparencia(objetivo, interactuable)
        );
    }

    private IEnumerator CambiarTransparencia(
        float objetivo,
        bool interactuable
    )
    {
        float inicial = canvasGroup.alpha;
        float tiempo = 0f;

        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;

        while (tiempo < duracion)
        {
            tiempo += Time.unscaledDeltaTime;

            canvasGroup.alpha = Mathf.Lerp(
                inicial,
                objetivo,
                tiempo / duracion
            );

            yield return null;
        }

        canvasGroup.alpha = objetivo;
        canvasGroup.interactable = interactuable;
        canvasGroup.blocksRaycasts = interactuable;

        if (objetivo <= 0f)
        {
            gameObject.SetActive(false);
        }

        animacionActual = null;
    }
}