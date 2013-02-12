/*
 * DynamicMusic - The interface by which implementers will be hooking up gameplay
 *
 * (c) 2012, Charlie Huguenard
 */

using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class DynamicMusic
{
	static DynamicMusicController controller;
	public static DynamicMusicController Controller
	{
		get
		{
			// if we already have the controller, return it
			if( controller != null ) return controller;
			
			// if we don't, create one and return it
			GameObject go = new GameObject( "DynamicMusicController", typeof( DynamicMusicController ) );
			controller = go.GetComponent<DynamicMusicController>();
			return controller;
		}
	}
	
	private DynamicMusic(){}
	
	public static void Play( int songIndex )
	{
		Controller.Play( songIndex );	
	}
	
	public static void Play( string songName )
	{
		for( int i = 0; i < Controller.songs.Count; i++ )
		{
			SongInstance song = Controller.songs[i];
			if( song.definition.songName == songName )
			{
				Controller.Play( i );
				return;
			}
		}
		
		Debug.LogError( "No song with name " + songName );
	}
	
	public static MoodTag SetMoods( MoodTag moodTags, bool additive = false )
	{
		MoodTag tags = moodTags;
		
		if( additive )
		{
			tags |= Controller.CurrentMoods;
		}
		
		Controller.CurrentMoods = tags;
		
		return tags;
	}
	
	public static MoodTag CancelMoods( MoodTag moodTags )
	{
		Controller.CurrentMoods &= ~moodTags;
		return Controller.CurrentMoods;
	}
	
	public static MoodTag GetMoods()
	{
		return Controller.CurrentMoods;	
	}
	
	public static void SetIntensity( float intensity )
	{
		Controller.CurrentIntensity = intensity;	
	}
	
	public static float GetIntensity()
	{
		return Controller.CurrentIntensity;	
	}
}