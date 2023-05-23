using UnityEngine;

public class PlayerUIManager : MonoBehaviour
{
    [SerializeField] private GameObject _playerUIObject;

    void OnEnable()
    {
        EnemiesManager.OnWaveEnded += HideUI;
        GameStateManager.OnNextWaveCountdownFinished += ShowUI;
    }

    void OnDisable()
    {
        EnemiesManager.OnWaveEnded -= HideUI;
        GameStateManager.OnNextWaveCountdownFinished -= ShowUI;
    }

    private void ShowUI()
    {
        _playerUIObject.SetActive(true);
    }

    private void HideUI(int arg1, int arg2)
    {
        _playerUIObject.SetActive(false);
    }
}
