/*
 * SongDefinitionAsset - Description here
 *
 * (c) 2012, Charlie Huguenard
 */

using UnityEngine;
using UnityEditor;

public class SongDefinitionAsset
{
	[MenuItem("Assets/DynamicMusic/SongDefinition")]
	public static void CreateAsset()
	{
		ScriptableObjectUtility.CreateAsset<SongDefinition>();	
	}
}