using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] private string nombreEscenaJuego = "SampleScene";
    [SerializeField] private GameObject panelInstrucciones;

    private void Start()
    {
        Time.timeScale = 1f;

        if (panelInstrucciones != null)
        {
            panelInstrucciones.SetActive(false);
        }
    }

    public void Jugar()
    {
        SceneManager.LoadScene(nombreEscenaJuego);
    }

    public void MostrarInstrucciones()
    {
        if (panelInstrucciones != null)
        {
            panelInstrucciones.SetActive(true);
        }
    }

    public void OcultarInstrucciones()
    {
        if (panelInstrucciones != null)
        {
            panelInstrucciones.SetActive(false);
        }
    }

    public void Salir()
    {
        Application.Quit();
    }
}