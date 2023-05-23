using System.Collections;
using UnityEngine;
using GlobalEnums;

public class EnemyColorManager : MonoBehaviour
{
    [SerializeField] private Renderer _renderer;
    [SerializeField] private MeshRenderer _meshRenderer;
    [SerializeField] private Color _normalColor;
    [SerializeField] private Color _hitColor;
    [SerializeField, Range(0.01f, 1f)]
    private float _hitFXDuration = 0.2f;

    private Coroutine _hitVSFCoroutine;
    private WaitForSeconds _waitForFlashVFX;

    public ThemeColor EnemyColor { get; set; }
    public Material Material
    {
        get => _meshRenderer.material;
        set
        {
            _meshRenderer.material = value;
            _normalColor = _meshRenderer.material.color;
        }
    }

    private void Awake()
    {
        _waitForFlashVFX = new(_hitFXDuration);
    }

    void OnEnable()
    {
        _meshRenderer.material.color = _normalColor;
    }

    void OnDisable()
    {
        if (_hitVSFCoroutine != null)
            StopCoroutine(_hitVSFCoroutine);
    }

    private IEnumerator FlashOnHit()
    {
        _meshRenderer.material.color = _hitColor;
        yield return _waitForFlashVFX;
        _meshRenderer.material.color = _normalColor;
    }

    public void HitVFX()
    {
        if (_hitVSFCoroutine != null)
            StopCoroutine(_hitVSFCoroutine);

        _hitVSFCoroutine = StartCoroutine(FlashOnHit());
    }
}
