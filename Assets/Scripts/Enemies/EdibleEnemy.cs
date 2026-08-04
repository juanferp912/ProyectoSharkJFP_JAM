using UnityEngine;

public class EdibleEnemy : MonoBehaviour
{
    [SerializeField] private int puntos = 20;

    private bool comido;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (comido)
        {
            return;
        }

        if (collision.CompareTag("Mouth"))
        {
            comido = true;

            if (GameManager.Instance != null)
            {
                GameManager.Instance.SumarPuntos(puntos);
            }

            Destroy(gameObject);
        }
    }
}