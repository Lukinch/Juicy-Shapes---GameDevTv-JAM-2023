using System.Collections;
using UnityEngine;

public class ParticlePoolable : MonoBehaviour, IPoolable
{
    [SerializeField] private float _particleTime;
    public PoolingManager PoolingManagerSO { get; set; }

    private void Start()
    {
        StartCoroutine(WaitForVFXToEnd());
    }

    private IEnumerator WaitForVFXToEnd()
    {
        yield return new WaitForSeconds(_particleTime);
        PoolingManagerSO.ReturnObject(gameObject);
    }
}
