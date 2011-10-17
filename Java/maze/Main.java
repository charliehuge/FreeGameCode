//------------------------------------------------------------------------------
//
//  Main.java
//
//  © 2011 Charlie Huguenard
//
//------------------------------------------------------------------------------
package maze;

import java.awt.*;
import java.awt.image.*;
import javax.swing.*;

public class Main extends Canvas implements Runnable {

    private static final int WIDTH = 16;
    private static final int HEIGHT = 9;
    private static final int SCALE = 50;
    
    private Thread thread;
    private boolean running;
    private boolean hadFocus;
    
    private InputHandler inputHandler;
    Maze maze;
    
    public Main() {
        Dimension size = new Dimension(WIDTH * SCALE, HEIGHT * SCALE);
        setSize(size);
        setPreferredSize(size);
        setMinimumSize(size);
        setMaximumSize(size);
        
        inputHandler = new InputHandler(this);
        addKeyListener(inputHandler);
        addFocusListener(inputHandler);
        addMouseListener(inputHandler);
        addMouseMotionListener(inputHandler);
        
        maze = new Maze();
    }
    
    public synchronized void start() {
        if (running) return;
        running = true;
        thread = new Thread(this);
        thread.start();
    }

    public synchronized void stop() {
        if (!running) return;
        running = false;
        try {
            thread.join();
        } catch (InterruptedException e) {
            e.printStackTrace();
        }
    }
        
    @Override
    public void run() {
        int frames = 0;

        double unprocessedSeconds = 0;
        long lastTime = System.nanoTime();
        double maxFPS = 60.0;
        double secondsPerTick = 1 / maxFPS;
        int tickCount = 0;

        requestFocus();

        while (running) {
            long now = System.nanoTime();
            long passedTime = now - lastTime;
            lastTime = now;
            if (passedTime < 0) passedTime = 0;
            if (passedTime > 100000000) passedTime = 100000000;

            unprocessedSeconds += passedTime / 1000000000.0;

            boolean ticked = false;
            while (unprocessedSeconds > secondsPerTick) {
                tick((float)(passedTime*0.00000001));
                unprocessedSeconds -= secondsPerTick;
                ticked = true;

                tickCount++;
                if (tickCount % maxFPS == 0) {
                    //System.out.println(frames + " fps");
                    lastTime += 1000;
                    frames = 0;
                }
            }

            if (ticked) {
                render();
                frames++;
            } 
            else {
                try {
                    Thread.sleep(1);
                } catch (InterruptedException e) {
                    e.printStackTrace();
                }
            }
        }
    }
    
    private void render() {
        if (hadFocus != hasFocus()) {
            hadFocus = !hadFocus;
        }
        BufferStrategy bs = getBufferStrategy();
        if (bs == null) {
            createBufferStrategy(3);
            return;
        }

        Graphics g = bs.getDrawGraphics();
        g.setColor(Color.PINK);
        g.fillRect(0, 0, getWidth(), getHeight());
        
        maze.Draw(g);
        
        g.dispose();
        bs.show();
    }
    
    private void tick(float deltaTime) {
        if (hasFocus()) {
            maze.Update(deltaTime);
        }
    }   
    
    public static void main(String[] args) {
        Main game = new Main();
        
        JFrame frame = new JFrame("OMG Maze!!!1!!1");

        JPanel panel = new JPanel(new BorderLayout());
        panel.add(game, BorderLayout.CENTER);

        frame.setContentPane(panel);
        frame.pack();
        frame.setLocationRelativeTo(null);
        frame.setResizable(false);
        frame.setDefaultCloseOperation(JFrame.EXIT_ON_CLOSE);
        frame.setVisible(true);

        game.start();
    }
}
