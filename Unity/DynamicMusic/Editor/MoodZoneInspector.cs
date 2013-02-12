using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(MoodZone))]
public class MoodZoneInspector : Editor
{
	public override void OnInspectorGUI()
	{
		MoodZone mz = (MoodZone)target;
		
		mz.moodTags = (MoodTag)EditorGUILayout.EnumMaskField( 
			new GUIContent( "Mood Tags", "The tags this MoodZone will activate on entering" )
			, mz.moodTags );
	}
}