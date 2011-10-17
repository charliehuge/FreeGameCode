//------------------------------------------------------------------------------
//
//  Tile.java
//
//  © 2011 Charlie Huguenard
//
//------------------------------------------------------------------------------
package maze;

import java.awt.Color;
import java.awt.Graphics;
import maze.Enums.*;

public class Tile {    
    public short[] dirAttempted;
    private TileType type;
    private Tile[] neighbors;
    public boolean[] walls;
    
    public Tile() {
        type = TileType.NORMAL;
        
        // reset direction attempts + neighbors
        dirAttempted = new short[4];
        neighbors = new Tile[4];
        walls = new boolean[4];

        for(int i = 0; i < 4; i++) {
            dirAttempted[i] = 0;
            neighbors[i] = null;
            walls[i] = true;
        }  
        
    }
    
    public void Draw(Graphics g, Vector2 pos, Vector2 size) {
        // draw the square
        Color c = null;
        switch(type) {
            case START:
                c = Color.CYAN;
                break;
            case GOAL:
                c = Color.GREEN;
                break;
            case NORMAL:
                c = Color.WHITE;
                break;
        }
        g.setColor(c);
        g.fillRect((int)pos.x, (int)pos.y, (int)size.x, (int)size.y);
        
        // draw the walls, if any
        g.setColor(Color.BLACK);
        Vector2 startPos, endPos;
        for(int i=0;i<4;i++) {
            if(walls[i]) {
                if(Direction.values()[i] == Direction.UP) {
                    startPos = pos;
                    endPos = Vector2.Add(pos, Vector2.Scale(Vector2.Right(), size.x));
                }
                else if(Direction.values()[i] == Direction.DOWN) {
                    startPos = Vector2.Add(pos, Vector2.Scale(Vector2.Down(), size.y));
                    endPos = Vector2.Add(pos, size);
                }
                else if(Direction.values()[i] == Direction.LEFT) {
                    startPos = pos;
                    endPos = Vector2.Add(pos, Vector2.Scale(Vector2.Down(), size.y));
                }
                else { // RIGHT
                    startPos = Vector2.Add(pos, Vector2.Scale(Vector2.Right(), size.x));
                    endPos = Vector2.Add(pos, size);
                }
                g.drawLine((int)startPos.x, (int)startPos.y, (int)endPos.x, (int)endPos.y);
            }
        }
        /*
        // draw the attempt lines
        g.setColor(Color.GRAY);
        for(int i=0;i<4;i++) {
            if(dirAttempted[i] > 0) {
                int thickness = 1;
                if(dirAttempted[i] > 1) {
                    thickness = 3;
                }
                int width, height;
                if(Direction.values()[i] == Direction.UP) {
                    startPos = Vector2.Add(pos, Vector2.Scale(Vector2.Right(), size.x * 0.5));
                    width = thickness;
                    height = (int)(size.y * 0.5);
                }
                else if(Direction.values()[i] == Direction.DOWN) {
                    startPos = Vector2.Add(pos, new Vector2(size.x* 0.5, size.y));
                    width = thickness;
                    height = (int)(size.y * 0.5);
                }
                else if(Direction.values()[i] == Direction.LEFT) {
                    startPos = Vector2.Add(pos, Vector2.Scale(Vector2.Down(), size.y * 0.5));
                    width = (int)(size.x * 0.5);
                    height = thickness;
                }
                else { // RIGHT
                    startPos = Vector2.Add(pos, new Vector2(size.x, size.y * 0.5));
                    width = (int)(size.x * 0.5);
                    height = thickness;
                }
                g.fillRect((int)startPos.x, (int)startPos.y, width, height);
            }            
        }
        
         */
    }
    
    public boolean isGoal() {
        return type == TileType.GOAL;
    }
    
    public boolean isStart() {
        return type == TileType.START;
    }
    
    public void AddNeighbor(Tile t, Direction dir) {
        neighbors[dir.ordinal()] = t;
    }
    
    public Tile[] GetNeighbors() {
        return neighbors;
    }
    
    public Direction GetNeighborDirection(Tile n) {
        for(int i = 0; i < 4; i++) {
            if(neighbors[i] == n) {
                return Direction.values()[i];
            }
        }
        return null;
    }
    
    public boolean SurroundedByWalls() {
        for(int i = 0; i < 4; i++) {
            if(!walls[i]) {
                return false;
            }
        }
        return true;
    }
    
    public void SetType(TileType t) {
        type = t;
    }
    
    public TileType GetType() {
        return type;
    }
    
    public void Reset() {
        type = TileType.NORMAL;
        for(int i = 0; i < 4; i++) {
            dirAttempted[i] = 0;
            walls[i] = true;
        }  
    }
}
