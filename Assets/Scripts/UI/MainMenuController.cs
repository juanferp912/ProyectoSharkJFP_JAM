using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] private GameObject panelPrincipal;
    [SerializeField] private GameObject panelMundos;

    private void Start()
    {
        Time.timeScale = 1f;
        MostrarMenuPrincipal();
    }

    public void AbrirSeleccionMundos()
    {
        if (panelPrincipal != null)
        {
            panelPrincipal.SetActive(false);
        }

        if (panelMundos != null)
        {
            panelMundos.SetActive(true);
        }
    }

    public void VolverMenuPrincipal()
    {
        MostrarMenuPrincipal();
    }

    public void CargarMundo1()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("NivelFacil");
    }

    public void CargarMundo2()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("NivelDificil");
    }

    public void SalirJuego()
    {
        Application.Quit();
    }

    private void MostrarMenuPrincipal()
    {
        if (panelPrincipal != null)
        {
            panelPrincipal.SetActive(true);
        }

        if (panelMundos != null)
        {
            panelMundos.SetActive(false);
        }
    }
}