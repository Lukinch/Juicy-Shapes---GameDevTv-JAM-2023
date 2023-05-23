using System;
using System.Collections.Generic;
using GlobalEnums;
using UnityEngine;

[CreateAssetMenu(menuName = "Project Juicer/Managers/Pooling Manager", fileName = "Pooling Manager")]
public class PoolingManager : ScriptableObject
{
    [Header("Projectile Materials")]
    [SerializeField] private Material _projectileMaterialRed;
    [SerializeField] private Material _projectileMaterialPink;
    [SerializeField] private Material _projectileMaterialLightBlue;
    [Header("Enemy Materials")]
    [SerializeField] private Material _enemyMaterialRed;
    [SerializeField] private Material _enemyMaterialPink;
    [SerializeField] private Material _enemyMaterialLightBlue;
    // string: prefab name, List: poolable prefabs
    [NonSerialized] // <- Data will not be saved between play sessions
    private Dictionary<string, List<GameObject>> _poolablesPool;
    [NonSerialized] // <- Data will not be saved between play sessions
    private Dictionary<string, GameObject> _parents;

    private void OnEnable()
    {
        _poolablesPool = new();
        _parents = new();
    }

    private void OnDisable()
    {
        DestroyAllGameObjects();
        _poolablesPool.Clear();
        _parents.Clear();
    }

    /// <summary>
    /// Data is no serialized, so each time there is a scripts recompilation
    /// all references to the objects in the scene gets lost, and starts creating new objects again
    /// while the old ones remain in scene unused at all.
    /// So each time a scripts recompilation happens, we destroy all game objects and clear the dictionaries.
    /// </summary>
    private void DestroyAllGameObjects()
    {
        foreach (var pool in _poolablesPool)
        {
            foreach (var gameObject in pool.Value)
            {
                if (gameObject)
                    Destroy(gameObject);
            }
        }
        foreach (var gameObject in _parents.Values)
        {
            if (gameObject)
                Destroy(gameObject);
        }

        _poolablesPool.Clear();
        _parents.Clear();
    }

    public void PoolProjectile(GameObject prefab, Vector3 position, Quaternion rotation, float projectileDamage, ThemeColor themeColor)
    {
        if (!_poolablesPool.ContainsKey(prefab.name))
        {
            // Add new dictionaries entries and create a new poolable.
            GameObject newParent = new(prefab.name + "s " + "Pool");
            _parents.Add(prefab.name, newParent);
            _poolablesPool.Add(prefab.name, new());

            GameObject newPoolable = Instantiate(prefab, position, rotation, newParent.transform);
            if (newPoolable.TryGetComponent(out IPoolable poolable))
                poolable.PoolingManagerSO = this;
            if (newPoolable.TryGetComponent(out Projectile projectile))
            {
                projectile.ProjectileColor = themeColor;
                projectile.Damage = projectileDamage;
                ChangeProjectileColor(projectile, themeColor);
            }

            _poolablesPool[prefab.name].Add(newPoolable);
        }
        else
        {
            Projectile projectile;
            // Look for available poolable in existing Pool
            foreach (GameObject poolableObject in _poolablesPool[prefab.name])
            {
                if (poolableObject.activeInHierarchy) continue;

                if (poolableObject.TryGetComponent(out projectile))
                {
                    projectile.ProjectileColor = themeColor;
                    projectile.Damage = projectileDamage;
                    ChangeProjectileColor(projectile, themeColor);
                }
                poolableObject.transform.SetPositionAndRotation(position, rotation);
                poolableObject.SetActive(true);
                return;
            }

            // If any available, creates a new one and adds it to the Pool
            GameObject newPoolable = Instantiate(prefab, position, rotation, _parents[prefab.name].transform);
            if (newPoolable.TryGetComponent(out IPoolable poolable))
                poolable.PoolingManagerSO = this;
            if (newPoolable.TryGetComponent(out projectile))
            {
                projectile.ProjectileColor = themeColor;
                projectile.Damage = projectileDamage;
                ChangeProjectileColor(projectile, themeColor);
            }

            _poolablesPool[prefab.name].Add(newPoolable);
        }
    }

    private void ChangeProjectileColor(Projectile projectile, ThemeColor themeColor)
    {
        switch (themeColor)
        {
            case ThemeColor.Red:
                projectile.Material = _projectileMaterialRed;
                break;
            case ThemeColor.Pink:
                projectile.Material = _projectileMaterialPink;
                break;
            case ThemeColor.LightBlue:
                projectile.Material = _projectileMaterialLightBlue;
                break;
        }
    }

    public void PoolEnemy(GameObject prefab, Vector3 position, Quaternion rotation, Transform player)
    {
        if (!_poolablesPool.ContainsKey(prefab.name))
        {
            // Add new dictionaries entries and create a new poolable.
            GameObject newParent = new(prefab.name + "s " + "Pool");
            _parents.Add(prefab.name, newParent);
            _poolablesPool.Add(prefab.name, new());

            GameObject newPoolable = Instantiate(prefab, position, rotation, newParent.transform);
            if (newPoolable.TryGetComponent(out IPoolable poolable))
                poolable.PoolingManagerSO = this;
            if (newPoolable.TryGetComponent(out EnemyAI enemyAI))
                enemyAI.Player = player;
            if (newPoolable.TryGetComponent(out EnemyColorManager enemyColorManager))
                ChangeEnemyToRandomColor(enemyColorManager);

            _poolablesPool[prefab.name].Add(newPoolable);
        }
        else
        {
            EnemyColorManager enemyColorManager;
            // Look for available poolable in existing Pool
            foreach (GameObject poolableObject in _poolablesPool[prefab.name])
            {
                if (poolableObject.activeInHierarchy) continue;

                if (poolableObject.TryGetComponent(out enemyColorManager))
                    ChangeEnemyToRandomColor(enemyColorManager);

                poolableObject.transform.SetPositionAndRotation(position, rotation);
                poolableObject.SetActive(true);
                return;
            }

            // If no available, creates a new one and adds it to the Pool
            GameObject newPoolable = Instantiate(prefab, position, rotation, _parents[prefab.name].transform);
            if (newPoolable.TryGetComponent(out IPoolable poolable))
                poolable.PoolingManagerSO = this;
            if (newPoolable.TryGetComponent(out EnemyAI enemyAI))
                enemyAI.Player = player;
            if (newPoolable.TryGetComponent(out enemyColorManager))
                ChangeEnemyToRandomColor(enemyColorManager);

            _poolablesPool[prefab.name].Add(newPoolable);
        }
    }

    private void ChangeEnemyToRandomColor(EnemyColorManager enemyColorManager)
    {
        int index = UnityEngine.Random.Range(0, 3);
        if (index == 0)
        {
            enemyColorManager.EnemyColor = ThemeColor.Red;
            enemyColorManager.Material = _enemyMaterialRed;
        }
        else if (index == 1)
        {
            enemyColorManager.EnemyColor = ThemeColor.Pink;
            enemyColorManager.Material = _enemyMaterialPink;
        }
        else
        {
            enemyColorManager.EnemyColor = ThemeColor.LightBlue;
            enemyColorManager.Material = _enemyMaterialLightBlue;
        }
    }

    public void ReturnObject(GameObject objectToReturn)
    {
        objectToReturn.SetActive(false);
    }

    public void ClearAllPools()
    {
        DestroyAllGameObjects();
    }
}
