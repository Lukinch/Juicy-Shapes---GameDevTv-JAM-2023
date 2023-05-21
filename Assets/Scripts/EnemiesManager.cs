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
    public static event Action OnAllWavesCleared;

    void OnEnable()
    {
        _player = FindFirstObjectByType<PlayerMovement>().transform;
        // Listens to any enemy died event
        EnemyHealth.OnEnemyDied += OnEnemyDied;
        GameStateManager.OnNextWaveCountdownFinished += SpawnNextWave;
        GameStateManager.OnPlayAgain += GameStateManager_OnPlayAgain;

        GameStateManager_OnPlayAgain();
    }

    void OnDisable()
    {
        EnemyHealth.OnEnemyDied -= OnEnemyDied;
        GameStateManager.OnNextWaveCountdownFinished -= SpawnNextWave;
        GameStateManager.OnPlayAgain -= GameStateManager_OnPlayAgain;
    }

    private void GameStateManager_OnPlayAgain()
    {
        _poolingManager.ClearAllPools();
        _maxWaves = _waves.Waves.Count;
        _currentWave = 0;
        _currentAmountOfEnemies = 0;
    }

    private void OnEnemyDied()
    {
        _currentAmountOfEnemies--;
        if (_currentAmountOfEnemies > 0) return;
        if (_currentWave >= _maxWaves)
        {
            OnAllWavesCleared?.Invoke();
            return;
        }

        _currentAmountOfEnemies = 0; // Safe check

        OnWaveEnded?.Invoke(_currentWave, _maxWaves);
    }

    // [Button]
    private void SpawnNextWave()
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
