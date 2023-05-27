using System.Collections;
using UnityEngine;
using UnityEngine.Splines;

public class MainMenuSplineObjectsManager : MonoBehaviour
{
    [SerializeField, Min(0f)] private float _timeBetweenObjectActivation = 0.1f;
    [SerializeField] private SplineAnimate[] _splineAnimate;

    void Start()
    {
        StartCoroutine(StartObjectsAwakening());
    }

    private IEnumerator StartObjectsAwakening()
    {
        for (int i = 0; i < _splineAnimate.Length; i++)
        {
            _splineAnimate[i].Play();
            if (i + 1 == _splineAnimate.Length - 1)
                yield return new WaitForSeconds(_timeBetweenObjectActivation + 0.05f);
            else
                yield return new WaitForSeconds(_timeBetweenObjectActivation);
        }
    }
}
