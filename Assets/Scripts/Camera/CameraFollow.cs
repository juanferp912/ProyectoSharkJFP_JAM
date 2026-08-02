using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform jugador;
    [SerializeField] private float suavizado = 5f;
    [SerializeField] private Vector2 limiteMinimo = new Vector2(-20f, -12f);
    [SerializeField] private Vector2 limiteMaximo = new Vector2(20f, 12f);

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

        posicionSuavizada.x = Mathf.Clamp(
            posicionSuavizada.x,
            limiteMinimo.x,
            limiteMaximo.x
        );

        posicionSuavizada.y = Mathf.Clamp(
            posicionSuavizada.y,
            limiteMinimo.y,
            limiteMaximo.y
        );

        transform.position = posicionSuavizada;
    }
}