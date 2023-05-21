using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseManager : MonoBehaviour
{
    [SerializeField] private InputReaderSO _inputReader;
    [SerializeField] private GameObject _pauseScreen;

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
        _isPaused = !_isPaused;
        if (_isPaused)
            Pause();
        else
            Resume();
    }

    public void Pause()
    {
        OnGamePaused?.Invoke(true);
        _pauseScreen.SetActive(true);
        Time.timeScale = 0;
    }

    public void Resume()
    {
        OnGamePaused?.Invoke(false);
        _pauseScreen.SetActive(false);
        Time.timeScale = 1;
    }
}
