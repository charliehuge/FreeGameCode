using System;
using System.Diagnostics;

public class D
{
	public static void Assert( bool condition, string message )
	{
		if( !condition ) throw new Exception( message );
	}
}