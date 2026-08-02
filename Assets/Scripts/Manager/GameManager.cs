using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private int puntuacion;

    public int Puntuacion => puntuacion;

    public event Action<int> PuntuacionCambiada;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void SumarPuntos(int cantidad)
    {
        puntuacion += cantidad;
        PuntuacionCambiada?.Invoke(puntuacion);
    }
}