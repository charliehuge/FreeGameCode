/*
 * Track - Description here
 * 
 * (c) 2012, Charlie Huguenard
 */

using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

[Serializable]
public class TrackDefinition
{
	public AudioClip file;
	public MoodTag moodTags = 0;
	public float intensity = 0f; // the intensity at which this track will play
}
