using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public AudioClip homeMusic;
    public AudioClip gameMusic;
    private AudioSource audioSource;

    private string lastSceneName = "";

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.loop = true;
            audioSource.playOnAwake = false;
            audioSource.volume = 1.0f;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        PlayMusic(SceneManager.GetActiveScene().name);
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        PlayMusic(scene.name);
    }

    public void PlayMusic(string sceneName)
    {


        AudioClip clipToPlay = null;

        if (sceneName == "HomeScene")
            clipToPlay = homeMusic;
        else if (sceneName == "GameScene")
            clipToPlay = gameMusic;


        if (clipToPlay != null && (audioSource.clip != clipToPlay || sceneName == lastSceneName))
        {
            audioSource.clip = clipToPlay;
            audioSource.Stop();
            audioSource.Play();
        }

        lastSceneName = sceneName;
    }

    public void SetVolume(float volume)
    {
        if (audioSource != null)
        {
            audioSource.volume = volume;
        }
    }


    public void RestartMusic()
    {
        if (audioSource != null && audioSource.clip != null)
        {
            audioSource.Stop();
            audioSource.Play();
        }
    }
}
