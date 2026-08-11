using UnityEngine;

public class CieloInfinito : MonoBehaviour
{
    [SerializeField] private Transform camara;
    [SerializeField] private SpriteRenderer cieloA;
    [SerializeField] private SpriteRenderer cieloB;
    [SerializeField] private SpriteRenderer cieloC;
    [SerializeField] private float factorParallax = 0.08f;
    [SerializeField] private float superposicion = 0.05f;

    private float ancho;
    private float camaraXInicial;

    private float alturaA;
    private float alturaB;
    private float alturaC;

    private float profundidadA;
    private float profundidadB;
    private float profundidadC;

    private void Start()
    {
        if (camara == null && Camera.main != null)
        {
            camara = Camera.main.transform;
        }

        if (
            camara == null ||
            cieloA == null ||
            cieloB == null ||
            cieloC == null
        )
        {
            enabled = false;
            return;
        }

        ancho = cieloA.bounds.size.x - superposicion;

        camaraXInicial = camara.position.x;

        alturaA = cieloA.transform.position.y;
        alturaB = cieloB.transform.position.y;
        alturaC = cieloC.transform.position.y;

        profundidadA = cieloA.transform.position.z;
        profundidadB = cieloB.transform.position.z;
        profundidadC = cieloC.transform.position.z;

        ActualizarCielo();
    }

    private void LateUpdate()
    {
        ActualizarCielo();
    }

    private void ActualizarCielo()
    {
        float recorrido =
            camara.position.x - camaraXInicial;

        float desplazamientoParallax =
            recorrido * factorParallax;

        float centroDeseado =
            camara.position.x - desplazamientoParallax;

        int indiceCentro =
            Mathf.RoundToInt(
                centroDeseado / ancho
            );

        float centro =
            indiceCentro * ancho;

        Posicionar(
            cieloA,
            centro - ancho,
            indiceCentro - 1,
            alturaA,
            profundidadA
        );

        Posicionar(
            cieloB,
            centro,
            indiceCentro,
            alturaB,
            profundidadB
        );

        Posicionar(
            cieloC,
            centro + ancho,
            indiceCentro + 1,
            alturaC,
            profundidadC
        );
    }

    private void Posicionar(
        SpriteRenderer cielo,
        float x,
        int indice,
        float y,
        float z
    )
    {
        cielo.transform.position =
            new Vector3(
                x,
                y,
                z
            );

        cielo.flipX =
            Mathf.Abs(indice) % 2 == 1;
    }
}