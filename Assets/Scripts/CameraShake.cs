using UnityEngine;
using NaughtyAttributes;
using DG.Tweening;

public class CameraShake : MonoBehaviour
{
    [SerializeField] private Camera _camera;

    [Header("Tweening")]
    [SerializeField] private float _duration = 0.5f;
    [SerializeField] private float _strength = 0.2f;
    [SerializeField] private int _vibrato = 30;
    [SerializeField] private float _randomness = 90f;
    [SerializeField] private bool _fadeOut = true;
    [SerializeField] private ShakeRandomnessMode _randomnessMode = ShakeRandomnessMode.Harmonic;

    void OnEnable()
    {
        PlayerHealth.OnPlayerDamaged += ShakeCamera;
    }

    void OnDisable()
    {
        PlayerHealth.OnPlayerDamaged -= ShakeCamera;
    }

    private void ShakeCamera()
    {
        _camera.DOShakePosition(_duration, _strength, _vibrato, _randomness, _fadeOut, _randomnessMode);
    }
}
