using UnityEngine;
using System;
using Random = UnityEngine.Random;

public class EnemiesManager : MonoBehaviour
{
    [SerializeField] private PoolingManager _poolingManager;
    [SerializeField] private WavesSO _waves;
    [SerializeField] private Transform[] _spawnPoints;

    private Transform _previouslyUsedSpawnPoint;
    private Transform _player;
    private int _maxWaves;
    private int _currentWave;
    private int _currentAmountOfEnemies;

    // _currentWave, _maxWave
    public static event Action<int, int> OnWaveEnded;

    void OnEnable()
    {
        _player = FindFirstObjectByType<PlayerMovement>().transform;
        // Listens to any enemy died event
        ProgressionManager.OnUpgradeFinished += ProgressionManager_OnUpgradeFinished;
        EnemyHealth.OnEnemyDied += OnEnemyDied;

        _maxWaves = _waves.Waves.Count;
        _currentWave = 0;
        SpawnWave();
    }

    private void ProgressionManager_OnUpgradeFinished()
    {
        SpawnWave();
    }

    void OnDisable()
    {
        EnemyHealth.OnEnemyDied -= OnEnemyDied;
        ProgressionManager.OnUpgradeFinished -= ProgressionManager_OnUpgradeFinished;
    }

    private void OnEnemyDied()
    {
        _currentAmountOfEnemies--;
        if (_currentAmountOfEnemies > 0) return;
        if (_currentWave >= _maxWaves) return; // If out of waves, do something in the future, like "You win".
        _currentAmountOfEnemies = 0; // Safe check
        // for now we will continue to spawn the next wave until we ran out of them
        OnWaveEnded?.Invoke(_currentWave, _maxWaves);
    }

    // [Button]
    private void SpawnWave()
    {
        int spawnIndex;
        Wave currentWave = _waves.Waves[_currentWave];
        foreach (EnemyWave enemy in currentWave.Enemies)
        {
            for (int i = 0; i < enemy.AmountToSpawn; i++)
            {
                // Get a random spawn point different from the previous one
                spawnIndex = Random.Range(0, _spawnPoints.Length);
                while (_spawnPoints[spawnIndex] == _previouslyUsedSpawnPoint)
                {
                    spawnIndex = Random.Range(0, _spawnPoints.Length);
                }
                _previouslyUsedSpawnPoint = _spawnPoints[spawnIndex];

                _poolingManager.PoolEnemy(enemy.EnemyPrefab, _spawnPoints[spawnIndex].position, Quaternion.identity, _player);
                _currentAmountOfEnemies++;
            }
        }

        _currentWave++;
    }
}
