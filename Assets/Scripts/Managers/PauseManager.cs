using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseManager : MonoBehaviour
{
    [SerializeField] private InputReaderSO _inputReader;
    [SerializeField] private GameObject _pauseScreen;
    [SerializeField] private GameObject _optionsSCreen;

    private bool _isPaused = false;

    public static event Action<bool> OnGamePaused;

    void OnEnable()
    {
        _inputReader.OnPaused += InputReader_OnPause;
    }

    void OnDisable()
    {
        _inputReader.OnPaused -= InputReader_OnPause;
    }

    private void InputReader_OnPause()
    {
        TogglePause();
    }

    private void Pause()
    {
        OnGamePaused?.Invoke(true);
        _pauseScreen.SetActive(true);
        Time.timeScale = 0;
    }

    private void Resume()
    {
        OnGamePaused?.Invoke(false);
        _optionsSCreen.SetActive(false);
        _pauseScreen.SetActive(false);
        Time.timeScale = 1;
    }

    public void TogglePause()
    {
        _isPaused = !_isPaused;
        if (_isPaused)
            Pause();
        else
            Resume();
    }

    public void ShowOptions()
    {
        _pauseScreen.SetActive(false);
        _optionsSCreen.SetActive(true);
    }

    public void HideOptions()
    {
        _optionsSCreen.SetActive(false);
        _pauseScreen.SetActive(true);
    }
}
