/*
 * SongDefinitionInspector - Description here
 *
 * (c) 2012, Charlie Huguenard
 */

using UnityEngine;
using UnityEditor;
using System;
using System.Collections;
using System.Collections.Generic;

[CustomEditor(typeof(SongDefinition))]
public class SongDefinitionInspector : Editor
{
	public override void OnInspectorGUI ()
	{
		SongDefinition song = (SongDefinition)target;
		
		// Song Name
		song.songName = EditorGUILayout.TextField( "Song Name", song.songName );
		
		EditorGUILayout.BeginHorizontal();
		
		// Add Track
		if( GUILayout.Button( "Add Track" ) )
		{
			song.tracks.Add(  new TrackDefinition() );
			EditorUtility.SetDirty( song );
		}
		
		// Clear Tracks
		if( GUILayout.Button( "Clear Tracks" ) )
		{
			song.tracks = new List<TrackDefinition>();
			EditorUtility.SetDirty( song );
		}
		
		EditorGUILayout.EndHorizontal();
		
		// Display the Tracks
		for( int i = 0; i < song.tracks.Count; i++ )
		{
			DisplayTrack( song.tracks[i], i );
		}
		
		if( GUI.changed ) EditorUtility.SetDirty( song );
	}
	
	void DisplayTrack( TrackDefinition track, int index )
	{
		GUILayout.Label( "Track " + ( index + 1 ));	
		
		EditorGUI.indentLevel += 3;
		
		EditorGUILayout.BeginHorizontal();
		track.file = (AudioClip)EditorGUILayout.ObjectField( track.file, typeof(AudioClip), false );
		track.moodTags = (MoodTag)EditorGUILayout.EnumMaskField( track.moodTags );
		EditorGUILayout.EndHorizontal();
		
		track.intensity = EditorGUILayout.Slider( "Intensity", track.intensity, 0f, 1f );
		
		EditorGUI.indentLevel -= 3;
	}
}