using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [SerializeField] private AudioSource audioSource;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            AplicarVolumeSalvo();
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        if (Instance == this)
            SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        AplicarVolumeSalvo();
    }

    private void AplicarVolumeSalvo()
    {
        float v = PlayerPrefs.GetFloat("Volume", 1f);
        AudioListener.volume = v;
        if (audioSource != null)
        {
            audioSource.volume = v;             
        }
    }

    public void PlayBGM(AudioClip audio)
    {
        if (audioSource == null || audio == null) return;
        audioSource.volume = PlayerPrefs.GetFloat("Volume", AudioListener.volume);
        audioSource.clip = audio;
        audioSource.Play();
    }
}
