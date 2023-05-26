using System;
using UnityEngine;

public class ButtonInteraction : MonoBehaviour
{
    public static event Action OnButtonHover;
    public static event Action OnButtonPressed;

    public void ButtonHover()
    {
        OnButtonHover?.Invoke();
    }
    public void ButtonPressed()
    {
        OnButtonPressed?.Invoke();
    }
}
