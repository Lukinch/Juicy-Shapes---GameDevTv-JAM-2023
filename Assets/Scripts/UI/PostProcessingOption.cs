using UnityEngine;
using UnityEngine.UI;

public class PostProcessingOption : MonoBehaviour
{
    [SerializeField] private GameObject _postProcessingVolume;
    [SerializeField] private Toggle _toggleToUpdate;

    private bool _enabled;

    void Start()
    {
        _enabled = AudioSettingsManager.Instance.PostProcessingEnabled;
        _toggleToUpdate.SetIsOnWithoutNotify(_enabled);
        _postProcessingVolume.SetActive(_enabled);
    }

    public void TogglePostProcessing()
    {
        _enabled = !_enabled;
        AudioSettingsManager.Instance.PostProcessingEnabled = _enabled;
        _postProcessingVolume.SetActive(_enabled);
    }
}