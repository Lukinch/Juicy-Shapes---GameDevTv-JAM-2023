using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Random = UnityEngine.Random;

public class ProgressionManager : MonoBehaviour
{
    [Header("UI Dependencies")]
    [SerializeField] private GameObject _upgradesPanel;
    [SerializeField] private TextMeshProUGUI _upgradeTitle;
    [SerializeField] private TextMeshProUGUI _upgradeDescription;
    [SerializeField] private TextMeshProUGUI _currentStatText;

    [Header("Upgrades")]
    [SerializeField] private List<UpgradeSO> _upgrades;

    #region Cards Data
    [Header("Cards Button")]
    [SerializeField] private Button _leftCardButton;
    [SerializeField] private Button _middleCardButton;
    [SerializeField] private Button _rightCardButton;

    [Header("Cards Data")]
    [SerializeField] private UpgradeData[] _upgradesData;
    #endregion

    private PlayerStats _playerStats;
    private UpgradeSO[] _drawnUpgrades;

    public static event Action OnUpgradeFinished;

    void OnEnable()
    {
        _upgradesPanel.SetActive(false);

        _playerStats = FindFirstObjectByType<PlayerStats>();

        EnemiesManager.OnWaveEnded += EnemiesManager_OnWaveEnded;

        _leftCardButton.onClick.AddListener(() => OnUpgradeSelected(0));
        _middleCardButton.onClick.AddListener(() => OnUpgradeSelected(1));
        _rightCardButton.onClick.AddListener(() => OnUpgradeSelected(2));
    }

    void OnDisable()
    {
        EnemiesManager.OnWaveEnded -= EnemiesManager_OnWaveEnded;

        _leftCardButton.onClick.RemoveAllListeners();
        _middleCardButton.onClick.RemoveAllListeners();
        _rightCardButton.onClick.RemoveAllListeners();
    }

    private void OnUpgradeSelected(int upgradeIndex)
    {
        _drawnUpgrades[upgradeIndex].ApplyUpgrade(_playerStats);
        _upgradesPanel.SetActive(false);
        OnUpgradeFinished?.Invoke();
    }

    /// <summary>
    /// Beings the Upgrade selection
    /// </summary>
    private void EnemiesManager_OnWaveEnded(int currentWave, int maxWaves)
    {
        if (currentWave == maxWaves) return; // TODO: you win screen is shown here.

        _upgradesPanel.SetActive(true);
        DrawThreeUpgrades();
    }

    private void DrawThreeUpgrades()
    {
        int upgradeIndex;
        bool isUpgradeValid;
        _drawnUpgrades = new UpgradeSO[3];
        for (int i = 0; i < 3; i++)
        {
            upgradeIndex = Random.Range(0, _upgrades.Count);
            isUpgradeValid = CheckUpgradeValidity(_upgrades[upgradeIndex]);

            while (isUpgradeValid == false ||
                _upgrades[upgradeIndex] == _drawnUpgrades[0] ||
                _upgrades[upgradeIndex] == _drawnUpgrades[1])
            {
                upgradeIndex = Random.Range(0, _upgrades.Count);
                isUpgradeValid = CheckUpgradeValidity(_upgrades[upgradeIndex]);
            }
            _drawnUpgrades[i] = _upgrades[upgradeIndex];
        }
        UpdateCards();
    }

    private void UpdateCards()
    {
        for (int i = 0; i < _upgradesData.Length; i++)
        {
            _upgradesData[i].CardIcon.sprite = _drawnUpgrades[i].Icon;
            _upgradesData[i].CardName.text = _drawnUpgrades[i].Title;
            _upgradesData[i].Title = _drawnUpgrades[i].Title;
            _upgradesData[i].Description = _drawnUpgrades[i].Description;

            if (_upgradesData[i].CardIcon.sprite == null)
            {
                _upgradesData[i].CardIcon.gameObject.SetActive(false);
                _upgradesData[i].CardName.gameObject.SetActive(true);
            }
            else
            {
                _upgradesData[i].CardIcon.gameObject.SetActive(true);
                _upgradesData[i].CardName.gameObject.SetActive(false);
            }
        }

        SelectDefaultButtonAndUpdateDescriptions();
    }

    private void SelectDefaultButtonAndUpdateDescriptions()
    {
        _leftCardButton.Select();

        _upgradeTitle.text = _upgradesData[0].Title;
        _upgradeDescription.text = _upgradesData[0].Description;
    }

    private bool CheckUpgradeValidity(UpgradeSO upgradeSO)
    {
        switch (upgradeSO.UpgradeType)
        {
            case UpgradeType.Movement:
                return true; // Movement has no cap right now, so just return as valid
            case UpgradeType.Fire:
                switch (upgradeSO.FireUpgradeType)
                {
                    case FireUpgradeType.Pattern:
                        if (_playerStats.ShootingPattern == upgradeSO.ShootingPattern)
                            return false;
                        else
                            return true;
                    case FireUpgradeType.ShotsAmount:
                        if ((upgradeSO.AddShotsAmount + _playerStats.ShotsAmount) >
                            _playerStats.MaxShotsAmount)
                            return false;
                        else
                            return true;
                    case FireUpgradeType.ShotsPerSecond:
                        if ((upgradeSO.AddShotsPerSecond + _playerStats.ShotsPerSecond) >
                            _playerStats.MaxShotsPerSecond)
                            return false;
                        else
                            return true;
                    case FireUpgradeType.Damage:
                        return true; // Damage has no cap right now, so just return as valid
                }
                break;
            case UpgradeType.Health:
                return true; // Health has no cap right now, so just return as valid
            default:
                return false;
        }

        return false;
    }

    private void UpdateCurrentText(UpgradeSO upgradeSO)
    {
        _currentStatText.text = upgradeSO.UpgradeType switch
        {
            UpgradeType.Movement => upgradeSO.MovementUpgradeType switch
            {
                MovementUpgradeType.MovementSpeed => $"Current: {_playerStats.MoveSpeed}",
                MovementUpgradeType.Acceleration => $"Current: {_playerStats.Acceleration}",
                _ => $"Current: Undefined MovementUpgradeType",
            },
            UpgradeType.Fire => upgradeSO.FireUpgradeType switch
            {
                FireUpgradeType.ShotsAmount => $"Current: {_playerStats.ShotsAmount}",
                FireUpgradeType.ShotsPerSecond => $"Current: {_playerStats.ShotsPerSecond}",
                FireUpgradeType.Damage => $"Current: {_playerStats.Damage}",
                FireUpgradeType.Pattern => $"Current: {_playerStats.ShootingPattern.PatternName}",
                _ => $"Current: Undefined FireUpgradeType",
            },
            UpgradeType.Health => $"Current: {_playerStats.CurrentMaxHealthPoints}",
            _ => $"Current: Undefined UpgradeType",
        };
    }

    public void OnCardHighlighted(int cardIndex)
    {
        _upgradeTitle.text = _upgradesData[cardIndex].Title;
        _upgradeDescription.text = _upgradesData[cardIndex].Description;
        UpdateCurrentText(_drawnUpgrades[cardIndex]);
    }
}

[Serializable]
public struct UpgradeData
{
    public Image CardIcon;
    public TextMeshProUGUI CardName;
    [HideInInspector] public string Title;
    [HideInInspector] public string Description;
}
