using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using DG.Tweening;

public class EnemyTweener : MonoBehaviour
{
    [SerializeField]
    private Transform _objectToTween;
    [SerializeField]
    private bool _shouldTween = true;

    [Header("Scale Tweening")]
    [SerializeField]
    private Ease _scaleEase = Ease.OutQuint;
    [SerializeField]
    private Vector3 _originalScale;
    [SerializeField]
    private Vector3 _targetScaleMin;
    [SerializeField]
    private Vector3 _targetScaleMax;
    [SerializeField, Min(0.01f)]
    private float _scaleTweenDuration = 0.2f;

    [Header("Rotation Tweening")]
    [SerializeField]
    private Ease _rotationEase = Ease.OutQuint;
    [SerializeField, MinMaxSlider(-90f, 90f)]
    private Vector2 _rotationRange;
    [SerializeField, Min(0.01f)]
    private float _rotationTweenDuration = 0.2f;

    private Vector3 _finalScale = new();
    private Vector3 _finalRotation = new();
    private float _finalRotationValue = 0.0f;

    private Tween _scaleTween;
    private Tween _rotationTween;

    public void ExecuteTweener()
    {
        _scaleTween?.Kill();
        _rotationTween?.Kill();

        if (!_shouldTween) return;

        _objectToTween.localScale = _originalScale;
        _objectToTween.rotation = Quaternion.identity;

        _finalScale.x = Random.Range(_targetScaleMin.x, _targetScaleMax.x);
        _finalScale.z = Random.Range(_targetScaleMin.z, _targetScaleMax.z);
        _finalRotationValue = Random.Range(_rotationRange.x, _rotationRange.y);
        _finalRotation.y = _finalRotationValue;

        _scaleTween = _objectToTween.DOScale(_finalScale, _scaleTweenDuration / 2)
            .SetEase(_scaleEase)
            .OnComplete(() =>
            {
                _objectToTween.DOScale(_originalScale, _scaleTweenDuration / 2).SetEase(_scaleEase);
            })
            .Play();

        _rotationTween = _objectToTween.DORotate(_finalRotation, _rotationTweenDuration / 2).SetEase(_rotationEase)
            .OnComplete(() =>
            {
                _objectToTween.DORotate(Vector3.zero, _rotationTweenDuration / 2).SetEase(_rotationEase);
            })
            .Play();
    }
}
