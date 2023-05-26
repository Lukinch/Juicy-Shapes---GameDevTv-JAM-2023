using System;
using System.Collections.Generic;
using UnityEngine;
// using NaughtyAttributes;

[CreateAssetMenu(
    menuName = "Project Juicer/Waves",
    fileName = "Waves")]
public class WavesSO : ScriptableObject
{
    public List<Wave> Waves;
}

[Serializable]
public struct Wave
{
    public List<EnemyWave> Enemies;
}


[Serializable]
public struct EnemyWave
{
    // [ShowAssetPreview]
    public GameObject EnemyPrefab;
    public int AmountToSpawn;
}
