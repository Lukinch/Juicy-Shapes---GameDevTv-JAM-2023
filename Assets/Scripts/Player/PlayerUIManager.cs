using UnityEngine;
using TMPro;

public class PlayerUIManager : MonoBehaviour
{
    [SerializeField] private GameObject _playerUIObject;
    [SerializeField] private TextMeshProUGUI _wavesCount;

    void OnEnable()
    {
        EnemiesManager.OnWaveEnded += EnemiesManager_OnWaveEnded;
        GameStateManager.OnAnyEndScreenShown += GameStateManager_OnAnyEndScreenShown;
        GameStateManager.OnNextWaveCountdownFinished += GameStateManager_OnNextWaveCountdownFinished;
    }

    void OnDisable()
    {
        EnemiesManager.OnWaveEnded -= EnemiesManager_OnWaveEnded;
        GameStateManager.OnAnyEndScreenShown -= GameStateManager_OnAnyEndScreenShown;
        GameStateManager.OnNextWaveCountdownFinished -= GameStateManager_OnNextWaveCountdownFinished;
    }

    private void EnemiesManager_OnWaveEnded(int currentWave, int maxWaves)
    {
        _wavesCount.text = $"Wave: {currentWave + 1}/{maxWaves}";
    }

    private void GameStateManager_OnAnyEndScreenShown()
    {
        _wavesCount.text = $"Wave: {1}/{8}";
        HideUI();
    }

    private void GameStateManager_OnNextWaveCountdownFinished()
    {
        ShowUI();
    }

    private void ShowUI()
    {
        _playerUIObject.SetActive(true);
    }

    private void HideUI()
    {
        _playerUIObject.SetActive(false);
    }
}
