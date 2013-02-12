/*
 * Mood - Description here
 * 
 * (c) 2012, Charlie Huguenard
 */

using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

[Flags]
public enum MoodTag
{
	Serene			= 1 << 0,
	Contemplative	= 1 << 1,
	Angry			= 1 << 2,
	Frightened		= 1 << 3,
	Suspenseful		= 1 << 4,
}