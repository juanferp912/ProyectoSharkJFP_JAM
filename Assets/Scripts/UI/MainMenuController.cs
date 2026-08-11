using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] private GameObject panelMenuPrincipal;
    [SerializeField] private GameObject panelInstrucciones;
    [SerializeField] private GameObject panelMundos;

    private void Start()
    {
        Time.timeScale = 1f;

        panelMenuPrincipal.SetActive(true);
        panelInstrucciones.SetActive(false);
        panelMundos.SetActive(false);
    }

    public void AbrirSeleccionMundos()
    {
        panelMenuPrincipal.SetActive(false);
        panelInstrucciones.SetActive(false);
        panelMundos.SetActive(true);
    }

    public void MostrarInstrucciones()
    {
        panelMenuPrincipal.SetActive(false);
        panelInstrucciones.SetActive(true);
        panelMundos.SetActive(false);
    }

    public void CerrarInstrucciones()
    {
        panelMenuPrincipal.SetActive(true);
        panelInstrucciones.SetActive(false);
        panelMundos.SetActive(false);
    }

    public void VolverMenuPrincipal()
    {
        panelMenuPrincipal.SetActive(true);
        panelInstrucciones.SetActive(false);
        panelMundos.SetActive(false);
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
}