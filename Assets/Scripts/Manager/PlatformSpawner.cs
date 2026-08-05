using System.Collections.Generic;
using UnityEngine;

public class PlatformSpawner : MonoBehaviour
{
    [Header("Prefabs")]
    [SerializeField] private GameObject[] plataformasPrefabs;

    [Header("Jugador")]
    [SerializeField] private Transform jugador;
    [SerializeField] private float distanciaMinimaJugador = 5f;

    [Header("Cantidad")]
    [SerializeField] private int cantidadInicial = 12;
    [SerializeField] private int cantidadMaxima = 20;

    [Header("Generación")]
    [SerializeField] private float tiempoEntreGeneraciones = 4f;
    [SerializeField] private Vector2 limiteMinimo = new Vector2(-25f, -12f);
    [SerializeField] private Vector2 limiteMaximo = new Vector2(25f, 12f);

    [Header("Separación")]
    [SerializeField] private float distanciaMinimaEntrePlataformas = 3f;
    [SerializeField] private int intentosMaximos = 25;

    private readonly List<GameObject> plataformasGeneradas = new List<GameObject>();
    private float contador;

    private void Start()
    {
        contador = tiempoEntreGeneraciones;

        for (int i = 0; i < cantidadInicial; i++)
        {
            GenerarPlataforma();
        }
    }

    private void Update()
    {
        LimpiarReferencias();

        contador -= Time.deltaTime;

        if (contador > 0f)
        {
            return;
        }

        if (plataformasGeneradas.Count < cantidadMaxima)
        {
            GenerarPlataforma();
        }

        contador = tiempoEntreGeneraciones;
    }

    private void GenerarPlataforma()
    {
        if (plataformasPrefabs == null || plataformasPrefabs.Length == 0)
        {
            return;
        }

        Vector2 posicion;

        if (!BuscarPosicionValida(out posicion))
        {
            return;
        }

        int indice = Random.Range(0, plataformasPrefabs.Length);

        GameObject plataforma = Instantiate(
            plataformasPrefabs[indice],
            posicion,
            Quaternion.identity
        );

        plataformasGeneradas.Add(plataforma);
    }

    private bool BuscarPosicionValida(out Vector2 posicion)
    {
        for (int intento = 0; intento < intentosMaximos; intento++)
        {
            posicion = new Vector2(
                Random.Range(limiteMinimo.x, limiteMaximo.x),
                Random.Range(limiteMinimo.y, limiteMaximo.y)
            );

            if (jugador != null &&
                Vector2.Distance(posicion, jugador.position) < distanciaMinimaJugador)
            {
                continue;
            }

            if (EstaCercaDeOtraPlataforma(posicion))
            {
                continue;
            }

            return true;
        }

        posicion = Vector2.zero;
        return false;
    }

    private bool EstaCercaDeOtraPlataforma(Vector2 posicion)
    {
        foreach (GameObject plataforma in plataformasGeneradas)
        {
            if (plataforma == null)
            {
                continue;
            }

            if (Vector2.Distance(posicion, plataforma.transform.position) <
                distanciaMinimaEntrePlataformas)
            {
                return true;
            }
        }

        return false;
    }

    private void LimpiarReferencias()
    {
        plataformasGeneradas.RemoveAll(plataforma => plataforma == null);
    }
}