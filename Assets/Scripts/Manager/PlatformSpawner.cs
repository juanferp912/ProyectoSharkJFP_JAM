using System.Collections.Generic;
using UnityEngine;

public class PlatformSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] plataformasPrefabs;
    [SerializeField] private Transform jugador;

    [Header("Piso")]
    [SerializeField] private float alturaPiso = -17f;
    [SerializeField] private float escalaX = 3f;
    [SerializeField] private float distanciaGeneracion = 45f;
    [SerializeField] private float distanciaEliminacion = 60f;
    [SerializeField] private float superposicion = 0.05f;

    [Header("Collider")]
    [SerializeField] private bool crearCollider = true;
    [SerializeField] private float alturaCollider = 0.8f;
    [SerializeField] private float ajusteColliderY = 0.3f;

    private readonly List<GameObject> plataformas =
        new List<GameObject>();

    private float extremoIzquierdo;
    private float extremoDerecho;

    private BoxCollider2D colliderSuelo;

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

        if (jugador == null)
        {
            return;
        }

        extremoIzquierdo = jugador.position.x;
        extremoDerecho = jugador.position.x;

        if (crearCollider)
        {
            CrearColliderGeneral();
        }

        GenerarPisoInicial();
    }

    private void Update()
    {
        if (jugador == null)
        {
            return;
        }

        GenerarHaciaDerecha();
        GenerarHaciaIzquierda();
        EliminarLejanas();

        if (crearCollider)
        {
            ActualizarCollider();
        }
    }

    private void GenerarPisoInicial()
    {
        GenerarHaciaDerecha();
        GenerarHaciaIzquierda();
    }

    private void GenerarHaciaDerecha()
    {
        while (
            extremoDerecho <
            jugador.position.x + distanciaGeneracion
        )
        {
            CrearDerecha();
        }
    }

    private void GenerarHaciaIzquierda()
    {
        while (
            extremoIzquierdo >
            jugador.position.x - distanciaGeneracion
        )
        {
            CrearIzquierda();
        }
    }

    private void CrearDerecha()
    {
        GameObject prefab = ObtenerPrefab();

        if (prefab == null)
        {
            return;
        }

        GameObject plataforma = Instantiate(
            prefab,
            new Vector3(
                0f,
                alturaPiso,
                0f
            ),
            Quaternion.identity,
            transform
        );

        Vector3 escalaOriginal =
            plataforma.transform.localScale;

        plataforma.transform.localScale =
            new Vector3(
                escalaOriginal.x * escalaX,
                escalaOriginal.y,
                escalaOriginal.z
            );

        SpriteRenderer renderer =
            plataforma.GetComponentInChildren<SpriteRenderer>();

        if (renderer == null)
        {
            Destroy(plataforma);
            return;
        }

        Bounds bounds =
            renderer.bounds;

        float desplazamiento =
            extremoDerecho -
            bounds.min.x -
            superposicion;

        plataforma.transform.position +=
            new Vector3(
                desplazamiento,
                0f,
                0f
            );

        extremoDerecho =
            renderer.bounds.max.x;

        plataformas.Add(plataforma);
    }

    private void CrearIzquierda()
    {
        GameObject prefab = ObtenerPrefab();

        if (prefab == null)
        {
            return;
        }

        GameObject plataforma = Instantiate(
            prefab,
            new Vector3(
                0f,
                alturaPiso,
                0f
            ),
            Quaternion.identity,
            transform
        );

        Vector3 escalaOriginal =
            plataforma.transform.localScale;

        plataforma.transform.localScale =
            new Vector3(
                escalaOriginal.x * escalaX,
                escalaOriginal.y,
                escalaOriginal.z
            );

        SpriteRenderer renderer =
            plataforma.GetComponentInChildren<SpriteRenderer>();

        if (renderer == null)
        {
            Destroy(plataforma);
            return;
        }

        Bounds bounds =
            renderer.bounds;

        float desplazamiento =
            extremoIzquierdo -
            bounds.max.x +
            superposicion;

        plataforma.transform.position +=
            new Vector3(
                desplazamiento,
                0f,
                0f
            );

        extremoIzquierdo =
            renderer.bounds.min.x;

        plataformas.Add(plataforma);
    }

    private GameObject ObtenerPrefab()
    {
        if (
            plataformasPrefabs == null ||
            plataformasPrefabs.Length == 0
        )
        {
            return null;
        }

        return plataformasPrefabs[
            Random.Range(
                0,
                plataformasPrefabs.Length
            )
        ];
    }

    private void EliminarLejanas()
    {
        for (
            int i = plataformas.Count - 1;
            i >= 0;
            i--
        )
        {
            GameObject plataforma =
                plataformas[i];

            if (plataforma == null)
            {
                plataformas.RemoveAt(i);
                continue;
            }

            float distancia =
                Mathf.Abs(
                    plataforma.transform.position.x -
                    jugador.position.x
                );

            if (
                distancia >
                distanciaEliminacion
            )
            {
                Destroy(plataforma);
                plataformas.RemoveAt(i);
            }
        }

        RecalcularExtremos();
    }

    private void RecalcularExtremos()
    {
        if (plataformas.Count == 0)
        {
            extremoIzquierdo =
                jugador.position.x;

            extremoDerecho =
                jugador.position.x;

            return;
        }

        extremoIzquierdo =
            float.MaxValue;

        extremoDerecho =
            float.MinValue;

        foreach (
            GameObject plataforma
            in plataformas
        )
        {
            if (plataforma == null)
            {
                continue;
            }

            SpriteRenderer renderer =
                plataforma.GetComponentInChildren<SpriteRenderer>();

            if (renderer == null)
            {
                continue;
            }

            extremoIzquierdo =
                Mathf.Min(
                    extremoIzquierdo,
                    renderer.bounds.min.x
                );

            extremoDerecho =
                Mathf.Max(
                    extremoDerecho,
                    renderer.bounds.max.x
                );
        }
    }

    private void CrearColliderGeneral()
    {
        colliderSuelo =
            GetComponent<BoxCollider2D>();

        if (colliderSuelo == null)
        {
            colliderSuelo =
                gameObject.AddComponent<BoxCollider2D>();
        }

        colliderSuelo.isTrigger = false;
    }

    private void ActualizarCollider()
    {
        if (colliderSuelo == null)
        {
            return;
        }

        float ancho =
            distanciaGeneracion * 2f + 20f;

        colliderSuelo.size =
            new Vector2(
                ancho,
                alturaCollider
            );

        colliderSuelo.offset =
            new Vector2(
                jugador.position.x -
                transform.position.x,
                alturaPiso +
                ajusteColliderY -
                transform.position.y
            );
    }
}