using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIColorChangeManager : MonoBehaviour
{
    [Header("Input")]
    [SerializeField, Tooltip("Scriptable Object from which each input is read")]
    private InputReaderSO _inputReader;

    [Header("Buttons")]
    [SerializeField] private GameObject _redIndicator;
    [SerializeField] private GameObject _pinkIndicator;
    [SerializeField] private GameObject _lightBlueIndicator;

    private void OnEnable()
    {
        _inputReader.OnColorRedPressed += Input_OnRedPressed;
        _inputReader.OnColorPinkPressed += Input_OnPinkPressed;
        _inputReader.OnColorLightBluePressed += Input_OnLightBluePressed;
    }

    private void OnDisable()
    {
        _inputReader.OnColorRedPressed -= Input_OnRedPressed;
        _inputReader.OnColorPinkPressed -= Input_OnPinkPressed;
        _inputReader.OnColorLightBluePressed -= Input_OnLightBluePressed;
    }

    private void Input_OnRedPressed()
    {
        HideAllIndicators();
        _redIndicator.SetActive(true);
    }

    private void Input_OnPinkPressed()
    {
        HideAllIndicators();
        _pinkIndicator.SetActive(true);
    }

    private void Input_OnLightBluePressed()
    {
        HideAllIndicators();
        _lightBlueIndicator.SetActive(true);
    }

    private void HideAllIndicators()
    {
        _redIndicator.SetActive(false);
        _pinkIndicator.SetActive(false);
        _lightBlueIndicator.SetActive(false);
    }
}
