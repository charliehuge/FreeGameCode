/*
 * Test - Description here
 *
 * (c) 2012, Charlie Huguenard
 */

using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class Test : MonoBehaviour 
{
	void Start () 
	{
		DynamicMusic.Init();
	}
	
	void OnGUI()
	{
		if( GUILayout.Button( "Play" ) )
		{
			DynamicMusic.Play( 0 );
		}
	}
}
