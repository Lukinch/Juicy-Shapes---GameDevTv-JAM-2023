using UnityEngine;

public class EnemyPooling : MonoBehaviour, IPoolable
{
    [SerializeField] private EnemyColorManager _enemyColorManager;
    [SerializeField] private GameObject _deathVFXRed;
    [SerializeField] private GameObject _deathVFXPink;
    [SerializeField] private GameObject _deathVFXLightblue;

    public PoolingManager PoolingManagerSO { get; set; }

    public void ReturnToPool()
    {
        SpawnVFX();
        PoolingManagerSO.ReturnObject(gameObject);
    }

    private void SpawnVFX()
    {
        switch (_enemyColorManager.EnemyColor)
        {
            case GlobalEnums.ThemeColor.Red:
                Instantiate(_deathVFXRed, transform.position, Quaternion.identity);
                break;
            case GlobalEnums.ThemeColor.Pink:
                Instantiate(_deathVFXPink, transform.position, Quaternion.identity);
                break;
            case GlobalEnums.ThemeColor.LightBlue:
                Instantiate(_deathVFXLightblue, transform.position, Quaternion.identity);
                break;
        }
    }
}
