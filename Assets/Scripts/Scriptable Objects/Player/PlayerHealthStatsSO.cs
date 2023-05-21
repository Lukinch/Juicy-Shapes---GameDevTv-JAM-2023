using UnityEngine;

[CreateAssetMenu(
    menuName = "Project Juicer/Player/Health Stats",
    fileName = "Player Health Stats")]
public class PlayerHealthStatsSO : ScriptableObject
{
    [Header("Health Options")]
    [SerializeField, Min(0)]
    private int _maxHealthPoints = 100;

    public int MaxHealthPoints => _maxHealthPoints;
}
