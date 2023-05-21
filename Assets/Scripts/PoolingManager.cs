using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Project Juicer/Managers/Pooling Manager", fileName = "Pooling Manager")]
public class PoolingManager : ScriptableObject
{
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

    public void PoolProjectile(GameObject prefab, Vector3 position, Quaternion rotation, float projectileDamage)
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
                projectile.Damage = projectileDamage;

            _poolablesPool[prefab.name].Add(newPoolable);
        }
        else
        {
            // Look for available poolable in existing Pool
            foreach (GameObject poolableObject in _poolablesPool[prefab.name])
            {
                if (poolableObject.activeInHierarchy) continue;

                if (poolableObject.TryGetComponent(out Projectile bullet))
                    bullet.Damage = projectileDamage;
                poolableObject.transform.SetPositionAndRotation(position, rotation);
                poolableObject.SetActive(true);
                return;
            }

            // If any available, creates a new one and adds it to the Pool
            GameObject newPoolable = Instantiate(prefab, position, rotation, _parents[prefab.name].transform);
            if (newPoolable.TryGetComponent(out IPoolable poolable))
                poolable.PoolingManagerSO = this;
            if (newPoolable.TryGetComponent(out Projectile projectile))
                projectile.Damage = projectileDamage;

            _poolablesPool[prefab.name].Add(newPoolable);
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

            _poolablesPool[prefab.name].Add(newPoolable);
        }
        else
        {
            // Look for available poolable in existing Pool
            foreach (GameObject poolableObject in _poolablesPool[prefab.name])
            {
                if (poolableObject.activeInHierarchy) continue;

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

            _poolablesPool[prefab.name].Add(newPoolable);
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
