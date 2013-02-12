/*
 * TrackObject - Description here
 * 
 * (c) 2012, Charlie Huguenard
 */

using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(AudioSource))]
public class TrackObject : MonoBehaviour 
{
	public enum Fade { None, In, Out }
	
	[HideInInspector]
	public TrackDefinition trackDef;
	
	public event Action<TrackObject> Stopped;
	
	Fade fadeType = Fade.None;
	
	// TODO: Profile this and determine if there's a more efficient way to handle fades
	void Update()
	{
		if( !audio.isPlaying ) return;
		
		if( fadeType == Fade.In )
		{
			if( audio.volume < 0.9f ) audio.volume = Mathf.Lerp( audio.volume, 1f, Time.deltaTime );
			else
			{
				audio.volume = 1f;
				fadeType = Fade.None;
			}
		}
		else if( fadeType == Fade.Out )
		{
			if( audio.volume > 0.1f ) audio.volume = Mathf.Lerp( audio.volume, 0f, Time.deltaTime );
			else
			{
				audio.volume = 0f;
				fadeType = Fade.None;
				Stop();
			}
		}
	}
	
	public void SetTrackDefinition( TrackDefinition trackDefIn )
	{
		audio.clip = trackDefIn.file;
		audio.loop = true;
		this.trackDef = trackDefIn;
	}
	
	public void Play( int timeInSamples = 0 )
	{
		gameObject.active = true;
		gameObject.name = "TrackObject-" + audio.clip.name;
		audio.timeSamples = timeInSamples;
		audio.volume = 0f;
		audio.Play();
		FadeIn();
	}
	
	/// <summary>
	/// Stop audio and disable. Should really use FadeOut from outside this class, though.
	/// Obvious exception when the program wants music to stop immediately.
	/// </summary>
	public void Stop()
	{
		audio.Stop();
		gameObject.active = false;
		gameObject.name = "TrackObject(pooled)";
		if( Stopped != null ) Stopped( this );
	}
	
	public void FadeIn()
	{
		fadeType = Fade.In;
	}
	
	public void FadeOut()
	{
		fadeType = Fade.Out;
	}
}
