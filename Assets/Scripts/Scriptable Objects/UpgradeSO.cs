using UnityEngine;
using NaughtyAttributes;

[CreateAssetMenu(
    menuName = "Project Juicer/Upgrade",
    fileName = "Upgrade")]
public class UpgradeSO : ScriptableObject
{
    // There is a custom Editor for this class in Assets/Editor
    [ShowAssetPreview]
    public Sprite Icon;
    public string Title;
    [TextArea(5, 20)]
    public string Description;
    public UpgradeType UpgradeType;

    public MovementUpgradeType MovementUpgradeType;
    public FireUpgradeType FireUpgradeType;
    public HealthUpgradeType HealthUpgradeType;

    [Range(1, 10)]
    public int AddMoveSpeed = 1;
    [Range(1, 100)]
    public int AddAcceleration = 1;
    [Range(10, 100)]
    public int AddCurrentMaxHealthPoints = 50;
    [Range(0.1f, 1f)]
    public float CurrentHPPercentageHealAmount = 0.2f;
    [Range(2, 10)]
    public int AddShotsAmount = 2;
    [Range(1, 10)]
    public int AddShotsPerSecond = 1;
    [Range(1, 100)]
    public int AddDamage = 1;
    public ShootingPatternSO ShootingPattern;

    private void ApplyMovementUpgrade(PlayerStats playerStats)
    {
        switch (MovementUpgradeType)
        {
            case MovementUpgradeType.MovementSpeed:
                playerStats.MoveSpeed += AddMoveSpeed;
                break;
            case MovementUpgradeType.Acceleration:
                playerStats.Acceleration += AddAcceleration;
                break;
        }
    }
    private void ApplyFireUpgrade(PlayerStats playerStats)
    {
        switch (FireUpgradeType)
        {
            case FireUpgradeType.ShotsAmount:
                playerStats.ShotsAmount += AddShotsAmount;
                break;
            case FireUpgradeType.ShotsPerSecond:
                playerStats.ShotsPerSecond += AddShotsPerSecond;
                break;
            case FireUpgradeType.Damage:
                playerStats.Damage += AddDamage;
                break;
            case FireUpgradeType.Pattern:
                if (playerStats.ShotsAmount < 3)
                {
                    if (playerStats.ShootingPattern.PatternName != "Inverted T")
                        playerStats.ShotsAmount += AddShotsAmount;
                }
                playerStats.ShootingPattern = ShootingPattern;
                break;
        }
    }
    private void ApplyHealthUpgrade(PlayerStats playerStats)
    {
        playerStats.IncreaseMaxHPAndHeal(CurrentHPPercentageHealAmount, AddCurrentMaxHealthPoints);
    }

    public void ApplyUpgrade(PlayerStats playerStats)
    {
        switch (UpgradeType)
        {
            case UpgradeType.Movement:
                ApplyMovementUpgrade(playerStats);
                break;
            case UpgradeType.Fire:
                ApplyFireUpgrade(playerStats);
                break;
            case UpgradeType.Health:
                ApplyHealthUpgrade(playerStats);
                break;
        }
    }
}

public enum UpgradeType { Movement, Fire, Health }
public enum MovementUpgradeType { MovementSpeed, Acceleration }
public enum FireUpgradeType { Pattern, ShotsAmount, ShotsPerSecond, Damage }
public enum HealthUpgradeType { MaxHealthPoints }
