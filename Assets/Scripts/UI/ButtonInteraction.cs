using System;
using UnityEngine;
using UnityEngine.UI;

public class ButtonInteraction : MonoBehaviour
{
    [SerializeField] Button _button;
    public static event Action OnButtonHover;
    public static event Action OnButtonPressed;

    public void ButtonHover()
    {
        if (_button != null && _button.interactable)
            OnButtonHover?.Invoke();
    }
    public void ButtonPressed()
    {
        if (_button != null && _button.interactable)
            OnButtonPressed?.Invoke();
    }

    [ContextMenu("Get Button")]
    void GetVisualTransform()
    {
        _button = GetComponent<Button>();
    }
}
