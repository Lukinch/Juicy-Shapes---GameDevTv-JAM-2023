using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UIPlayerHealthBar : MonoBehaviour
{
    [SerializeField] private Image _ghostHealthBar;
    [SerializeField] private Image _healthBar;
    [SerializeField] private float _barsEaseSpeed = 2f;
    [SerializeField] private float _timeToTriggerGhostBarUpdate = 1f;

    private float _currentHealth;
    private WaitForEndOfFrame _waitForEndOfFrame;
    private Coroutine _waitToUpdateGhostBarCoroutine;
    private Coroutine _updateHealthBarCoroutine;

    private void Awake()
    {
        _waitForEndOfFrame = new();
        ResetHealthValues();
    }

    private void OnEnable()
    {
        PlayerHealth.OnPlayerHealthChanged += OnHealthChanged;
        PlayerHealth.OnResetHealth += ResetHealthValues;
    }

    private void OnDisable()
    {
        PlayerHealth.OnPlayerHealthChanged -= OnHealthChanged;
        PlayerHealth.OnResetHealth -= ResetHealthValues;

        if (_waitToUpdateGhostBarCoroutine != null)
            StopCoroutine(_waitToUpdateGhostBarCoroutine);
        if (_updateHealthBarCoroutine != null)
            StopCoroutine(_updateHealthBarCoroutine);
    }

    private void ResetHealthValues()
    {
        _currentHealth = _healthBar.fillAmount = _ghostHealthBar.fillAmount = 1;
    }

    private void UpdateHealthVisuals()
    {
        if (_updateHealthBarCoroutine != null) StopCoroutine(_updateHealthBarCoroutine);
        if (_waitToUpdateGhostBarCoroutine != null) StopCoroutine(_waitToUpdateGhostBarCoroutine);

        _updateHealthBarCoroutine = StartCoroutine(UpdateHealthBar());
    }

    private IEnumerator UpdateHealthBar()
    {
        while (_healthBar.fillAmount != _currentHealth)
        {
            _healthBar.fillAmount =
                Mathf.MoveTowards(_healthBar.fillAmount, _currentHealth, _barsEaseSpeed * Time.deltaTime);

            yield return _waitForEndOfFrame;
        }

        if (_ghostHealthBar.fillAmount != _healthBar.fillAmount && _healthBar.fillAmount == _currentHealth)
            _waitToUpdateGhostBarCoroutine = StartCoroutine(WaitToUpdateGhostBar());
    }
    private IEnumerator WaitToUpdateGhostBar()
    {
        yield return new WaitForSeconds(_timeToTriggerGhostBarUpdate);
        StartCoroutine(UpdateGhostBar());
    }
    private IEnumerator UpdateGhostBar()
    {
        while (_ghostHealthBar.fillAmount != _currentHealth)
        {
            _ghostHealthBar.fillAmount =
                Mathf.MoveTowards(_ghostHealthBar.fillAmount, _currentHealth, _barsEaseSpeed * Time.deltaTime);

            yield return _waitForEndOfFrame;
        }
    }

    private void OnHealthChanged(float currentUnitHealth, float maxHealthPoints)
    {
        _currentHealth = currentUnitHealth == 0 ? 0f : currentUnitHealth / maxHealthPoints;

        UpdateHealthVisuals();
    }
}
