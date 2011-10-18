using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

public class InputManager : GameComponent
{
    private KeyboardState oldKBState;
    private KeyboardState newKBState;

    public delegate void KBEvent(KeyboardState state);
    public event KBEvent KB_Escape;     // pressed escape
    public event KBEvent KB_Any;        // pressed the 'Any' key

    public InputManager(Game game)
        : base(game)
    {
        game.Components.Add(this);
    }

    public override void Update(GameTime gameTime)
    {
        newKBState = Keyboard.GetState();

        if (newKBState != oldKBState)
        {
            if (KB_Escape != null && newKBState.IsKeyDown(Keys.Escape))
                KB_Escape(newKBState);
            else if (KB_Any != null && newKBState.GetPressedKeys().Length > 0)
                KB_Any(newKBState);
        }

        oldKBState = newKBState;
    }
}