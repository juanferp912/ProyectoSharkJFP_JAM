using System;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private int vidasMaximas = 3;

    private int vidasActuales;
    private bool derrotado;

    public int VidasActuales => vidasActuales;

    public event Action<int> VidasCambiadas;
    public event Action JugadorDerrotado;

    private void Awake()
    {
        vidasActuales = vidasMaximas;
    }

    private void Start()
    {
        VidasCambiadas?.Invoke(vidasActuales);
    }

    public void RecibirDanio(int cantidad)
    {
        if (derrotado || cantidad <= 0)
        {
            return;
        }

        vidasActuales -= cantidad;
        vidasActuales = Mathf.Max(vidasActuales, 0);

        VidasCambiadas?.Invoke(vidasActuales);

        if (vidasActuales == 0)
        {
            derrotado = true;
            JugadorDerrotado?.Invoke();
        }
    }
}