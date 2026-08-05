using UnityEngine;

public class PlatformObject : MonoBehaviour
{
    [SerializeField] private float tiempoDeVida = 40f;

    private void Start()
    {
        if (tiempoDeVida > 0f)
        {
            Destroy(gameObject, tiempoDeVida);
        }
    }
}