
using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

public class MusicAudioManager : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] private AudioSource _musicAudioSource;
    [SerializeField] private AudioSource _upgradeAudioSource;

    [Header("Clips")]
    [SerializeField] private AudioClip _gameOverClip;
    [SerializeField] private AudioClip _upgradeClip;
    [SerializeField] private AudioClip[] _combatClips;

    private bool _isUpgradePlaying = false;
    private bool _isCombatPlaying = false;
    private AudioClip _currentClip;
    private Coroutine _waitForNextTrackCoroutine;

    void OnEnable()
    {
        PauseManager.OnGamePaused += PauseManager_OnGamePaused;
        ProgressionManager.OnUpgradeFinished += ProgressionManager_OnUpgradeFinished;
        ProgressionManager.OnUpgradeScreenShown += ProgressionManager_OnUpgradeScreenShown;
        GameStateManager.OnAnyEndScreenShown += GameStateManager_OnAnyEndScreenShown;
        GameStateManager.OnPlayAgain += GameStateManager_OnPlayAgain;

        PlayRandomLevelTheme();
    }

    void OnDisable()
    {
        PauseManager.OnGamePaused -= PauseManager_OnGamePaused;
        ProgressionManager.OnUpgradeFinished -= ProgressionManager_OnUpgradeFinished;
        ProgressionManager.OnUpgradeScreenShown -= ProgressionManager_OnUpgradeScreenShown;
        GameStateManager.OnAnyEndScreenShown -= GameStateManager_OnAnyEndScreenShown;
        GameStateManager.OnPlayAgain -= GameStateManager_OnPlayAgain;
    }

    private void GameStateManager_OnPlayAgain()
    {
        _musicAudioSource.Stop();
        PlayRandomLevelTheme();
    }

    private void GameStateManager_OnAnyEndScreenShown()
    {
        _musicAudioSource.Stop();
        _musicAudioSource.clip = _gameOverClip;
        _musicAudioSource.loop = true;
        _musicAudioSource.Play();
    }

    private void ProgressionManager_OnUpgradeFinished()
    {
        _upgradeAudioSource.Pause();
        _musicAudioSource.Play();
        _isUpgradePlaying = false;
        _isCombatPlaying = true;
    }

    private void ProgressionManager_OnUpgradeScreenShown()
    {
        _musicAudioSource.Pause();
        _upgradeAudioSource.Play();
        _isCombatPlaying = false;
        _isUpgradePlaying = true;
    }

    private void PauseManager_OnGamePaused(bool isPaused)
    {
        if (isPaused)
        {
            if (_isCombatPlaying)
                _musicAudioSource.Pause();
            else if (_isUpgradePlaying)
                _upgradeAudioSource.Pause();
        }
        else
        {
            if (_isCombatPlaying)
                _musicAudioSource.Play();
            else if (_isUpgradePlaying)
                _upgradeAudioSource.Play();
        }
    }

    private IEnumerator PlayNextTrackAfterCurrent()
    {
        yield return new WaitForSeconds(_currentClip.length);
        PlayRandomLevelTheme();
    }

    private void PlayRandomLevelTheme()
    {
        _musicAudioSource.Stop();
        int index = Random.Range(0, _combatClips.Length);
        while (_currentClip == _combatClips[index])
        {
            index = Random.Range(0, _combatClips.Length);
        }
        _musicAudioSource.clip = _combatClips[index];
        _musicAudioSource.loop = false;
        _musicAudioSource.Play();

        _isCombatPlaying = true;
        _currentClip = _musicAudioSource.clip;

        _waitForNextTrackCoroutine = StartCoroutine(PlayNextTrackAfterCurrent());
    }
}
