/*
 * TrackInstance - Description here
 *
 * (c) 2012, Charlie Huguenard
 */

using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class TrackInstance
{
	public SongInstance songInstance;
	
	TrackObject trackObject;
	public TrackObject TrackObject { get { return trackObject; } }
	
	TrackDefinition definition;
	
	public event Action<TrackInstance> TrackObjectStopped;
	
	public TrackInstance( TrackDefinition trackDef, SongInstance song )
	{
		definition = trackDef;
		songInstance = song;
	}
	
	public void SetTrackObject( TrackObject trackObj )
	{
		D.Assert( trackObj != null, "TrackObject is null" );
		trackObject = trackObj;
		trackObject.SetTrackDefinition( definition );
		trackObject.Stopped += OnTrackObjectStopped;
	}
	
	public void Play( int timeInSamples = 0 )
	{
		trackObject.Play( timeInSamples );	
	}
	
	public void Stop()
	{
		trackObject.Stop();
	}
	
	public bool TrackShouldPlay( MoodTag moodTags, float intensity )
	{
		bool hasActiveMood = ( definition.moodTags & moodTags ) != 0;
		bool isUnderIntensityThreshold = definition.intensity <= intensity;
		return hasActiveMood && isUnderIntensityThreshold;
	}
	
	public bool TrackIsPlaying()
	{
		return trackObject != null && trackObject.audio.isPlaying;	
	}

	void OnTrackObjectStopped (TrackObject obj)
	{
		trackObject.Stopped -= OnTrackObjectStopped;
		if( TrackObjectStopped != null ) TrackObjectStopped( this );
		trackObject = null;
	}
}