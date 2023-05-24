using UnityEngine;

public class PlayerUIManager : MonoBehaviour
{
    [SerializeField] private GameObject _playerUIObject;

    void OnEnable()
    {
        GameStateManager.OnAnyEndScreenShown += GameStateManager_OnAnyEndScreenShown;
        GameStateManager.OnNextWaveCountdownFinished += GameStateManager_OnNextWaveCountdownFinished;
    }

    void OnDisable()
    {
        GameStateManager.OnAnyEndScreenShown -= GameStateManager_OnAnyEndScreenShown;
        GameStateManager.OnNextWaveCountdownFinished -= GameStateManager_OnNextWaveCountdownFinished;
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
