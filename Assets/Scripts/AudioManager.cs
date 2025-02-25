using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    public AudioSource startMusic;
    public AudioSource levelMusic;
    private string currentScene;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void Start()
    {
        currentScene = SceneManager.GetActiveScene().name;
        PlayMusic(currentScene);
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        currentScene = scene.name;
        PlayMusic(currentScene);
    }

    void PlayMusic(string sceneName)
    {
        if (sceneName == "StartScene")
        {
            levelMusic.Stop();
            if (!startMusic.isPlaying) startMusic.Play();
        }
        else
        {
            startMusic.Stop();
            if (!levelMusic.isPlaying) levelMusic.Play();
        }
    }
}
