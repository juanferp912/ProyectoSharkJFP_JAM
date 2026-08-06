using UnityEngine;

public class WaterZone : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        PlayerController player = other.GetComponent<PlayerController>();

        if (player != null)
        {
            player.CambiarEstadoAgua(true);
        }

        WaterPhysics2D waterPhysics = other.GetComponent<WaterPhysics2D>();

        if (waterPhysics != null)
        {
            waterPhysics.CambiarEstadoAgua(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        PlayerController player = other.GetComponent<PlayerController>();

        if (player != null)
        {
            player.CambiarEstadoAgua(false);
        }

        WaterPhysics2D waterPhysics = other.GetComponent<WaterPhysics2D>();

        if (waterPhysics != null)
        {
            waterPhysics.CambiarEstadoAgua(false);
        }
    }
}