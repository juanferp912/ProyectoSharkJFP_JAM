using UnityEngine;

public class PlayerWaterSplash : MonoBehaviour
{
    [SerializeField] private GameObject splashPrefab;
    [SerializeField] private float nivelSuperficieAgua = 8f;
    [SerializeField] private float separacionMinimaEntreSplash = 0.2f;

    private bool estabaDentroDelAgua;
    private float ultimoSplash;

    private void Start()
    {
        estabaDentroDelAgua =
            transform.position.y <= nivelSuperficieAgua;
    }

    private void Update()
    {
        bool dentroDelAgua =
            transform.position.y <= nivelSuperficieAgua;

        if (
            dentroDelAgua != estabaDentroDelAgua &&
            Time.time >= ultimoSplash + separacionMinimaEntreSplash
        )
        {
            CrearSplash();

            ultimoSplash = Time.time;
            estabaDentroDelAgua = dentroDelAgua;
        }
    }

    private void CrearSplash()
    {
        if (splashPrefab == null)
        {
            return;
        }

        Vector3 posicion = new Vector3(
            transform.position.x,
            nivelSuperficieAgua,
            -1f
        );

        GameObject splash = Instantiate(
            splashPrefab,
            posicion,
            Quaternion.identity
        );

        ParticleSystem sistema =
            splash.GetComponent<ParticleSystem>();

        if (sistema != null)
        {
            sistema.Play();
        }

        Destroy(splash, 2f);
    }
}