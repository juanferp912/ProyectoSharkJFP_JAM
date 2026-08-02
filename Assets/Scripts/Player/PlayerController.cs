using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float velocidad = 5f;
    [SerializeField] private Transform boca;
    [SerializeField] private float posicionBocaX = 1.2f;

    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private Vector2 movimiento;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        movimiento = new Vector2(horizontal, vertical).normalized;

        if (horizontal > 0)
        {
            spriteRenderer.flipX = false;

            boca.localPosition = new Vector3(
                Mathf.Abs(posicionBocaX),
                boca.localPosition.y,
                boca.localPosition.z
            );
        }
        else if (horizontal < 0)
        {
            spriteRenderer.flipX = true;

            boca.localPosition = new Vector3(
                -Mathf.Abs(posicionBocaX),
                boca.localPosition.y,
                boca.localPosition.z
            );
        }
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = movimiento * velocidad;
    }
}