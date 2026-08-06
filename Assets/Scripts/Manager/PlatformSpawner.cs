using UnityEngine;

public class PlatformSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] plataformasPrefabs;
    [SerializeField] private float limiteIzquierdo = -25f;
    [SerializeField] private float limiteDerecho = 25f;
    [SerializeField] private float alturaPiso = -17f;
    [SerializeField] private float superposicion = 0.05f;
    [SerializeField] private float alturaCollider = 0.8f;
    [SerializeField] private float ajusteColliderY = 0.3f;

    private void Start()
    {
        GenerarPiso();
        CrearColliderGeneral();
    }

    private void GenerarPiso()
    {
        if (plataformasPrefabs == null || plataformasPrefabs.Length == 0)
        {
            return;
        }

        float posicionX = limiteIzquierdo;

        while (posicionX < limiteDerecho)
        {
            GameObject prefab = plataformasPrefabs[
                Random.Range(0, plataformasPrefabs.Length)
            ];

            GameObject plataforma = Instantiate(
                prefab,
                Vector3.zero,
                Quaternion.identity,
                transform
            );

            SpriteRenderer spriteRenderer =
                plataforma.GetComponentInChildren<SpriteRenderer>();

            if (spriteRenderer == null)
            {
                Destroy(plataforma);
                break;
            }

            float ancho = spriteRenderer.bounds.size.x;

            plataforma.transform.position = new Vector3(
                posicionX + ancho / 2f,
                alturaPiso,
                0f
            );

            posicionX += ancho - superposicion;
        }
    }

    private void CrearColliderGeneral()
    {
        BoxCollider2D colliderExistente =
            GetComponent<BoxCollider2D>();

        if (colliderExistente != null)
        {
            Destroy(colliderExistente);
        }

        BoxCollider2D colliderSuelo =
            gameObject.AddComponent<BoxCollider2D>();

        float anchoTotal =
            limiteDerecho - limiteIzquierdo;

        float centroX =
            (limiteIzquierdo + limiteDerecho) / 2f;

        colliderSuelo.size = new Vector2(
            anchoTotal,
            alturaCollider
        );

        colliderSuelo.offset = new Vector2(
            centroX,
            alturaPiso + ajusteColliderY
        );

        colliderSuelo.isTrigger = false;
    }
}