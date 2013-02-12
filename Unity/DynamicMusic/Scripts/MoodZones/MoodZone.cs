using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

[RequireComponent( typeof( SphereCollider ) )]
public class MoodZone : MonoBehaviour 
{
	public MoodTag moodTags;
	
	bool shouldUpdate = false;
	SphereCollider sc;
	Collider playerCollider;
	
	void Start()
	{
		if( sc == null ) sc = GetComponent<SphereCollider>();
	}
	
	void OnTriggerEnter( Collider c )
	{
		Debug.Log( "Enter" );
		if( c.gameObject.CompareTag( "Player" ) )
		{
			shouldUpdate = true;
			playerCollider = c;
		}
	}
	
	void OnTriggerExit( Collider c )
	{
		if( c.gameObject.CompareTag( "Player" ) )
		{
			shouldUpdate = false;
			DynamicMusic.CancelMoods( moodTags );
		}
	}
	
	void Update()
	{
		if( !shouldUpdate ) return;
		
		float r = sc.radius;
		float d = Vector3.Distance( playerCollider.transform.position, transform.position );
		float pct = ( r - d ) / r;
		DynamicMusic.SetIntensity( pct );
		
		DynamicMusic.SetMoods( moodTags, true );
	}
	
	void OnDrawGizmos()
	{
		Gizmos.DrawIcon( transform.position, "mood.png" );	
		
		Gizmos.color = Color.magenta;
		if( sc != null ) Gizmos.DrawWireSphere( transform.position, sc.radius );
	}
}
