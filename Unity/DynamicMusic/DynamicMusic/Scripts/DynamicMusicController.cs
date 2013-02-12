/*
 * DynamicMusicController - Description here
 * 
 * (c) 2012, Charlie Huguenard
 */

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DynamicMusicController : MonoBehaviour 
{
	MoodTag currentMoods = 0;
	public MoodTag CurrentMoods
	{
		get { return currentMoods; }
		set
		{
			currentMoods = value;
			UpdatePlayingTracks();
		}
	}
	
	float currentIntensity = 0f;
	public float CurrentIntensity
	{
		get { return currentIntensity; }
		set
		{
			currentIntensity = Mathf.Clamp( value, 0f, 1f );
			UpdatePlayingTracks();
		}
	}
	
	[HideInInspector]
	public List<SongInstance> songs;
	
	SongInstance currentSong;
	
	List<TrackInstance> playingTrackInstances;
	List<TrackObject> pooledTrackObjects;
	
	void Awake()
	{
		LoadSongs();
		playingTrackInstances = new List<TrackInstance>();
		pooledTrackObjects = new List<TrackObject>();
		StartCoroutine( PeriodicResync() );
	}
	
	/// <summary>
	/// Play the specified songIndex.
	/// </summary>
	/// <param name='songIndex'>
	/// Song index.
	/// </param>
	public void Play( int songIndex )
	{	
		currentSong = songs[songIndex];
		
		if( playingTrackInstances.Count > 0 )
		{
			TrackInstance[] copy = new TrackInstance[playingTrackInstances.Count];
			playingTrackInstances.CopyTo( copy );
			foreach( TrackInstance track in copy )
			{
				track.TrackObject.FadeOut();
			}
		}
		
		UpdatePlayingTracks();
	}
	
	/// <summary>
	/// Stop this instance.
	/// </summary>
	public void Stop()
	{
		currentSong = null;
		
		if( playingTrackInstances.Count > 0 )
		{
			TrackInstance[] copy = new TrackInstance[playingTrackInstances.Count];
			playingTrackInstances.CopyTo( copy );
			foreach( TrackInstance track in copy )
			{
				track.Stop();
			}
		}		
	}
	
	/// <summary>
	/// Resync every few seconds to ensure loops don't get out of sync
	/// </summary>
	IEnumerator PeriodicResync()
	{
		while( true )
		{
			yield return new WaitForSeconds( 2f );
			ResyncCurrentSong();
		}
	}
	
	/// <summary>
	/// Resyncs the current song.
	/// </summary>
	void ResyncCurrentSong()
	{
		TrackInstance master = null;
		
		if( playingTrackInstances.Count > 0 )
		{
			// look for playing tracks from this song
			// (start from the end, because that's the most likely place to find them)
			for( int i = playingTrackInstances.Count - 1; i >=0; i-- )
			{
				if( playingTrackInstances[i].TrackIsPlaying() 
					&& playingTrackInstances[i].songInstance == currentSong )
				{
					master = playingTrackInstances[i];
					break;
				}
			}
		}
		
		if( master != null )
		{
			int posToSyncTo = master.TrackObject.audio.timeSamples;
			foreach( TrackInstance track in playingTrackInstances )
			{
				if( track.TrackIsPlaying() 
					&& track != master 
					&& track.songInstance == currentSong )
				{
					track.TrackObject.audio.timeSamples = posToSyncTo;
				}	
			}
		}
	}
	
	/// <summary>
	/// Loads SongDefinitions from the resources folder
	/// </summary>
	void LoadSongs()
	{
		Debug.Log("Loading Songs");
		Object[] songDefsFromDisk = Resources.LoadAll( "SongDefinitions", typeof( SongDefinition ) );
		
		if( songDefsFromDisk != null )
		{
			Debug.Log( "Found " + songDefsFromDisk.Length + " songs" );
			songs = new List<SongInstance>();
			foreach( Object songDef in songDefsFromDisk )
			{
				SongInstance newSong = new SongInstance( (SongDefinition)songDef );
				songs.Add( newSong );
			}
		}
		else
		{
			Debug.LogWarning( "No songs loaded" );	
		}
	}
	
	/// <summary>
	/// Plays and fades tracks based on mood and intensity
	/// </summary>
	void UpdatePlayingTracks()
	{
		if( currentSong == null || currentSong.tracks.Count == 0 ) return;
		
		foreach( TrackInstance track in currentSong.tracks )
		{
			// fade the track in if it should play and it's not already playing
			if( track.TrackShouldPlay( currentMoods, currentIntensity ) )
			{
				if( track.TrackIsPlaying() )
				{
					track.TrackObject.FadeIn();
				}
				else
				{
					SetupTrack( track );
				}
			}
			// fade the track out if it's playing and it shouldn't play
			else if( track.TrackIsPlaying()
				&& !track.TrackShouldPlay( currentMoods, currentIntensity ) )
			{
				track.TrackObject.FadeOut();
			}
		}
	}
	
	/// <summary>
	/// Creates the new track or uses a pooled one, then plays it.
	/// </summary>
	/// <param name='trackInstance'>
	/// Track instance.
	/// </param>
	void SetupTrack( TrackInstance trackInstance )
	{
		// if there are pooled track objects, grab one and use it
		if( pooledTrackObjects.Count > 0 )
		{
			// find a pooled object
			int lastPooledTrackIdx = pooledTrackObjects.Count - 1;
			TrackObject objToUse = pooledTrackObjects[lastPooledTrackIdx];
			pooledTrackObjects.RemoveAt( lastPooledTrackIdx );
			// attach it to this track instance
			trackInstance.SetTrackObject( objToUse );
		}
		// otherwise, create a new one
		else
		{
			CreateNewTrackObject( trackInstance );
		}
		
		trackInstance.Play( GetSyncTime( trackInstance ) );
		
		// add this instance to the playing instances
		playingTrackInstances.Add( trackInstance );
	}
	
	/// <summary>
	/// Finds out what sample this track should sync to
	/// </summary>
	/// <returns>
	/// Sync time in samples
	/// </returns>
	/// <param name='trackInstance'>
	/// Track instance.
	/// </param>
	int GetSyncTime( TrackInstance trackInstance )
	{
		int posInSamples = 0;
		
		if( playingTrackInstances.Count > 0 )
		{
			// look for playing tracks from this song
			// (start from the end, because that's the most likely place to find them)
			for( int i = playingTrackInstances.Count - 1; i >=0; i-- )
			{
				if( playingTrackInstances[i].TrackIsPlaying() 
					&& playingTrackInstances[i].songInstance == currentSong )
				{
					posInSamples = playingTrackInstances[i].TrackObject.audio.timeSamples;
					break;
				}
			}
		}
		
		return posInSamples;
	}
	
	/// <summary>
	/// Creates a new track object.
	/// </summary>
	/// <param name='trackInstance'>
	/// Track instance.
	/// </param>
	void CreateNewTrackObject( TrackInstance trackInstance )
	{
		// make the new GameObject
		GameObject go = new GameObject( "TrackObject", typeof( TrackObject ) );
		TrackObject objToUse = go.GetComponent<TrackObject>();
		// attach it to this track instance
		trackInstance.SetTrackObject( objToUse );
		// listen for the track object to stop playing
		// will happen when it fully fades out to minimize playing AudioSources
		trackInstance.TrackObjectStopped += OnTrackStopped;
	}
	
	/// <summary>
	/// Returns a stopped track to the pool
	/// </summary>
	/// <param name='instance'>
	/// Instance.
	/// </param>
	void OnTrackStopped( TrackInstance instance )
	{
		pooledTrackObjects.Add( instance.TrackObject );
		playingTrackInstances.Remove( instance );
	}
}
