using UnityEngine;
using DG.Tweening;

public class ButtonTweener : MonoBehaviour
{
    [Header("Original values")]
    [SerializeField] private Vector3 _originalScale;
    [Header("Scale Tweening")]
    [SerializeField] private Vector3 _scaleVector;
    [SerializeField] private float _scaleDuration = 0.2f;

    private Tween _scaleTween;

    void OnDisable()
    {
        TweenReset();
    }

    private void TweenScaleUp()
    {
        TweenReset();

        _scaleTween = transform.DOScale(_scaleVector, _scaleDuration).SetUpdate(true);
    }

    public void TweenScaleUpAndShake()
    {
        TweenScaleUp();
    }

    public void TweenReset()
    {
        _scaleTween?.Kill();
        transform.localScale = _originalScale;
    }
}
