using UnityEngine;

public class SpawnerManager : MonoBehaviour
{
    [Header("Prefabs")]
    [SerializeField] private GameObject[] pecesPrefabs;

    [Header("Cantidad")]
    [SerializeField] private int cantidadInicial = 10;
    [SerializeField] private int cantidadMaxima = 15;

    [Header("Generación")]
    [SerializeField] private float tiempoEntreGeneraciones = 3f;
    [SerializeField] private Vector2 limiteMinimo = new Vector2(-8f, -4f);
    [SerializeField] private Vector2 limiteMaximo = new Vector2(8f, 4f);

    private float contador;

    private void Start()
    {
        for (int i = 0; i < cantidadInicial; i++)
        {
            GenerarPez();
        }
    }

    private void Update()
    {
        contador -= Time.deltaTime;

        if (contador <= 0f)
        {
            int pecesActuales = GameObject.FindGameObjectsWithTag("Fish").Length;

            if (pecesActuales < cantidadMaxima)
            {
                GenerarPez();
            }

            contador = tiempoEntreGeneraciones;
        }
    }

    private void GenerarPez()
    {
        if (pecesPrefabs.Length == 0)
        {
            return;
        }

        float posicionX = Random.Range(limiteMinimo.x, limiteMaximo.x);
        float posicionY = Random.Range(limiteMinimo.y, limiteMaximo.y);

        Vector2 posicion = new Vector2(posicionX, posicionY);

        int indiceAleatorio = Random.Range(0, pecesPrefabs.Length);

        Instantiate(
            pecesPrefabs[indiceAleatorio],
            posicion,
            Quaternion.identity
        );
    }
}