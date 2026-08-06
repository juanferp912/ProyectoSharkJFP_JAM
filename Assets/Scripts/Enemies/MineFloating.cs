using UnityEngine;

public class MineFloating : MonoBehaviour
{
    [SerializeField] private float altura = 0.15f;
    [SerializeField] private float velocidad = 1.5f;

    [Header("Agua")]
    [SerializeField] private float nivelSuperficieAgua = 8f;
    [SerializeField] private float margenSuperficie = 1.5f;
    [SerializeField] private float limiteInferiorY = -13f;

    private Vector3 posicionInicial;

    private void Start()
    {
        float limiteSuperior =
            nivelSuperficieAgua - margenSuperficie;

        Vector3 posicion = transform.position;

        posicion.y = Mathf.Clamp(
            posicion.y,
            limiteInferiorY + altura,
            limiteSuperior - altura
        );

        transform.position = posicion;
        posicionInicial = posicion;
    }

    private void Update()
    {
        float desplazamiento =
            Mathf.Sin(Time.time * velocidad) * altura;

        float nuevaY =
            posicionInicial.y + desplazamiento;

        float limiteSuperior =
            nivelSuperficieAgua - margenSuperficie;

        nuevaY = Mathf.Clamp(
            nuevaY,
            limiteInferiorY,
            limiteSuperior
        );

        transform.position = new Vector3(
            posicionInicial.x,
            nuevaY,
            posicionInicial.z
        );
    }
}