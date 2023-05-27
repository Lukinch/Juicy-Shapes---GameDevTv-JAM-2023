using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuAudioManager : MonoBehaviour
{
    [SerializeField] private AudioSource _UIAudioSource;
    [SerializeField] private AudioClip _buttonHoverClip;
    [SerializeField] private AudioClip _buttonPressedClip;

    void OnEnable()
    {
        ButtonInteraction.OnButtonHover += ButtonInteraction_OnButtonHover;
        ButtonInteraction.OnButtonPressed += ButtonInteraction_OnButtonPressed;
    }

    void OnDisable()
    {
        ButtonInteraction.OnButtonHover -= ButtonInteraction_OnButtonHover;
        ButtonInteraction.OnButtonPressed -= ButtonInteraction_OnButtonPressed;
    }

    private void ButtonInteraction_OnButtonHover()
    {
        _UIAudioSource.PlayOneShot(_buttonHoverClip);
    }

    private void ButtonInteraction_OnButtonPressed()
    {
        _UIAudioSource.PlayOneShot(_buttonPressedClip);
    }
}
