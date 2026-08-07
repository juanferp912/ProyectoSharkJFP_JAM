using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    [SerializeField] private Transform jugador;
    [SerializeField] private float factorParallax = 0.6f;
    [SerializeField] private float altura = 16f;

    private float jugadorXAnterior;

    private void Start()
    {
        if (jugador == null)
        {
            GameObject player =
                GameObject.FindGameObjectWithTag("Player");

            if (player != null)
            {
                jugador = player.transform;
            }
        }

        if (jugador != null)
        {
            jugadorXAnterior =
                jugador.position.x;
        }
    }

    private void LateUpdate()
    {
        if (jugador == null)
        {
            return;
        }

        float deltaX =
            jugador.position.x -
            jugadorXAnterior;

        float nuevaX =
            transform.position.x +
            deltaX *
            factorParallax;

        transform.position =
            new Vector3(
                nuevaX,
                altura,
                transform.position.z
            );

        jugadorXAnterior =
            jugador.position.x;
    }
}