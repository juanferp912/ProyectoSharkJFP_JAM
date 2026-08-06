using UnityEngine;

public class WaterPhysics2D : MonoBehaviour
{
    [SerializeField] private float gravedadAgua = 0f;
    [SerializeField] private float gravedadAire = 2.5f;

    private Rigidbody2D rb;
    private bool dentroDelAgua = true;

    public bool DentroDelAgua => dentroDelAgua;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void CambiarEstadoAgua(bool estaDentro)
    {
        dentroDelAgua = estaDentro;

        if (rb == null)
        {
            return;
        }

        rb.gravityScale = dentroDelAgua
            ? gravedadAgua
            : gravedadAire;
    }
}