using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Sonidos")]
    [SerializeField] private AudioClip sonidoComer;
    [SerializeField] private AudioClip sonidoDanio;
    [SerializeField] private AudioClip sonidoExplosion;
    [SerializeField] private AudioClip sonidoDisparo;
    [SerializeField] private AudioClip sonidoEntradaAgua;
    [SerializeField] private AudioClip sonidoSalidaAgua;
    [SerializeField] private AudioClip sonidoGameOver;

    [Header("Musica")]
    [SerializeField] private AudioClip musicaJuego;
    [SerializeField] private float volumenMusica = 0.35f;
    [SerializeField] private float volumenEfectos = 0.8f;

    private AudioSource musicaSource;
    private AudioSource efectosSource;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        musicaSource = gameObject.AddComponent<AudioSource>();
        efectosSource = gameObject.AddComponent<AudioSource>();

        musicaSource.loop = true;
        musicaSource.playOnAwake = false;
        musicaSource.volume = volumenMusica;

        efectosSource.playOnAwake = false;
        efectosSource.volume = volumenEfectos;
    }

    private void Start()
    {
        if (musicaJuego != null)
        {
            musicaSource.clip = musicaJuego;
            musicaSource.Play();
        }
    }

    public void ReproducirComer()
    {
        Reproducir(sonidoComer);
    }

    public void ReproducirDanio()
    {
        Reproducir(sonidoDanio);
    }

    public void ReproducirExplosion()
    {
        Reproducir(sonidoExplosion);
    }

    public void ReproducirDisparo()
    {
        Reproducir(sonidoDisparo);
    }

    public void ReproducirEntradaAgua()
    {
        Reproducir(sonidoEntradaAgua);
    }

    public void ReproducirSalidaAgua()
    {
        Reproducir(sonidoSalidaAgua);
    }

    public void ReproducirGameOver()
    {
        Reproducir(sonidoGameOver);
    }

    private void Reproducir(AudioClip clip)
    {
        if (clip == null || efectosSource == null)
        {
            return;
        }

        efectosSource.PlayOneShot(clip);
    }
}