using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Range(0f, 0.5f)]
    public float musicVolume = 0.5f;

    private AudioSource musicSource;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        musicSource = GetComponent<AudioSource>();
        if (musicSource != null)
            musicSource.volume = musicVolume;
    }

    public void SetMusicVolume(float value)
    {
        musicVolume = value;

        if (musicSource != null)
            musicSource.volume = musicVolume;
    }
}
