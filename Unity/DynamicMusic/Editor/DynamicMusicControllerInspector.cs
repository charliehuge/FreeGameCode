/*
 * DynamicMusicControllerInspector - Description here
 * 
 * (c) 2012, Charlie Huguenard
 */

using UnityEngine;
using UnityEditor;
using System;
using System.Collections;
using System.Collections.Generic;

[CustomEditor(typeof(DynamicMusicController))]
public class DynamicMusicControllerInspector : Editor 
{
	int selectedSongIndex = 0;
	string[] songNames;
	
	void UpdateSongNames( List<SongInstance> songs )
	{
		songNames = new string[songs.Count];
		for( int i = 0; i < songs.Count; i++ )
		{
			songNames[i] = songs[i].definition.songName;
		}
	}
	
	public override void OnInspectorGUI ()
	{
		DynamicMusicController controller = (DynamicMusicController)target;
		
		controller.CurrentMoods = (MoodTag)EditorGUILayout.EnumMaskField( controller.CurrentMoods );
		
		controller.CurrentIntensity = EditorGUILayout.Slider( "Intensity", controller.CurrentIntensity, 0f, 1f );
		
		if( controller.songs.Count > 0 )
		{
			GUILayout.BeginHorizontal();
			if( GUILayout.Button( "Play!" ) ) controller.Play( selectedSongIndex );
			if( GUILayout.Button( "Stop!" ) ) controller.Stop();
			GUILayout.EndHorizontal();
			
			UpdateSongNames( controller.songs );
			selectedSongIndex = EditorGUILayout.Popup( selectedSongIndex, songNames);
		}
	}
}
