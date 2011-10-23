//------------------------------------------------------------------------------
//
//  Maze.java
//
//  © 2011 Charlie Huguenard
//
//------------------------------------------------------------------------------
package maze;

import java.awt.Graphics;
import java.util.ArrayList;
import java.util.Random;
import java.util.Stack;
import maze.Enums.*;

public class Maze {
    private static final int COLS = 38;
    private static final int ROWS = 21;
    private static final int TILE_SIZE = 20;
    private static final Vector2 TOP_LEFT = new Vector2(20,20);
    private static final Random rand = new Random();
    
    private Tile[][] tiles;
    private Robot robot;
    
    public Maze() {
        robot = new Robot(TOP_LEFT, this);
        tiles = new Tile[COLS][ROWS];
        Init();
    }
    
    public final void Init() {
        for(int x = 0; x < COLS; x++) {
            for(int y = 0; y < ROWS; y++) {
                tiles[x][y] = new Tile();
                // add neighbors
                if(x-1 >= 0) {
                    tiles[x][y].AddNeighbor(tiles[x-1][y], Direction.LEFT);
                    tiles[x-1][y].AddNeighbor(tiles[x][y], Direction.RIGHT);
                }
                if(y-1 >= 0) {
                    tiles[x][y].AddNeighbor(tiles[x][y-1], Direction.UP);
                    tiles[x][y-1].AddNeighbor(tiles[x][y], Direction.DOWN);
                }
            }
        }
        Generate();
    }
    
    public int GetTileSize() {
        return TILE_SIZE;
    }
    
    public void Clear() {
        for(int x = 0; x < COLS; x++) {
            for(int y = 0; y < ROWS; y++) {
                tiles[x][y].Reset();
            }
        }
    }
    
    public void Generate() {
        Clear();
        /*  PSEUDOCODE!
            create a CellStack (LIFO) to hold a list of cell locations  
            set TotalCells = number of cells in grid  
            choose a cell at random and call it CurrentCell  
            set VisitedCells = 1  

            while VisitedCells < TotalCells 
                find all neighbors of CurrentCell with all walls intact   
                if one or more found 
                    choose one at random  
                    knock down the wall between it and CurrentCell  
                    push CurrentCell location on the CellStack  
                    make the new cell CurrentCell  
                    add 1 to VisitedCells
                else 
                    pop the most recent cell entry off the CellStack  
                    make it CurrentCell
                endIf
            endWhile  
         */
        Stack<Tile> tileStack = new Stack<Tile>();
        int totalTiles = COLS * ROWS;
        
        int randCol = rand.nextInt(COLS);
        int randRow = rand.nextInt(ROWS);
        Tile curTile = tiles[randCol][randRow];
        curTile.SetType(TileType.START);
        
        // set up the robot
        Vector2 start = Vector2.Add(TOP_LEFT, new Vector2(randCol * TILE_SIZE, randRow * TILE_SIZE));
        robot.SetPos(start);
        robot.SetStartPos(start);
        
        int visitedTiles = 1;
        
        boolean endOfPath = true;
        int longestPath = 0;
        int curPathLength = 0;
        Tile lastGoal = null;
        
        while(visitedTiles < totalTiles) {
            // find all neighbors of CurrentCell with all walls intact
            ArrayList<Tile> validNeighbors = new ArrayList<Tile>();
            for(int i = 0; i < 4; i++) {
                Tile n = curTile.GetNeighbors()[i];
                if(n != null && n.SurroundedByWalls()) {
                    validNeighbors.add(n);
                }
            }
            // if one or more found
            if(validNeighbors.size() > 0) {
                //choose one at random
                Tile newTile = validNeighbors.get(rand.nextInt(validNeighbors.size()));
                //knock down the wall between it and CurrentCell
                newTile.walls[newTile.GetNeighborDirection(curTile).ordinal()] = false;
                curTile.walls[curTile.GetNeighborDirection(newTile).ordinal()] = false;
                //push CurrentCell location on the CellStack  
                tileStack.push(curTile);
                //make the new cell CurrentCell
                curTile = newTile;
                // add 1 to visitedTiles
                visitedTiles++;
                
                endOfPath = true;
                curPathLength++;
            }
            else {
                if(tileStack.size() > 0) {
                    if(endOfPath && curPathLength > longestPath) {
                        curTile.SetType(TileType.GOAL);
                        endOfPath = false;
                        longestPath = curPathLength;
                        curPathLength = 0;
                        if(lastGoal != null) {
                            lastGoal.SetType(TileType.NORMAL);
                        }
                        lastGoal = curTile;
                    }
                    // pop the most recent cell entry off the 
                    // tileStack and make it curTile
                    curTile = (Tile)tileStack.pop();
                }
                else {
                    break;
                }
            }
        }
    }
    
    public void Draw(Graphics g) {
        Vector2 pos = Vector2.Zero();
        for(int x = 0; x < COLS; x++) {
            for(int y = 0; y < ROWS; y++) {
                pos = Vector2.Add(TOP_LEFT, new Vector2(x * TILE_SIZE, y * TILE_SIZE));
                tiles[x][y].Draw(g, pos, new Vector2(TILE_SIZE, TILE_SIZE));
            }
        }
        robot.Draw(g);
    }
    
    public void Update(float deltaTime) {
        robot.Update(deltaTime);
    }
    
    public Tile GetTileAtPos(Vector2 pos) {
        Vector2 offset = Vector2.Subtract(pos, TOP_LEFT);
        int x = (int)(offset.x / TILE_SIZE);
        int y = (int)(offset.y / TILE_SIZE);
        return tiles[x][y];
    }
    
    // Holy crap, this is awful. Refactor!
    public Vector2 GetTilePos(Tile t) {
        for(int x = 0; x < COLS; x++) {
            for(int y = 0; y < ROWS; y++) {
                if(tiles[x][y] == t) {
                    return Vector2.Add(TOP_LEFT, new Vector2(x * TILE_SIZE, y * TILE_SIZE));
                }
            }
        }
        System.out.println("Uh, that tile's not here dude.");
        return Vector2.Zero();
    }
}
