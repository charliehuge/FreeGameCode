//------------------------------------------------------------------------------
//
//  Robot.java
//
//  © 2011 Charlie Huguenard
//
//------------------------------------------------------------------------------
package maze;
import java.awt.Color;
import java.awt.Graphics;
import java.util.ArrayList;
import java.util.Random;
import maze.Enums.*;

public class Robot {
    private static enum Algorithm { WALLFLOWER, TREMAUX, RANDOM };
    private static final Random rand = new Random();
    
    private Vector2 startPos;
    private Vector2 pos;
    private Direction dir;
    private Maze maze;
    private float timeSinceUpdate = 0f;
    private boolean reachedGoal = false;
    private double totalTime = 0.0;
    private long totalSteps = 0;
    private Algorithm algo = Algorithm.WALLFLOWER;
    
    public Robot(Vector2 startPos, Maze maze) {
        this.startPos = startPos;
        pos = startPos;
        dir = Direction.UP;
        this.maze = maze;
    }
    
    public void Draw(Graphics g) {
        g.setColor(Color.red);
        int tileSize = maze.GetTileSize();
        g.fillOval((int)pos.x, (int)pos.y, tileSize, tileSize);
    }
    
    public void SetPos(Vector2 pos) {
        this.pos = pos;
    }
    
    public void SetStartPos(Vector2 pos) {
        this.startPos = pos;
    }
    
    public void Update(float deltaTime) {
        timeSinceUpdate += deltaTime;
        if(!reachedGoal && timeSinceUpdate > 0.01f) {
            totalTime += timeSinceUpdate;
            timeSinceUpdate = 0f;
            
            Tile curTile = maze.GetTileAtPos(pos);
            if(curTile.GetType() == TileType.GOAL) {
                System.out.println(algo.toString() + " found the end in " + totalTime + "s (" + totalSteps + " steps)");
                // next algo, looping
                int nextAlgoInt = algo.ordinal() + 1;
                if(nextAlgoInt == Algorithm.values().length) {
                    maze.Generate();
                    nextAlgoInt = 0;
                }
                algo = Algorithm.values()[nextAlgoInt];
                totalSteps = 0;
                totalTime = 0f;
                timeSinceUpdate = 0f;
                pos = startPos;
                return;
            }
            
            totalSteps++;
            
            switch(algo) {
                case WALLFLOWER:
                    WallflowerUpdate(curTile);
                    break;
                case TREMAUX:
                    TremauxUpdate(curTile);
                    break;
                default:
                    RandomUpdate(curTile);
                    break;
            }
        }
    }
    
    private void WallflowerUpdate(Tile curTile) {
        // wallfower solver - just makes a right turn whenever it can
        while(true) {
            // if it can move to the right, do so
            if(!curTile.walls[(dir.ordinal() + 1) % 4]) {
                dir = Direction.values()[(dir.ordinal() + 1) % 4];
                MoveToTile(curTile, curTile.GetNeighbors()[dir.ordinal()]);       
                break;
            }
            // if it can move forward, do so
            else if(!curTile.walls[dir.ordinal()]) {
                MoveToTile(curTile, curTile.GetNeighbors()[dir.ordinal()]);       
                break;
            }
            // else, turn left and repeat
            else {
                int dirInt = (dir.ordinal() - 1) % 4;
                if(dirInt < 0) dirInt += 4;
                dir = Direction.values()[dirInt];
            }
        }
    }
    
    private void RandomUpdate(Tile curTile) {
        // random solver - if the direction it's already headed is blocked, 
        // pick a random direction that's not the way it came from
        Direction opposite = Direction.values()[(dir.ordinal() + 2) % 4];
        // find out how many options there are
        ArrayList<Integer> options = new ArrayList<Integer>();
        for(int i = 0; i < 4; i++) {
            if(!curTile.walls[i]) {
                options.add(i);
            }
        }
        
        // if there are more than 2 options, 
        // pick a random one that's not the way it came from
        if(options.size() > 2) {
            do {
                dir = Direction.values()[options.get(rand.nextInt(options.size()))];
            } while(dir == opposite);            
        }
        // if there are 2 options, pick the one that's not the way it came from
        else if(options.size() == 2) {
            for(int dirInt : options) {
                if(dirInt != opposite.ordinal()) {
                    dir = Direction.values()[dirInt];
                    break;
                }
            }
        }
        // else, turn around
        else {
            dir = opposite;
        }

        pos = maze.GetTilePos(curTile.GetNeighbors()[dir.ordinal()]);
    }
    
    private void TremauxUpdate(Tile curTile) {
        // Tremaux's Algorithm
        ArrayList<Integer> validPaths = new ArrayList<Integer>();
        int leastMarks = 9999;
        for(int dirInt = 0; dirInt < 4; dirInt++) {
            if(!curTile.walls[dirInt] 
                    && curTile.dirAttempted[dirInt] < leastMarks) 
                leastMarks = curTile.dirAttempted[dirInt];
        }
        for(int dirInt = 0; dirInt < 4; dirInt++) {
            if(!curTile.walls[dirInt] 
                    && curTile.dirAttempted[dirInt] == leastMarks) 
                validPaths.add(dirInt);
        }
        // pick the path with the least marks
        // (or random between those sharing the least number of marks)
        if(validPaths.size() > 1) {
            int dirInt = -1;
            do {
                dirInt = rand.nextInt(4);
            } while(dirInt == -1 || !validPaths.contains(dirInt));
            dir = Direction.values()[dirInt];
        }
        else if(validPaths.size() == 1) {
            dir = Direction.values()[validPaths.get(0)];
        }
        else {
            System.out.println("No valid paths!");
            return;
        }
        
        MoveToTile(curTile, curTile.GetNeighbors()[dir.ordinal()]);       
    }
    
    public void MoveToTile(Tile fromTile, Tile toTile) {
        // move in the direction
        pos = maze.GetTilePos(toTile);
        // mark the direction you went on cur and new tile
        fromTile.dirAttempted[dir.ordinal()]++;
        fromTile.GetNeighbors()[dir.ordinal()].dirAttempted[(dir.ordinal() + 2) % 4]++;
    }
}
