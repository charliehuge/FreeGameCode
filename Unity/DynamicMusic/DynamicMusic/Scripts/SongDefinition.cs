/*
 * SongDefinition - Description here
 *
 * (c) 2012, Charlie Huguenard
 */

using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class SongDefinition : ScriptableObject
{
	public string songName;
	public List<TrackDefinition> tracks;
	
	public SongDefinition()
	{
		tracks = new List<TrackDefinition>();
		songName = "New Song " + DateTime.Now.ToLongTimeString();
	}
}
