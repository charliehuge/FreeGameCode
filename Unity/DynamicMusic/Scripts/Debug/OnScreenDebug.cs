using UnityEngine;
using System.Collections;

public class OnScreenDebug : MonoBehaviour 
{
	void OnGUI()
	{
		GUI.Label( Rect.MinMaxRect(0,0,300,100), DynamicMusic.GetMoods().ToString() );
	}
}
