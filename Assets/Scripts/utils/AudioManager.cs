using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Audio")]
    public AudioSource musicSource;

    [Header("Escenas sin música")]
    public List<string> escenasSinMusica;

    private void Awake()
    {
        // Singleton
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (escenasSinMusica.Contains(scene.name))
        {
            DetenerMusica();
        }
        else
        {
            ReproducirMusica();
        }
    }

    public void ReproducirMusica()
    {
        if (!musicSource.isPlaying)
        {
            musicSource.Play();
        }
    }

    public void DetenerMusica()
    {
        if (musicSource.isPlaying)
        {
            musicSource.Stop();
        }
    }
}