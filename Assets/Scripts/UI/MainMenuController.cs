using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] private string nombreEscenaJuego = "SampleScene";
    [SerializeField] private GameObject panelMenuPrincipal;
    [SerializeField] private GameObject panelInstrucciones;

    private void Start()
    {
        Time.timeScale = 1f;

        if (panelMenuPrincipal != null)
        {
            panelMenuPrincipal.SetActive(true);
        }

        if (panelInstrucciones != null)
        {
            panelInstrucciones.SetActive(false);
        }
    }

    public void Jugar()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(nombreEscenaJuego);
    }

    public void MostrarInstrucciones()
    {
        if (panelMenuPrincipal != null)
        {
            panelMenuPrincipal.SetActive(false);
        }

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

        if (panelMenuPrincipal != null)
        {
            panelMenuPrincipal.SetActive(true);
        }
    }

    public void Salir()
    {
        Application.Quit();
    }
}