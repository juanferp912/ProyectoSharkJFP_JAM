using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TurboUI : MonoBehaviour
{
    [SerializeField] private PlayerController jugador;
    [SerializeField] private Slider barraTurbo;
    [SerializeField] private TMP_Text textoTurbo;

    private void Update()
    {
        if (jugador == null || barraTurbo == null)
        {
            return;
        }

        if (jugador.TurboEnRecarga)
        {
            float progresoRecarga =
                1f - jugador.RecargaRestante / jugador.TiempoRecargaTurbo;

            barraTurbo.value = progresoRecarga;

            if (textoTurbo != null)
            {
                textoTurbo.text = "RECARGANDO";
            }

            return;
        }

        barraTurbo.value =
            jugador.TurboRestante / jugador.DuracionTurbo;

        if (textoTurbo == null)
        {
            return;
        }

        if (jugador.UsandoTurbo)
        {
            textoTurbo.text = "TURBO";
        }
        else
        {
            textoTurbo.text = "M - TURBO";
        }
    }
}