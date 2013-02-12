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
	static DynamicMusic instance;
	
	DynamicMusicController controller;
	
	public static void Init()
	{
		if( instance == null ) instance = new DynamicMusic();
		
		instance.GetController();
	}
	
	void GetController()
	{
		controller = (DynamicMusicController)GameObject.FindObjectOfType( typeof( DynamicMusicController ) );
		if( controller == null )
		{
			GameObject go = new GameObject( "DynamicMusicController", typeof( DynamicMusicController ) );
			controller = go.GetComponent<DynamicMusicController>();
		}
	}
	
	public static void Play( int songIndex )
	{
		instance.controller.Play( songIndex );	
	}
	
	public static MoodTag SetMoods( MoodTag moodTags, bool additive = false )
	{
		MoodTag tags = moodTags;
		
		if( additive )
		{
			tags |= instance.controller.CurrentMoods;
		}
		
		instance.controller.CurrentMoods = tags;
		
		return tags;
	}
	
	public static MoodTag GetMoods()
	{
		return instance.controller.CurrentMoods;	
	}
	
	public static void SetIntensity( float intensity )
	{
		instance.controller.CurrentIntensity = intensity;	
	}
	
	public static float GetIntensity()
	{
		return instance.controller.CurrentIntensity;	
	}
}