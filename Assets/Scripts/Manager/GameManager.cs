using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private int puntuacion;
    [SerializeField] private int puntosPorVida = 25;

    public int Puntuacion => puntuacion;

    public event Action<int> PuntuacionCambiada;

    private PlayerHealth playerHealth;
    private int vidasExtrasEntregadas;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void Start()
    {
        BuscarPlayerHealth();

        vidasExtrasEntregadas =
            puntuacion / puntosPorVida;

        PuntuacionCambiada?.Invoke(
            puntuacion
        );
    }

    public void SumarPuntos(int cantidad)
    {
        if (cantidad <= 0)
        {
            return;
        }

        puntuacion += cantidad;

        PuntuacionCambiada?.Invoke(
            puntuacion
        );

        RevisarVidasExtra();
    }

    private void RevisarVidasExtra()
    {
        if (puntosPorVida <= 0)
        {
            return;
        }

        if (playerHealth == null)
        {
            BuscarPlayerHealth();
        }

        if (playerHealth == null)
        {
            return;
        }

        int vidasQueCorresponden =
            puntuacion / puntosPorVida;

        while (
            vidasExtrasEntregadas <
            vidasQueCorresponden
        )
        {
            playerHealth.AgregarVida(1);

            vidasExtrasEntregadas++;
        }
    }

    private void BuscarPlayerHealth()
    {
        GameObject player =
            GameObject.FindGameObjectWithTag(
                "Player"
            );

        if (player != null)
        {
            playerHealth =
                player.GetComponent<PlayerHealth>();
        }
    }
}