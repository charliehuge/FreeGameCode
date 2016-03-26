using UnityEngine;
using System.Collections;

/// <summary>
/// A slider that will make audio people happy when working in Unity.
/// 
/// Thanks to Aaron, Brad, and Matt for making this: 
/// http://www.playdotsound.com/portfolio-item/decibel-db-to-float-value-calculator-making-sense-of-linear-values-in-audio-tools/
/// </summary>
[RequireComponent(typeof(AudioSource))]
public class AudioVolumeSlider : MonoBehaviour
{
    private const float MinVolume = -60f;

    private const float MaxVolume = 0f;

    [SerializeField, Range(MinVolume, MaxVolume), Tooltip("Volume (dB)")] private float _volume;

    private AudioSource _audioSource;

    public void SetVolume(float volume)
    {
        _volume = volume;
        Refresh();
    }

    private void OnValidate()
    {
        Refresh();
    }

    private void Refresh()
    {
        if (_audioSource == null)
        {
            _audioSource = GetComponent<AudioSource>();
        }

        // guard against out of range values
        if (_volume > MaxVolume)
        {
            _volume = MaxVolume;
        }
        else if (_volume < MinVolume)
        {
            _volume = MinVolume;
        }

        _audioSource.volume = Mathf.Pow(10f, _volume / 20f);
    }
}
