using UnityEngine;
using System.Collections;

public class LoopSync : MonoBehaviour
{
    public float tempo = 120;
    public int beatsPerBar = 4;
	public float fadeTime = 0.5f;
    public AudioClip currentLoop;
    public AudioClip nextLoop;

    private AudioSource[] audioSources;
    private int audioSourceIdx = 0;

    private void Start()
    {
        if (currentLoop == null)
        {
            Debug.LogError("No loop set to play at start!");
            return;
        }

        audioSources = new AudioSource[]
        {
            new GameObject("Audio Source 1", typeof(AudioSource)).audio,
            new GameObject("Audio Source 2", typeof(AudioSource)).audio,
        };

        audioSources[audioSourceIdx].clip = currentLoop;
        audioSources[audioSourceIdx].loop = true;
        audioSources[audioSourceIdx].Play();
    }

    private void Update()
    {
        if (nextLoop != null)
        {
            // get the next bar offset
			int samplesElapsed = audioSources[audioSourceIdx].timeSamples;
			int samplesPerBeat = (int)(currentLoop.frequency * 60 / tempo); // possible truncation here
			int samplesPerBar = samplesPerBeat * beatsPerBar;
			int sampleInBar = samplesElapsed % samplesPerBar;
			int samplesUntilNextBar = samplesPerBar - sampleInBar;

			// start playing the next loop with a negative offset (play from before the end of the loop)
			int nextAudioSourceIdx = (audioSourceIdx + 1) % audioSources.Length;
			audioSources[nextAudioSourceIdx].clip = nextLoop;
			audioSources[nextAudioSourceIdx].loop = true;
			audioSources[nextAudioSourceIdx].timeSamples = nextLoop.samples - samplesUntilNextBar;
			audioSources[nextAudioSourceIdx].Play();

			// trigger the fade
			StartCoroutine(DoFade(nextAudioSourceIdx, audioSourceIdx));

			// advance the current loop
			currentLoop = nextLoop;
			nextLoop = null;

			// advance the audio source currently in use
			audioSourceIdx = nextAudioSourceIdx;
		}
    }
	
	private IEnumerator DoFade(int fadeInIdx, int fadeOutIdx)
	{
		float fadeStartTime = Time.time;
		float fadeEndTime = fadeStartTime + fadeTime;

		audioSources[fadeInIdx].volume = 0f;
		audioSources[fadeOutIdx].volume = 1f;

		while (Time.time < fadeEndTime)
		{
			// linear fade...ewww. Probably wanna do the math for an equal power fade or something.
			// I'll leave that to you :)
			audioSources[fadeInIdx].volume = (Time.time - fadeStartTime) / fadeTime;
			audioSources[fadeOutIdx].volume = (fadeEndTime - Time.time) / fadeTime;
			yield return null;
		}

		audioSources[fadeInIdx].volume = 1f;
		audioSources[fadeOutIdx].volume = 0f;
	}

    private void OnGUI()
    {
        GUI.Label(new Rect(10,10,300, 300), "Drop a new audio file into the \"Next Loop\" slot in the Loop Sync component on Main Camera");
    }
}
