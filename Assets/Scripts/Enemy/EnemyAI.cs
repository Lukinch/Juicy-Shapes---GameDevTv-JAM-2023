using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] private NavMeshAgent _navMeshAgent;
    [SerializeField] private float _updateEachSeconds = 0.5f;

    public Transform Player { set; get; }

    Coroutine _updateDestination;
    WaitForSeconds _waitToUpdate;

    void OnEnable()
    {
        _updateDestination = StartCoroutine(UpdateDestination());
    }

    void OnDisable()
    {
        if (_updateDestination != null)
        {
            StopCoroutine(_updateDestination);
            _updateDestination = null;
        }
    }

    IEnumerator UpdateDestination()
    {
        _waitToUpdate = new(_updateEachSeconds);
        while (true)
        {
            if (Player == null)
                yield return null;

            _navMeshAgent.SetDestination(Player.position);
            yield return _waitToUpdate;
        }
    }
}
