/*
 * SongInstance - Description here
 *
 * (c) 2012, Charlie Huguenard
 */

using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class SongInstance
{
	public SongDefinition definition;
	public List<TrackInstance> tracks;
	
	public SongInstance( SongDefinition songDef )
	{
		definition = songDef;
		
		tracks = new List<TrackInstance>();
		
		foreach( TrackDefinition trackDef in definition.tracks )
		{
			tracks.Add( new TrackInstance( trackDef, this ) );	
		}
	}
}