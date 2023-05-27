using UnityEngine;
using UnityEngine.Audio;

public class AudioSettingsManager : MonoBehaviour
{
    [SerializeField] private AudioMixer _audioMixer;

    const string MASTER_VOLUME_NAME = "Master";
    const string GAMEPLAY_VOLUME_NAME = "Gameplay";
    const string UI_VOLUME_NAME = "UI";
    const string MUSIC_VOLUME_NAME = "Music";

    private const float _defaultVolume = 0.70f;
    public float MasterVolume { get; set; }
    public float GameplayVolume { get; set; }
    public float UiVolume { get; set; }
    public float MusicVolume { get; set; }

    // I didn't wanted to create another script that carries over scene or a scriptable object
    // So I will leave it here
    public bool PostProcessingEnabled { get; set; }

    public static AudioSettingsManager Instance;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        PostProcessingEnabled = true;
    }

    void Start()
    {
        SetMixerVolume(MASTER_VOLUME_NAME, _defaultVolume);
        SetMixerVolume(GAMEPLAY_VOLUME_NAME, _defaultVolume);
        SetMixerVolume(UI_VOLUME_NAME, _defaultVolume);
        SetMixerVolume(MUSIC_VOLUME_NAME, _defaultVolume);
    }

    private void SetMixerVolume(string mixerName, float volume)
    {
        float normalized = Mathf.Log10(volume) * 20;
        _audioMixer.SetFloat(mixerName, normalized);

        if (mixerName == MASTER_VOLUME_NAME) MasterVolume = volume;
        if (mixerName == GAMEPLAY_VOLUME_NAME) GameplayVolume = volume;
        if (mixerName == UI_VOLUME_NAME) UiVolume = volume;
        if (mixerName == MUSIC_VOLUME_NAME) MusicVolume = volume;
    }

    public float GetMixerVolume(string mixerName)
    {
        if (mixerName == MASTER_VOLUME_NAME) return MasterVolume;
        if (mixerName == GAMEPLAY_VOLUME_NAME) return GameplayVolume;
        if (mixerName == UI_VOLUME_NAME) return UiVolume;
        if (mixerName == MUSIC_VOLUME_NAME) return MusicVolume;

        return _defaultVolume;
    }

    public void SetMixerVolumeFromSlider(string mixerName, float volume)
    {
        SetMixerVolume(mixerName, volume);
    }
}
