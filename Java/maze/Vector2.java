//------------------------------------------------------------------------------
//
//  Vector2.java
//
//  © 2011 Charlie Huguenard
//
//------------------------------------------------------------------------------
package maze;

public class Vector2 {
    public double x, y;
    
    public Vector2(double x, double y) {
        this.x = x;
        this.y = y;
    }
    
    public static Vector2 Zero() {
        return new Vector2(0.0,0.0);
    }
    
    public static Vector2 One() {
        return new Vector2(1.0,1.0);
    }
    
    public static Vector2 Up() {
        return new Vector2(0.0,-1.0);
    }
    
    public static Vector2 Down() {
        return new Vector2(0.0,1.0);
    }
    
    public static Vector2 Left() {
        return new Vector2(-1.0,0.0);
    }
    
    public static Vector2 Right() {
        return new Vector2(1.0,0.0);
    }
    
    public static Vector2 Add(Vector2 v1, Vector2 v2) {
        return new Vector2(v1.x + v2.x, v1.y + v2.y);
    }
    
    public static Vector2 Subtract(Vector2 v1, Vector2 v2) {
        return new Vector2(v1.x - v2.x, v1.y - v2.y);
    }
    
    public static Vector2 Scale(Vector2 v, double scalar) {
        return new Vector2(v.x * scalar, v.y * scalar);
    }
    
    public static double Dot(Vector2 v1, Vector2 v2) {
        return v1.x * v2.x + v1.y * v2.y;
    }
}
