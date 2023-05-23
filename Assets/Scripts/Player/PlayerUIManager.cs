using UnityEngine;

public class PlayerUIManager : MonoBehaviour
{
    [SerializeField] private GameObject _playerUIObject;

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

    private void EnemiesManager_OnWaveEnded(int arg1, int arg2)
    {
        HideUI();
    }

    private void GameStateManager_OnAnyEndScreenShown()
    {
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
