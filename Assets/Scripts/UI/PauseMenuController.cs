using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuController : MonoBehaviour
{
    [SerializeField] private GameObject panelPausa;
    [SerializeField] private string nombreEscenaMenu = "MainMenu";

    private bool pausado;

    private void Start()
    {
        Time.timeScale = 1f;
        AudioListener.pause = false;

        if (panelPausa != null)
        {
            panelPausa.SetActive(false);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            CambiarPausa();
        }
    }

    public void CambiarPausa()
    {
        pausado = !pausado;

        if (panelPausa != null)
        {
            panelPausa.SetActive(pausado);
        }

        Time.timeScale = pausado ? 0f : 1f;
        AudioListener.pause = pausado;
    }

    public void Continuar()
    {
        pausado = false;

        if (panelPausa != null)
        {
            panelPausa.SetActive(false);
        }

        Time.timeScale = 1f;
        AudioListener.pause = false;
    }

    public void Reiniciar()
    {
        Time.timeScale = 1f;
        AudioListener.pause = false;

        SceneManager.LoadScene(
            SceneManager.GetActiveScene().buildIndex
        );
    }

    public void VolverAlMenu()
    {
        Time.timeScale = 1f;
        AudioListener.pause = false;

        SceneManager.LoadScene(
            nombreEscenaMenu
        );
    }
}