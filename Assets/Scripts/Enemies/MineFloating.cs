using UnityEngine;

public class MineFloating : MonoBehaviour
{
    [SerializeField] private float altura = 0.15f;
    [SerializeField] private float velocidad = 1.5f;

    private Vector3 posicionInicial;

    private void Start()
    {
        posicionInicial = transform.position;
    }

    private void Update()
    {
        float desplazamiento = Mathf.Sin(Time.time * velocidad) * altura;

        transform.position = new Vector3(
            posicionInicial.x,
            posicionInicial.y + desplazamiento,
            posicionInicial.z
        );
    }
}