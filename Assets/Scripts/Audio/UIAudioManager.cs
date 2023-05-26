using UnityEngine;

public class UIAudioManager : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] private AudioSource _audioSource;

    [Header("Clips")]
    [SerializeField] private AudioClip _pauseClip;
    [SerializeField] private AudioClip _buttonHoverClip;
    [SerializeField] private AudioClip _buttonPressedClip;

    void OnEnable()
    {
        ButtonInteraction.OnButtonHover += ButtonInteraction_OnButtonHover;
        ButtonInteraction.OnButtonPressed += ButtonInteraction_OnButtonPressed;
        PauseManager.OnGamePaused += PauseManager_OnGamePaused;
    }
    void OnDisable()
    {
        ButtonInteraction.OnButtonHover -= ButtonInteraction_OnButtonHover;
        ButtonInteraction.OnButtonPressed -= ButtonInteraction_OnButtonPressed;
        PauseManager.OnGamePaused -= PauseManager_OnGamePaused;
    }

    private void PauseManager_OnGamePaused(bool isPaused)
    {
        if (isPaused)
            _audioSource.PlayOneShot(_pauseClip);
    }

    private void ButtonInteraction_OnButtonHover()
    {
        _audioSource.PlayOneShot(_buttonHoverClip);
    }

    private void ButtonInteraction_OnButtonPressed()
    {
        _audioSource.PlayOneShot(_buttonPressedClip);
    }
}
