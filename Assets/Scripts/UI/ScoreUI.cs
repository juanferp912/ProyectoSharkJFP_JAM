using TMPro;
using UnityEngine;

public class ScoreUI : MonoBehaviour
{
    [SerializeField] private TMP_Text textoPuntuacion;

    private void Start()
    {
        if (GameManager.Instance == null)
        {
            return;
        }

        ActualizarTexto(GameManager.Instance.Puntuacion);
        GameManager.Instance.PuntuacionCambiada += ActualizarTexto;
    }

    private void OnDestroy()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.PuntuacionCambiada -= ActualizarTexto;
        }
    }

    private void ActualizarTexto(int puntuacion)
    {
        textoPuntuacion.text = "Puntos: " + puntuacion;
    }
}