using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform jugador;
    [SerializeField] private float suavizado = 5f;

    [Header("Limites verticales")]
    [SerializeField] private float limiteMinimoY = -8f;
    [SerializeField] private float limiteMaximoY = 11f;

    private void LateUpdate()
    {
        if (jugador == null)
        {
            return;
        }

        Vector3 posicionObjetivo = new Vector3(
            jugador.position.x,
            jugador.position.y,
            transform.position.z
        );

        Vector3 posicionSuavizada = Vector3.Lerp(
            transform.position,
            posicionObjetivo,
            suavizado * Time.deltaTime
        );

        posicionSuavizada.y = Mathf.Clamp(
            posicionSuavizada.y,
            limiteMinimoY,
            limiteMaximoY
        );

        transform.position = posicionSuavizada;
    }
}