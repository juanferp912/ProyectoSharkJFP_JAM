using UnityEngine;

public class DestroyAfterTime : MonoBehaviour
{
    [SerializeField] private float tiempoDeVida = 1f;

    private void Start()
    {
        Destroy(gameObject, tiempoDeVida);
    }
}