using UnityEngine;
using UnityEngine.Audio;
using NaughtyAttributes;
using System;
using Random = UnityEngine.Random;

public class GameplayAudioManager : MonoBehaviour
{
    [Header("Audio Mix Group")]
    [SerializeField] private AudioMixerGroup _audioMixerGroup;

    [Header("Volume")]
    [SerializeField, Range(0f, 1f)] private float _playerFireVolume;

    [SerializeField, Range(0f, 1f)] private float _enemyDamageVolume;

    [SerializeField, Range(0f, 1f)] private float _enemyDeathVolume;


    [Header("Fire Customization")]
    [SerializeField, Range(0, 256)] private int _playerFirePriority;
    [SerializeField] private AudioClip _playerFireClip;
    [SerializeField, MinMaxSlider(-3.0f, 3.0f)] private Vector2 _playerFirePitchRange;
    [Header("Enemy Damage Customization")]
    [SerializeField, Range(0, 256)] private int _enemyDamagePriority;
    [SerializeField] private AudioClip _enemyDamageClip;
    [SerializeField, MinMaxSlider(-3.0f, 3.0f)] private Vector2 _enemyDamagePitchRange;
    [Header("Enemy Death Customization")]
    [SerializeField, Range(0, 256)] private int _enemyDeathPriority;
    [SerializeField] private AudioClip _enemyDeathClip;
    [SerializeField, MinMaxSlider(-3.0f, 3.0f)] private Vector2 _enemyDeathPitchRange;

    void OnEnable()
    {
        PlayerFire.OnPlayerFired += PlayerFire_OnPlayerFired;
        EnemyHealth.OnEnemyDamaged += EnemyHealth_OnEnemyDamaged;
        EnemyHealth.OnEnemyDiedPosition += EnemyHealth_OnEnemyDiedPosition;
    }

    void OnDisable()
    {
        PlayerFire.OnPlayerFired -= PlayerFire_OnPlayerFired;
        EnemyHealth.OnEnemyDamaged -= EnemyHealth_OnEnemyDamaged;
        EnemyHealth.OnEnemyDiedPosition -= EnemyHealth_OnEnemyDiedPosition;
    }

    private void PlayerFire_OnPlayerFired(Vector3 position)
    {
        PlayClipAtPoint(_playerFireClip, position, _playerFirePitchRange.x, _playerFirePitchRange.y, _playerFireVolume);
    }

    private void EnemyHealth_OnEnemyDamaged(Vector3 position)
    {
        // Deactivated because there were waaaaaayy too many sounds and it was bugging out
        // PlayClipAtPoint(_enemyDamageClip, position, _enemyDamagePitchRange.x, _enemyDamagePitchRange.y, _enemyDamageVolume);
    }

    private void EnemyHealth_OnEnemyDiedPosition(Vector3 position)
    {
        PlayClipAtPoint(_enemyDeathClip, position, _enemyDeathPitchRange.x, _enemyDeathPitchRange.y, _enemyDeathVolume);
    }

    private void PlayClipAtPoint(AudioClip clip, Vector3 position, float minPitch, float maxPitch, float volume = 1)
    {
        GameObject gameObject = new("One shot audio");
        gameObject.transform.position = position;
        AudioSource audioSource = (AudioSource)gameObject.AddComponent(typeof(AudioSource));
        audioSource.clip = clip;
        audioSource.spatialBlend = 1f;
        audioSource.volume = volume;
        audioSource.outputAudioMixerGroup = _audioMixerGroup;

        if (minPitch >= -3.0f && maxPitch <= 3.0f)
            audioSource.pitch = Random.Range(minPitch, maxPitch);

        audioSource.Play();
        Destroy(gameObject, clip.length * ((Time.timeScale < 0.01f) ? 0.01f : Time.timeScale));
    }

}
