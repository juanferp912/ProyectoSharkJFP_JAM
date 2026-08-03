using TMPro;
using UnityEngine;

public class HealthUI : MonoBehaviour
{
    [SerializeField] private PlayerHealth saludJugador;
    [SerializeField] private TMP_Text textoVidas;
    [SerializeField] private TMP_Text textoGameOver;

    private void Start()
    {
        if (textoGameOver != null)
        {
            textoGameOver.gameObject.SetActive(false);
        }

        if (saludJugador == null)
        {
            return;
        }

        saludJugador.VidasCambiadas += ActualizarVidas;
        saludJugador.JugadorDerrotado += MostrarGameOver;

        ActualizarVidas(saludJugador.VidasActuales);
    }

    private void OnDestroy()
    {
        if (saludJugador == null)
        {
            return;
        }

        saludJugador.VidasCambiadas -= ActualizarVidas;
        saludJugador.JugadorDerrotado -= MostrarGameOver;
    }

    private void ActualizarVidas(int vidas)
    {
        if (textoVidas != null)
        {
            textoVidas.text = "Vidas: " + vidas;
        }
    }

    private void MostrarGameOver()
    {
        if (textoGameOver != null)
        {
            textoGameOver.gameObject.SetActive(true);
        }
    }
}