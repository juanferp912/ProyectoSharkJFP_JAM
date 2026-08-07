using UnityEngine;

public class FollowPlayerX : MonoBehaviour
{
    [SerializeField] private Transform jugador;
    [SerializeField] private bool seguirX = true;

    private float posicionInicialY;
    private float posicionInicialZ;

    private void Start()
    {
        posicionInicialY = transform.position.y;
        posicionInicialZ = transform.position.z;

        if (jugador == null)
        {
            GameObject player =
                GameObject.FindGameObjectWithTag("Player");

            if (player != null)
            {
                jugador = player.transform;
            }
        }
    }

    private void LateUpdate()
    {
        if (jugador == null || !seguirX)
        {
            return;
        }

        transform.position = new Vector3(
            jugador.position.x,
            posicionInicialY,
            posicionInicialZ
        );
    }
}