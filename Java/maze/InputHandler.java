//------------------------------------------------------------------------------
//
//  InputHandler.java
//
//  © 2011 Charlie Huguenard
//
//------------------------------------------------------------------------------
package maze;

import java.awt.event.*;

public class InputHandler 
implements KeyListener, FocusListener, MouseListener, MouseMotionListener {
    private Main main;
    
    public InputHandler(Main main) {
        this.main = main;
    }
    
    @Override
    public void keyTyped(KeyEvent e) {}
    @Override
    public void keyPressed(KeyEvent e) {}
    @Override
    public void keyReleased(KeyEvent e) {}

    @Override
    public void focusGained(FocusEvent e) {}
    @Override
    public void focusLost(FocusEvent e) {}


    @Override
    public void mousePressed(MouseEvent e) {
        System.out.println("mouseDown: " + e.getButton());
    }
    
    @Override
    public void mouseReleased(MouseEvent e) {
        System.out.println("mouseUp: " + e.getButton());
        //main.maze.Generate();
    }

    @Override
    public void mouseMoved(MouseEvent e) {
    }
    
    @Override
    public void mouseEntered(MouseEvent e) {}
    @Override
    public void mouseExited(MouseEvent e) {}
    @Override
    public void mouseDragged(MouseEvent e) {}
    @Override
    public void mouseClicked(MouseEvent e) {}
}
