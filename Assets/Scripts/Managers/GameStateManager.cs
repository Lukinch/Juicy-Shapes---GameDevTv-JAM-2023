using System;
using System.Collections;
using UnityEngine;
using TMPro;
using UnityEditor;
using UnityEngine.SceneManagement;

public class GameStateManager : MonoBehaviour
{
    [Header("Objective Screen Dependencies")]
    [SerializeField] private GameObject _objectiveScreen;
    [Header("Next Wave Screen Dependencies")]
    [SerializeField] private GameObject _nextWaveScreen;
    [SerializeField] private TextMeshProUGUI _countdownText;
    [SerializeField] private int _timeTillNextWave = 3;
    [Header("Win Screen Dependencies")]
    [SerializeField] private GameObject _winScreen;
    [Header("Game Over Screen Dependencies")]
    [SerializeField] private GameObject _gameOverScreen;

    private int _currentTime;
    private GameObject _currentShownScreen = null;

    public static event Action OnGameStarted;
    public static event Action OnNextWaveCountdownFinished;
    public static event Action OnPlayAgain;
    public static event Action OnAnyEndScreenShown;
    public static event Action OnControlsAccepted;

    void Awake()
    {
        _objectiveScreen.SetActive(false);
        _nextWaveScreen.SetActive(false);
        _winScreen.SetActive(false);
        _gameOverScreen.SetActive(false);
    }

    void OnEnable()
    {
        PlayerHealth.OnPlayerDied += ShowGameOverScreen;
        PauseManager.OnGamePaused += PauseManager_OnGamePaused;
        ProgressionManager.OnUpgradeFinished += StartCountDownToNextWave;
        EnemiesManager.OnAllWavesCleared += ShowWinScreen;
    }

    void OnDisable()
    {
        PlayerHealth.OnPlayerDied -= ShowGameOverScreen;
        PauseManager.OnGamePaused -= PauseManager_OnGamePaused;
        ProgressionManager.OnUpgradeFinished -= StartCountDownToNextWave;
        EnemiesManager.OnAllWavesCleared -= ShowWinScreen;
    }

    void Start()
    {
        OnGameStarted?.Invoke();
        _objectiveScreen.SetActive(true);
        _currentShownScreen = _objectiveScreen;
    }

    private void PauseManager_OnGamePaused(bool isPaused)
    {
        if (isPaused)
        {
            if (_currentShownScreen)
                _currentShownScreen.SetActive(false);
        }
        else
        {
            if (_currentShownScreen)
                _currentShownScreen.SetActive(true);
        }
    }

    private void ShowGameOverScreen()
    {
        _gameOverScreen.SetActive(true);
        _currentShownScreen = _gameOverScreen;
        OnAnyEndScreenShown?.Invoke();
    }

    private void ShowWinScreen()
    {
        _winScreen.SetActive(true);
        _currentShownScreen = _winScreen;
        OnAnyEndScreenShown?.Invoke();
    }

    private void StartCountDownToNextWave()
    {
        StartCoroutine(NextWaveCountdown());
    }

    private IEnumerator NextWaveCountdown()
    {
        _nextWaveScreen.SetActive(true);
        _currentTime = _timeTillNextWave;
        UpdateTimeText();

        while (_currentTime > 0)
        {
            yield return new WaitForSeconds(1);
            _currentTime--;
            UpdateTimeText();
        }

        OnNextWaveCountdownFinished?.Invoke();

        _nextWaveScreen.SetActive(false);
    }

    private void UpdateTimeText()
    {
        _countdownText.text = $"{_currentTime}";
    }

    // Meant to be called by buttons on Scene
    public void AcceptControls()
    {
        OnControlsAccepted?.Invoke();
        _objectiveScreen.SetActive(false);
        _currentShownScreen = null;
        StartCountDownToNextWave();
    }

    // Meant to be called by buttons on Scene
    public void ExitGame()
    {
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit();
#endif
    }

    // Meant to be called by buttons on Scene
    public void LoadMainMenu()
    {
        SceneManager.LoadScene(0);
        Time.timeScale = 1;
    }

    // Meant to be called by buttons on Scene
    public void PlayAgain()
    {
        OnPlayAgain?.Invoke();
        _winScreen.SetActive(false);
        _gameOverScreen.SetActive(false);
        _currentShownScreen = null;
        StartCountDownToNextWave();
    }
}
