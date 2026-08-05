using UnityEngine;

public class UIPulse : MonoBehaviour
{
    [SerializeField] private float escalaMinima = 0.95f;
    [SerializeField] private float escalaMaxima = 1.05f;
    [SerializeField] private float velocidad = 2f;

    private Vector3 escalaInicial;

    private void Awake()
    {
        escalaInicial = transform.localScale;
    }

    private void Update()
    {
        float valor =
            Mathf.Lerp(
                escalaMinima,
                escalaMaxima,
                (Mathf.Sin(Time.unscaledTime * velocidad) + 1f) * 0.5f
            );

        transform.localScale = escalaInicial * valor;
    }
}