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

    [Range(MinVolume, MaxVolume), Tooltip("Volume (dB)")] public float Volume;

    private AudioSource _audioSource;

    private void OnValidate()
    {
        if (_audioSource == null)
        {
            _audioSource = GetComponent<AudioSource>();
        }

        // guard against out of range values
        if (Volume > MaxVolume)
        {
            Volume = MaxVolume;
        }
        else if (Volume < MinVolume)
        {
            Volume = MinVolume;
        }

        _audioSource.volume = Mathf.Pow(10f, Volume / 20f);
    }
}
